using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection.DataStructures
{
    class RkAssociativeArrayUsingArrList<TKey, TValue>
    {
        // Fields
        private IEqualityComparer<TKey> m_comparer;
        private RkDoubleLinkedList<KVPair> m_list;
        private int m_updateCode;
        // Constructor
        public RkAssociativeArrayUsingArrList() 
        {
            m_list = new RkDoubleLinkedList<KVPair>();
            m_comparer = EqualityComparer<TKey>.Default;
        }
        public RkAssociativeArrayUsingArrList(IEqualityComparer<TKey> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            m_comparer = comparer;
            m_list = new RkDoubleLinkedList<KVPair>();
        }

        #region Crud operation method
        public void Add(TKey key, TValue value)
        {
            Add(key, value, false);
        }
        private void Add(TKey key, TValue value, bool overwrite)
        {
            RkDoubleLinkedListNode<KVPair> node = FindKey(key);
            if(node != null)
            {
                if (!overwrite)
                {
                    throw new InvalidOperationException("The specified key is already present");
                }
                else
                {
                    KVPair tmp = node.Data;
                    tmp.Value = value;
                    node.Data = tmp;
                }
                return;
            }

            KVPair kvp = new KVPair(key, value);
            m_list.AddToBeginning(kvp);
            ++m_updateCode;
        }
        public void Clear()
        {
            m_list.Clear();
            ++m_updateCode;
        }
        public bool Remove(TKey key)
        {
            RkDoubleLinkedListNode<KVPair> node = FindKey(key);
            if(node == null)
            {
                return false;
            }
            ++m_updateCode;
            return m_list.Remove(node);
        }
        private bool Remove(RkDoubleLinkedListNode<KVPair> node)
        {
            ++m_updateCode;
            return m_list.Remove(node);
        }
        public bool RemoveValue(TValue value)
        {
            return RemoveValue(value, false);
        }    
        public bool RemoveValue(TValue value, bool allOccurrences)
        {
            bool removed = false;
            RkDoubleLinkedListNode<KVPair> node = FindValue(value);
            if (allOccurrences)
            {
                while (node != null)
                {
                    removed = m_list.Remove(node);
                    ++m_updateCode;
                    node = FindValue(value);
                }
                return removed;
            }
            else
            {
                ++m_updateCode;
                return m_list.Remove(node);
            }
        }
        #endregion

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = FindKey(key);
            if(node == null)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = node.Data.Value;
                return true;
            }
        }
        public bool ContainsKey(TKey key)
        {
            var node = FindKey(key);
            if(node != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ContainsValue(TValue value)
        {
            var node = FindValue(value);
            if (node != null)
            {
                return true;
            }
            else
                return false;
        }
        
        private RkDoubleLinkedListNode<KVPair> FindKey(TKey key)
        {
            if (IsEmpty)
            {
                return null;
            }
            for (RkDoubleLinkedListNode<KVPair> node = m_list.Head; node != null; node = node.Next)
            {
                if (m_comparer.Equals(node.Data.Key, key))
                {
                    return node;
                }
            }
            return null;
        }
        private RkDoubleLinkedListNode<KVPair> FindValue(TValue value)
        {
            if (IsEmpty)
            {
                return null;
            }
            EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
            for (RkDoubleLinkedListNode<KVPair> node = m_list.Head; node != null; node = node.Next)
            {
                if (comparer.Equals(node.Data.Value, value))
                {
                    return node;
                }
            }
            return null;
        }

        // Properties
        public int Count { 
            get 
            { 
                return m_list.Count; 
            } 
        }
        public bool IsEmpty { 
            get 
            { 
                return m_list.IsEmpty; 
            } 
        }

        public TValue this[TKey key] 
        { 
            get {
                var node = FindKey(key);
                if (node == null)
                {
                    throw new KeyNotFoundException("The specified key couldn't be located");
                }
                return node.Data.Value;
            }
            set { Add(key, value, true); } 
        }

        public TKey[] Keys { 
            get
            {
                TKey[] keys = new TKey[Count];
                int index = 0;
                for(var currNode = m_list.Head; currNode != null; currNode = currNode.Next)
                {
                    keys[index++] = currNode.Data.Key;
                }
                return keys;
            }
        }

        public TValue[] Values {
            get
            {
                TValue[] values = new TValue[Count];
                int index = 0;
                for (var currNode = m_list.Head; currNode != null; currNode = currNode.Next)
                {
                    values[index++] = currNode.Data.Value;
                }
                return values;
            }
        }

        // Nested Types
        private struct KVPair
        {
            private TKey m_key;
            private TValue m_value;
            public KVPair(TKey key, TValue value) {
                m_key = key;
                m_value = value;
            }
            public TKey Key { get { return m_key; } }
            public TValue Value { 
                get 
                { 
                    return m_value; 
                } 
                set 
                { 
                    m_value = value; 
                } 
            }
        }
    }
}
