using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection.DataStructures
{
    class RkSingleLinkedListNode<T>
    {
        RkSingleLinkedList<T> m_owner;
        RkSingleLinkedListNode<T> m_next;
        T m_data;

        #region Constructor
        public RkSingleLinkedListNode(T data)
        {
            m_data = data;
        }

        internal RkSingleLinkedListNode(RkSingleLinkedList<T> owner, T data)
        {
            m_data = data;
            m_owner = owner;
        }
        #endregion

        internal RkSingleLinkedListNode<T> Next
        {
            get { return m_next; }
            set { m_next = value; }
        }

        // internal keyword allow the property to be accessed from files present in the same assembly
        // assembly the project like assembly.dll
        internal RkSingleLinkedList<T> Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public T Data
        {
            get { return m_data; }
            internal set { m_data = value; }
        }
    }
}
