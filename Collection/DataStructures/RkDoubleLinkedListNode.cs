using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection.DataStructures
{
    public class RkDoubleLinkedListNode<T>
    {
        RkDoubleLinkedList<T> m_owner;
        RkDoubleLinkedListNode<T> m_prev;
        RkDoubleLinkedListNode<T> m_next;
        T m_data;
        /// <summary>
        /// Initializes a new instance of the DoubleLinkedListNode(T) class
        /// with the specified data.
        /// </summary>
        /// <param name="data">The data that this node will contain.</param>
        public RkDoubleLinkedListNode(T data)
        {
            m_data = data;
            m_owner = null;
        }

        internal RkDoubleLinkedListNode(RkDoubleLinkedList<T> owner, T data)
        {
            m_data = data;
            m_owner = owner;
        }
        /// <summary>
        /// Gets the next node.
        /// </summary>
        public RkDoubleLinkedListNode<T> Next
        {
            get { return m_next; }
            internal set { m_next = value; }
        }
        /// <summary>
        /// Gets or sets the owner of the node.
        /// </summary>
        internal RkDoubleLinkedList<T> Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }
        /// <summary>
        /// Gets the previous node.
        /// </summary>
        public RkDoubleLinkedListNode<T> Previous
        {
            get { return m_prev; }
            internal set { m_prev = value; }
        }
        /// <summary>
        /// Gets the data contained in the node.
        /// </summary>
        public T Data
        {
            get { return m_data; }
            internal set { m_data = value; }
        }
    }
}
