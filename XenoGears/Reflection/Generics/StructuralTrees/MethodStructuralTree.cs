using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using XenoGears.Functional;

namespace XenoGears.Reflection.Generics.StructuralTrees
{
    // todo. see todo section for TypeStructuralTree
    [DebuggerNonUserCode]
    public class MethodStructuralTree : Dictionary<String, Type>
    {
        private MethodInfo Source { get; set; }

        public MethodStructuralTree(MethodInfo mi)
        {
            Source = mi;

            mi.DeclaringType.XGetGenericArguments().ForEach((targ, i) =>
                targ.GetStructuralTree().ForEach(kvp => Add("t" + "["+ i + "]" + kvp.Key, kvp.Value)));

            mi.XGetGenericArguments().ForEach((marg, i) =>
                marg.GetStructuralTree().ForEach(kvp => Add("m" + "["+ i + "]" + kvp.Key, kvp.Value)));

            // todo. there must be better and more permissive solution
            if (!(mi is MethodBuilder))
            {
                mi.ReturnType.GetStructuralTree().ForEach(kvp => Add("ret" + kvp.Key, kvp.Value));

                mi.GetParameters().Select(pi => pi.ParameterType).ForEach((parg, i) =>
                    parg.GetStructuralTree().ForEach(kvp => Add("a" + i + kvp.Key, kvp.Value)));
            }
        }

        public override string ToString()
        {
            return String.Format("[{0}]", this.Select(
                kvp => kvp.Key + " -> " + kvp.Value.GetBasisType().Name).StringJoin());
        }

        public MethodInfo BuildMethod()
        {
            try
            {
                var targc = Source.DeclaringType.XGetGenericArguments().Length;
                var margc = Source.XGetGenericArguments().Length;

                return Source.XMakeGenericMethod(
                    Enumerable.Range(0, targc).Select(key => Select("t[" + key + "]")).ToArray(),
                    Enumerable.Range(0, margc).Select(key => Select("m[" + key + "]")).ToArray());
            }
            catch (ArgumentException)
            {
                // todo. will fail if inferred type doesn't satisfy constraints
                // todo. and tbh atm I see no way to work around this shizzle
                return null;
            } 
        }

        public Type Select(String path)
        {
            if (ContainsKey(path))
            {
                var delimIndex = (path.StartsWith("t") || path.StartsWith("m")) ?
                    path.IndexOf("[", path.IndexOf("[") + 1) : path.IndexOf("[");

                var selector = delimIndex == -1 ? path : path.Substring(0, delimIndex);
                var subselector =
                    (selector.StartsWith("t") || selector.StartsWith("m")) ? int.Parse(
                        selector.Substring(2).Reverse().Skip(1).Reverse().StringJoin()) :
                    selector.StartsWith("a") ? int.Parse(selector.Substring(1)) : -1;

                var type =
                    path.StartsWith("t") ? Source.DeclaringType.XGetGenericArguments()[subselector] :
                    path.StartsWith("m") ? Source.XGetGenericArguments()[subselector] :
                    path.StartsWith("ret") ? Source.DeclaringType :
                    path.StartsWith("a") ? Source.GetParameters()[subselector].ParameterType : null;

                var tree = new TypeStructuralTree(type, this.Where(kvp => kvp.Key.StartsWith(selector))
                    .ToDictionary(kvp => kvp.Key.Substring(selector.Length), kvp => kvp.Value));
                var tpath = delimIndex == -1 ? String.Empty : path.Substring(delimIndex);
                return tree.Select(tpath);
            }
            else
            {
                throw new NotSupportedException(String.Format(
                    "Cannot select structural unit at path '{0}'. " +
                    "Reason: path is not supported by method structural tree '{1}'.",
                    path, this));
            }
        }

        public MethodStructuralTree Infer(String path, Type inferred)
        {
            if (!ContainsKey(path))
            {
                throw new NotSupportedException(String.Format(
                    "Structural unit at path '{0}' cannot be inferred. " +
                    "Reason: path is not supported by method structural tree '{1}'.",
                    path, this));
            }
            else
            {
                var unit = Select(path);
                if (!unit.IsGenericParameter)
                {
                    throw new NotSupportedException(String.Format(
                        "Structural unit at path '{0}' cannot be inferred. " +
                        "Reason: '{1}' is not a generic parameter.", path, unit));
                }
                else
                {
                    var mapping = Source.GetGenericArgsMapping();
                    var allOccurrences = mapping
                        .Where(mkvp => mkvp.Value == mapping[path]).Select(mkvp => mkvp.Key);
                    allOccurrences = allOccurrences.Concat(mapping[path].MkArray());

                    // yeh here we could just build a tree from Source.XMakeGenericType
                    // however, when I realized that I've already implemented this stuff
                    // in another way and was cba to rewrite the implementation.

                    foreach (var occ in allOccurrences)
                    {
                        this.RemoveElements(Keys.ToArray().Where(key => key.StartsWith(occ)));
                        inferred.GetStructuralTree().ForEach(kvp => Add(occ + kvp.Key, kvp.Value));
                    }

                    return this;
                }
            }
        }

        public MethodStructuralTree Infer(IEnumerable<KeyValuePair<String, Type>> batch)
        {
            return batch.Aggregate(this, (tree, kvp) => tree.Infer(kvp.Key, kvp.Value));
        }
    }
}