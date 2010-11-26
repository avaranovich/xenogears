using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XenoGears.Ogre.Reflection;

namespace XenoGears.Ogre.Exploration
{
    public class SimpleExplorer : Explorer
    {
        // child exploration config
        public Explore Explore { get; set; }
        public bool ShowTypeInProps { get; set; }
        public bool ShowToStringInProps { get; set; }

        // exploration categories config
        public Func<Object, bool> ExplorePropsCriterion { get; set; }
        public Func<Object, bool> ExploreElementsCriterion { get; set; }
        public bool ShowPropsForStandardCollections { get; set; }

        // child last-chance filter config
        public Func<Object, FieldInfo, bool> AcceptFieldCriterion { get; set; }
        public Func<Object, PropertyInfo, bool> AcceptPropCriterion { get; set; }
        public Func<Object, int, bool> AcceptElementCriterion { get; set; }
        public Func<String, Object, bool> AcceptNodeCriterion { get; set; }

        // drilldown termination config
        public List<Type> LeafTypes { get; set; }
        public Func<Object, bool> LeafCriterion { get; set; }

        public SimpleExplorer()
        {
            Explore = Explore.FieldsAndProperties | Explore.PublicAndPrivate;
            ShowTypeInProps = true;
            ShowToStringInProps = false;

            ExplorePropsCriterion = AllowExploreProps;
            ExploreElementsCriterion = AllowExploreElements;
            ShowPropsForStandardCollections = false;

            AcceptFieldCriterion = AllowField;
            AcceptPropCriterion = AllowProp;
            AcceptElementCriterion = AllowElement;
            AcceptNodeCriterion = AllowNode;

            LeafTypes = new List<Type>(typeof(IConvertible).GetMethods().Select(m => m.ReturnType));
            LeafTypes.Remove(typeof(Object));
            LeafTypes.Remove(typeof(TypeCode));
            LeafTypes.Add(typeof(Enum));
            LeafTypes.Add(typeof(Delegate));
            LeafTypes.Add(typeof(Type));
            LeafTypes.Add(typeof(Guid));
            LeafCriterion = o => o == null || this.LeafTypes.Any(bt => bt == o.GetType() || bt.IsAssignableFrom(o.GetType()));

            Logic = FilteredExploreImpl;
        }

        private IEnumerable<Pair<String, Object>> FilteredExploreImpl(Object obj)
        {
            foreach(var pair in ExploreImpl(obj))
            {
                if (AcceptNodeCriterion(pair.Key, pair.Value))
                {
                    yield return pair;
                }
            }
        }

        private bool AllowNode(String name, Object value)
        {
            return true;
        }

        private IEnumerable<Pair<String, Object>> ExploreImpl(Object obj)
        {
            if (obj != null)
            {
                if (!LeafCriterion(obj) && ShowTypeInProps)
                {
                    yield return new Pair<string, object>(
                        "$Type", SafeGet(() => obj.GetExplorableTypeInfo()));
                }

                if (!LeafCriterion(obj) && ShowToStringInProps)
                {
                    yield return new Pair<string, object>(
                        "$ToString", SafeGet(() => obj.ToString()));
                }

                if (ExplorePropsCriterion(obj))
                {
                    foreach (var pair in SafeExploreProps(obj))
                    {
                        yield return pair;
                    }
                }

                if (ExploreElementsCriterion(obj))
                {
                    foreach (var pair in SafeExploreElements(obj))
                    {
                        yield return pair;
                    }
                }
            }
        }

