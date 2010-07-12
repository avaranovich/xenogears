namespace XenoGears.Ogre.Exploration
{
    public class Pair<K, V>
    {
        public K Key { get; private set; }
        public V Value { get; private set; }

        public Pair(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}