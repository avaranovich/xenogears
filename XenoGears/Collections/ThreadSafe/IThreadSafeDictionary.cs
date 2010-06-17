using System.Collections.Generic;

namespace XenoGears.Collections.ThreadSafe
{
    public interface IThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Merge is similar to the SQL merge or upsert statement.  
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="newValue">New Value</param>
        void MergeSafe(TKey key, TValue newValue);

        /// <summary>
        /// This is a blind remove. Prevents the need to check for existence first.
        /// </summary>
        /// <param name="key">Key to Remove</param>
        void RemoveSafe(TKey key);

        /// <summary>
        /// Merge is similar to the SQL merge or upsert statement.  
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="newValue">New Value</param>
        /// <param name="removed">Whether the an old value was removed</param>
        void MergeSafe(TKey key, TValue newValue, out bool removed);

        /// <summary>
        /// This is a blind remove. Prevents the need to check for existence first.
        /// </summary>
        /// <param name="key">Key to Remove</param>
        /// <param name="removed">Whether a value was removed.</param>
        void RemoveSafe(TKey key, out bool removed);

        bool TryAdd(TKey key, TValue value);
        bool TryRemove(TKey key);
    }
}