        private Object SafeGet(Func<Object> obj)
        {
            try
            {
                return obj();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private bool AllowExploreProps(Object obj)
        {
            if (LeafCriterion(obj))
            {
                return false;
            }
            else
            {
                if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
                {
                    if (ShowPropsForStandardCollections)
                    {
                        return true;
                    }
                    else
                    {
                        return 
                            obj.GetType().Namespace == null ||
                            (
                                !obj.GetType().IsArray &&
                                !obj.GetType().Namespace.StartsWith("System.Linq") &&
                                !obj.GetType().Namespace.StartsWith("System.Collections") &&
                                !obj.GetType().Namespace.StartsWith("Reflector.CodeModel.Memory")
                            );
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        private bool AllowField(Object root, FieldInfo fi)
        {
            return true;
        }

        private bool AllowProp(Object root, PropertyInfo pi)
        {
            return true;
        }

        private IEnumerable<Pair<String, Object>> SafeExploreProps(Object obj)
        {
            var scope = BindingFlags.Instance;
            if ((Explore & Explore.Private) != 0) scope |= BindingFlags.NonPublic;
            if ((Explore & Explore.Public) != 0) scope |= BindingFlags.Public;

            var outFields = new Dictionary<String, Object>();
            foreach (var f in obj.GetType().GetExplorableFields(scope))
            {
                if (AcceptFieldCriterion(obj, f))
                {
                    outFields.Add(f.Name.NormalizeFieldName(), SafeGet(() => f.GetValue(obj)));
                }
            }

            var outProps = new Dictionary<String, Object>();
            var props = obj.GetType().GetExplorableProperties(scope);
            foreach (var p in props)
            {
                var sameCase = p.Name;
                var camelCase = Char.ToLower(p.Name[0]) + p.Name.Substring(1);
                var _camelCase = "_" + camelCase;
                var m_undercase = "m" + _camelCase;

                Object sfval = null, cfval = null, ufval = null, mufval = null;
                var dupeName = outFields.TryGetValue(sameCase, out sfval);
                dupeName |= outFields.TryGetValue(camelCase, out cfval);
                dupeName |= outFields.TryGetValue(_camelCase, out ufval);
                dupeName |= outFields.TryGetValue(m_undercase, out mufval);

                // todo. not 100% correct since it doesn't take into account diff names avail at once
                var fval = sfval ?? cfval ?? ufval ?? mufval;
                var pval = SafeGet(() => p.GetValue(obj, null));

                if (dupeName)
                {
                    if (fval == null || pval == null)
                    {
                        if (fval == null && pval == null)
                        {
                            outFields.Remove(sameCase);
                            outFields.Remove(camelCase);
                            outFields.Remove(_camelCase);
                            outFields.Remove(m_undercase);
                        }
                    }
                    else
                    {
                        var ftype = fval.GetType();
                        var ptype = pval.GetType();

                        if (!(ftype.IsClass ^ ptype.IsClass))
                        {
                            if (ftype.IsClass && ptype.IsClass)
                            {
                                if (ReferenceEquals(fval, pval))
                                {
                                    outFields.Remove(sameCase);
                                    outFields.Remove(camelCase);
                                    outFields.Remove(_camelCase);
                                    outFields.Remove(m_undercase);
                                }
                            }
                            else
                            {
                                if (Equals(fval, pval))
                                {
                                    outFields.Remove(sameCase);
                                    outFields.Remove(camelCase);
                                    outFields.Remove(_camelCase);
                                    outFields.Remove(m_undercase);
                                }
                            }
                        }
                    }
                }

                if (AcceptPropCriterion(obj, p))
                {
                    outProps.Add(p.Name, pval);
                }
            }

            return outFields.Concat(outProps).Select(kvp => new Pair<String, Object>(kvp.Key, kvp.Value));
        }

        private bool AllowExploreElements(Object obj)
        {
            return !LeafCriterion(obj) && typeof(IEnumerable).IsAssignableFrom(obj.GetType());
        }

        private bool AllowElement(Object root, int index)
        {
            return true;
        }

        private IEnumerable<Pair<String, Object>> SafeExploreElements(Object obj)
        {
            IEnumerator iter = null;

            try
            {
                var index = 0;

                var exception = false;
                Pair<String, Object> yield = null;

                try
                {
                    iter = ((IEnumerable)obj).GetEnumerator();
                }
                catch (Exception ex)
                {
                    exception = true;
                    yield = new Pair<string, object>("$" + index, ex);
                }

                Func<IEnumerator, int, bool> safeMoveNext = (iter1, i) =>
                {
                    try
                    {
                        return iter.MoveNext();
                    }
                    catch (Exception ex)
                    {
                        exception = true;
                        yield = new Pair<string, object>("$" + i, ex);
                        return false;
                    }
                };

                while (!exception && safeMoveNext(iter, index))
                {
                    Object current = null;

                    try
                    {
                        current = iter.Current;
                    }
                    catch (Exception ex)
                    {
                        exception = true;
                        yield = new Pair<string, object>("$" + index, ex);
                    }

                    if (!exception)
                    {
                        if (AcceptElementCriterion(obj, index))
                        {
                            yield return new Pair<string, object>(
                                "$" + index++, current);
                        }
                    }
                }

                if (exception)
                {
                    if (yield != null)
                    {
                        if (AcceptElementCriterion(obj, index))
                        {
                            yield return yield;
                        }
                    }
                }
            }
            finally
            {
                var disposable = iter as IDisposable;
                if (disposable != null)
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch(Exception)
                    {
                        // ignore this
                    }
                }
            }
        }
    }
}