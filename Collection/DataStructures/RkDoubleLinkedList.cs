using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection.DataStructures
{
    public class RkDoubleLinkedList<T>
    {
        // Fields
        private int m_count;
        private RkDoubleLinkedListNode<T> m_head;
        private RkDoubleLinkedListNode<T> m_tail;
        private int m_updateCode;
        
        #region Constructors
        // Constructors
        public RkDoubleLinkedList() { }
        public RkDoubleLinkedList(IEnumerable<T> items) {
            foreach(var item in items)
            {
                AddToEnd(item);
            }
        }
        #endregion

        // Methods
        public void AddValidation(RkDoubleLinkedListNode<T> node, RkDoubleLinkedListNode<T> newNode)
        {
            if(node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (newNode == null)
            {
                throw new ArgumentNullException("newNode");
            }

            if(newNode.Owner != this)
            {
                throw new InvalidOperationException("node is not owned by this list");
            }

            if (newNode.Owner != this)
            {
                throw new InvalidOperationException("newNode is not owned by this list");
            }
        }

        #region Crud Operation Method
        public RkDoubleLinkedListNode<T> AddAfter(RkDoubleLinkedListNode<T> node, T value) {
            RkDoubleLinkedListNode<T> newNode = new RkDoubleLinkedListNode<T>(this, value);
            AddValidation(node, newNode);
            if (m_tail == node) {
                m_tail.Next = newNode;
                newNode.Previous = m_tail;
                m_tail = newNode;
            }
            else
            {
                var nextNode = node.Next;
                if(nextNode != null)
                {
                    nextNode.Previous = newNode;
                    newNode.Next = nextNode;
                }
                node.Next = newNode;
                newNode.Previous = node;
            }
            ++m_count;
            ++m_updateCode;
            return newNode;
        }
        public RkDoubleLinkedListNode<T> AddBefore(RkDoubleLinkedListNode<T> node, T value) 
        {
            RkDoubleLinkedListNode<T> newNode = new RkDoubleLinkedListNode<T>(this, value);
            AddValidation(node, newNode);
            if(node == m_head)
            {
                m_head = newNode;
                node.Previous = newNode;
                newNode.Next = node;
            }
            else
            {
                var prevNode = node.Previous;
                if (prevNode != null)
                {
                    prevNode.Next = newNode;
                    newNode.Previous = prevNode;
                }
                node.Previous = newNode;
                newNode.Next = node;
            }
            ++m_count;
            ++m_updateCode;
            return newNode;
        }
        
        public RkDoubleLinkedListNode<T> AddToBeginning(T value) {
            RkDoubleLinkedListNode<T> newNode = new RkDoubleLinkedListNode<T>(this, value);
            if (IsEmpty)
            {
                m_head = newNode;
                m_tail = newNode;
            }
            else
            {
                m_head.Previous = newNode;
                newNode.Next = m_head;
                m_head = newNode;
            }
            ++m_count;
            ++m_updateCode;

            return newNode;
        }
        public RkDoubleLinkedListNode<T> AddToEnd(T value) {
            RkDoubleLinkedListNode<T> newNode = new RkDoubleLinkedListNode<T>(this, value);
            if (IsEmpty)
            {
                m_head = newNode;
                m_tail = newNode;
            }
            else
            {
                m_tail.Next = newNode;
                newNode.Previous = m_tail;
                m_tail = newNode;
            }
            ++m_count;
            ++m_updateCode;

            return newNode;
        }
        public void Clear()
        {
            RkDoubleLinkedListNode<T> tmp;
            // Clean up the items in the list
            for (RkDoubleLinkedListNode<T> node = m_head; node != null;)
            {
                tmp = node.Next;
                // Change the count and head pointer in case we throw an exception.
                // this way the node is removed before we clear the data
                m_head = tmp;
                if (tmp != null)
                {
                    tmp.Previous = null;
                }
                --m_count;
                // Erase the contents of the node
                node.Next = null;
                node.Previous = null;
                node.Owner = null;
                // Move to the next node
                node = tmp;
            }
            if (m_count <= 0)
            {
                m_head = null;
                m_tail = null;
            }
            ++m_updateCode;
        }
        public bool Remove(T item) {
            return Remove(item, false);
        }
        public bool Remove(RkDoubleLinkedListNode<T> node)
        {
            if (IsEmpty)
            {
                return false;
            }
            if (node == null)
            {
                throw new ArgumentNullException("node");

            }
            if (node.Owner != this)
            {
                throw new InvalidOperationException("The node doesn't belong to this list.");
            }
            RkDoubleLinkedListNode<T> prev = node.Previous;
            RkDoubleLinkedListNode<T> next = node.Next;
            // Assign the head to the next node if the specified node is the head
            if (m_head == node)
            {
                m_head = next;
            }
            // Assign the tail to the previous node if the specified node is the tail
            if (m_tail == node)
            {
                m_tail = prev;
            }
            // Set the previous node next reference to the removed nodes next reference.
            if (prev != null)
            {
                prev.Next = next;
            }
            // Set the next node prev reference to the removed nodes prev reference.
            if (next != null)
            {
                next.Previous = prev;
            }
            // Null out the removed nodes next and prev pointer to be safe.
            node.Previous = null;
            node.Next = null;
            node.Owner = null;
            --m_count;
            ++m_updateCode;
            return true;
        }
        public bool Remove(T item, bool allOccurrences) {
            if(IsEmpty)
            {
                return false;
            }
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            bool removed = false;
            RkDoubleLinkedListNode<T> curr = Head;
            while (curr != null)
            {
                // Check to see if the current node contains the data we are trying to delete
                if (!comparer.Equals(curr.Data, item))
                {
                    // Assign the current node to the previous node
                    // and the previous node to the current node
                    curr = curr.Next;
                    continue;
                }
                // Create a pointer to the next node in the previous node
                if (curr.Previous != null)
                {
                    curr.Previous.Next = curr.Next;
                }
                // Create a pointer to the previous node in the next node
                if (curr.Next != null)
                {
                    curr.Next.Previous = curr.Previous;
                }
                if (curr == Head)
                {
                    // If the current node is the head we will have to
                    // assign the next node as the head
                    Head = curr.Next;
                }
                if (curr == Tail)
                {
                    // If the current node is the tail we will have to
                    // assign the previous node as the tail
                    Tail = curr.Previous;
                }
                // Save the pointer for clean up later
                RkDoubleLinkedListNode<T> tmp = curr;
                // Advance the current to the next node
                curr = curr.Next;
                // Since the node will no longer be used clean up the pointers in it
                tmp.Next = null;
                tmp.Previous = null;
                tmp.Owner = null;
                // Decrement the counter since we have removed a node
                --m_count;
                removed = true;
                if (!allOccurrences)
                {
                    break;
                }
            }
            if (removed)
            {
                ++m_updateCode;
            }
            return removed;
        }
        #endregion


        #region Helper class and properties
        public bool Contains(T data) {
            return Find(data) != null;
        }

        public RkDoubleLinkedListNode<T> Find(T data) {
            if (IsEmpty)
            {
                return null;
            }
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for(var curr = m_head; curr != null; curr = curr.Next )
            {
                if(comparer.Equals(curr.Data, data))
                {
                    return curr;
                }
            }
            return null;
        }

        public T[] ToArray() 
        {
            T[] m_array = new T[Count];

            int index = 0;
            
            for(var curr = m_head; curr != null; curr = curr.Next, ++index)
            {
                m_array[index] = curr.Data;
            }

            return m_array;
        }
        public T[] ToArrayReversed() {
            T[] m_array = new T[Count];

            int index = 0;

            for (var curr = m_tail; curr != null; curr = curr.Previous, ++index)
            {
                m_array[index] = curr.Data;
            }

            return m_array;
        }
        // Properties
        public int Count {
            get
            {
                return m_count;
            }
        }
        public RkDoubleLinkedListNode<T> Head {
            get
            {
                return m_head;
            }
            private set {
                m_head = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return m_count <= 0;
            }
        }

        public RkDoubleLinkedListNode<T> Tail
        {
            get
            {
                return m_tail;
            }
            private set
            {
                m_tail = value;
            }
        }
        #endregion
    }
}
