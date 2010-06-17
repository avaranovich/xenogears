using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using XenoGears.Functional;

namespace XenoGears.Reflection.Generics.StructuralTrees
{
    // todo. Ideally, I'd hide all editing operations from the client
    // todo. However at this very moment I don't have enough time to implement this
    [DebuggerNonUserCode]
    public class TypeStructuralTree : Dictionary<String, Type>
    {
        private Type Source { get; set; }

        public TypeStructuralTree(Type t)
        {
            Source = t;
            Add(String.Empty, StripStructuralUnit(t));
            FillFlattenedStructuralTreeRecursive(t, String.Empty);
        }

        public TypeStructuralTree(Type t, IEnumerable<KeyValuePair<String, Type>> raw) 
        {
            Source = t;
            // todo. add structure and generic arg consistency checks
            this.AddElements(raw);
        }

        public override string ToString()
        {
            return String.Format("[{0}]", this.Select(
                kvp => kvp.Key + " -> " + kvp.Value.GetBasisType().Name).StringJoin());
        }

        private void FillFlattenedStructuralTreeRecursive(Type t, String level)
        {
            var index = 0;
            foreach (var subUnit in GetStructuralUnits(t))
            {
                var argLevel = level + "[" + index++ + "]";
                Add(argLevel, StripStructuralUnit(subUnit));
                FillFlattenedStructuralTreeRecursive(subUnit, argLevel);
            }
        }

        private static Type[] GetStructuralUnits(Type t)
        {
            if (t.IsGenericType)
            {
                return t.XGetGenericArguments();
            }
            else if (t.IsArray)
            {
                return t.GetElementType().MkArray();
            }
            else
            {
                return new Type[0];
            }
        }

        private static Type StripStructuralUnit(Type t)
        {
            if (t.IsGenericType && !t.IsGenericParameter)
            {
                return t.XGetGenericDefinition();
            }
            else if (t.IsArray)
            {
                return typeof(Object).MakeArrayType(t.GetArrayRank());
            }
            else
            {
                return t;
            }
        }

        public Type BuildType()
        {
            try
            {
                return Select(String.Empty);
            }
            catch(ArgumentException)
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
                var basis = this[path];
                if (basis.IsArray)
                {
                    return this[path + "[0]"].MakeArrayType(basis.GetArrayRank());
                }
                else
                {
                    Func<String, int> unitIndex = key =>
                    {
                        if (!key.StartsWith(path)) return -1;
                        var match = Regex.Match(key.Substring(path.Length), @"^\[(?<value>\d*)\]$");
                        return match.Success ? int.Parse(match.Result("${value}")) : -1;
                    };

                    var unitc = this.Max(kvp => unitIndex(kvp.Key)) + 1;
                    return basis.XMakeGenericType(Enumerable.Range(0, unitc)
                        .Select(i => Select(path + "[" + i + "]")).ToArray());
                }
            }
            else
            {
                throw new NotSupportedException(String.Format(
                    "Cannot select structural unit at path '{0}'. " +
                    "Reason: path is not supported by type structural tree '{1}'.",
                    path, this));
            }
        }

        public TypeStructuralTree Infer(String path, Type inferred)
        {
            if (!ContainsKey(path))
            {
                throw new NotSupportedException(String.Format(
                    "Structural unit at path '{0}' cannot be inferred. " +
                    "Reason: path is not supported by type structural tree '{1}'.",
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
                    if (path.IsNullOrEmpty())
                    {
                        this.RemoveElements(Keys.ToArray());
                        inferred.GetStructuralTree().ForEach(kvp => Add(kvp.Key, kvp.Value));
                    }
                    else
                    {
                        var mapping = Source.GetGenericArgsMapping();
                        var allOccurrences = mapping
                            .Where(mkvp => mkvp.Value == mapping[path]).Select(mkvp => mkvp.Key);

                        // yeh here we could just build a tree from Source.XMakeGenericType
                        // however, when I realized that I've already implemented this stuff
                        // in another way and was cba to rewrite the implementation.

                        foreach (var occ in allOccurrences)
                        {
                            this.RemoveElements(Keys.ToArray().Where(key => key.StartsWith(occ)));
                            inferred.GetStructuralTree().ForEach(kvp => Add(occ + kvp.Key, kvp.Value));
                        }
                    }

                    return this;
                }
            }
        }

        public TypeStructuralTree Infer(IEnumerable<KeyValuePair<String, Type>> batch)
        {
            return batch.Aggregate(this, (tree, kvp) => tree.Infer(kvp.Key, kvp.Value));
        }
    }
}