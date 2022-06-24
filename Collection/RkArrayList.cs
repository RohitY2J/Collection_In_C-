using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection
{
    [DebuggerDisplay("Count={Count}")]
    [DebuggerTypeProxy(typeof(ArrayDebugView))]
    class RkArrayList<T>
    {
        // Fields
        private const int GROW_BY = 10;        
        private int m_count;                //count
        private T[] m_data;                 //array to store data
        private int m_updateCode;           // to know if the collection has changed while iterating over

        #region Methods for Initialization
        // Constructors
        public RkArrayList()
        {
            //m_data = new T[GROW_BY];
            Initialize(GROW_BY);
        }

        public RkArrayList(IEnumerable<T> items)
        {
            try
            {
                Initialize(GROW_BY);
                foreach (var item in items)
                {
                    Add(item);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public RkArrayList(int capacity)
        {
            //m_data = new T[capacity];
            Initialize(capacity);
        }
        private void Initialize(int capacity)
        {
            m_data = new T[capacity];
        }
        #endregion



        #region Method for crud operation
        public void Add(T item)
        {
            if (m_data.Length <= m_count)
            {
                Capacity += GROW_BY;
            }

            m_data[m_count++] = item;
            ++m_updateCode;
        }
        public void Insert(int index, T item)
        {
            if (index < 0 && index > m_count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (m_count + 1>= Capacity)
            {
                Capacity = m_count + GROW_BY;
            }

            for (int i = m_count; i > index && i > 0; i--)
            {
                m_data[i] = m_data[i-1];
            }

            m_data[index] = item;
            ++m_count;
            ++m_updateCode;
        }
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= m_count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                return m_data[index];
            }
            set
            {
                if (index < 0 || index >= m_count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                m_data[index] = value;
                ++m_updateCode;
            }
        }
        public bool Remove(T item, bool allOccurrences = false)
        {
            int shiftto = 0;
            bool shiftmode = false;
            bool removed = false;
            int count = m_count;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < count; ++i)
            {
                if (comparer.Equals(m_data[i], item) && (allOccurrences || !shiftmode))
                {
                    // Decrement the count since we have found an instance
                    --m_count;
                    removed = true;
                    // Check to see if we have already found one occurrence of the
                    // item we are removing
                    if (!shiftmode)
                    {
                        // We will start shifting to the position of the first occurrence.
                        shiftto = i;
                        // Enable shifting
                        shiftmode = true;
                    }
                    continue;
                }
                if (shiftmode)
                {
                    // Since we are shifting elements we need to shift the element
                    // down and then update the shiftto index to the next element.
                    m_data[shiftto++] = m_data[i];
                }
            }
            for (int i = m_count; i < count; ++i)
            {
                m_data[i] = default(T);
            }
            if (removed)
            {
                ++m_updateCode;
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            if(index < 0 && index >= m_count)
            {
                throw new ArgumentOutOfRangeException();
            }
            int count = Count;
            
            //for(int i = index; i < m_count; i++)
            //{
            //    m_data[i] = m_data[i + 1];
            //}

            for(int i = index+1; i<m_count; i++)
            {
                m_data[i - 1] = m_data[i]; 
            }

            --m_count;
            ++m_updateCode;

            m_data[m_count] = default(T);
        }

        public void Clear()
        {
            Array.Clear(m_data, 0, m_count);
            m_count = 0;
            ++m_updateCode;
        }
        #endregion


        #region (Helper Methods and Properties) 
        //Methods For Checking status of array 
        public bool Contains(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < m_count; i++)
            {
                if (comparer.Equals(m_data[i], item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the index of the specified item.
        /// </summary>
        /// <param name="item">The item to get the index of.</param>
        /// <returns>
        /// -1 if the item isn't found in the array, the index of the first instance of the
        /// item other
        public int IndexOf(T item)
        {
            return Array.IndexOf<T>(m_data, item, 0, m_count);
        }

        /// <summary>
        /// Gets or sets an element in the ArrayEx(T).
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>The value of the element.</returns>


        public T[] ToArray()
        {
            T[] tmp = new T[Count];
            for (int i = 0; i < Count; ++i)
            {
                tmp[i] = m_data[i];
            }
            return tmp;
        }

        static string ArrayToString(Array array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (array.Length > 0)
            {
                sb.Append(array.GetValue(0));
            }
            for (int i = 1; i < array.Length; ++i)
            {
                sb.AppendFormat(",{0}", array.GetValue(i));
            }
            sb.Append("]");
            return sb.ToString();
        }

        // Properties
        public int Capacity
        {
            get { return m_data.Length; }
            set
            {
                // We do not support truncating the stored array.
                // So throw an exception if the array is less than Count.
                if (value < Count)
                {
                    throw new ArgumentOutOfRangeException("value", "The value is less than Count");
                }
                // We do not need to do anything if the newly specified capacity
                // is the same as the old one.
                if (value == Capacity)
                {
                    return;
                }
                // We will need to create a new array and move all of the
                // values in the old array to the new one
                T[] tmp = new T[value];
                for (int i = 0; i < Count; ++i)
                {
                    tmp[i] = m_data[i];
                }
                m_data = tmp;
                ++m_updateCode;
            }
        }

        public int Count { 
            get 
            { 
                return m_count; 
            } 
        }
        
        public bool IsEmpty 
        { 
            get 
            { 
                return m_count <= 0; 
            } 
        }
        #endregion


    }
}
