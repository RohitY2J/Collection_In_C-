using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Collection.DataStructures
{
    class RkSingleLinkedList<T>
    {
        #region Field
        // Fields
        private int m_count;                      //number of nodes in the list
        private RkSingleLinkedListNode<T> m_head; //references to the first node
        private RkSingleLinkedListNode<T> m_tail; //references to the last node
        private int m_updateCode;                 //collection has changed or not.
        #endregion

        #region Constructors
        public RkSingleLinkedList()
        {
        }

        public RkSingleLinkedList(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                AddToEnd(item);
            }
        }

        #endregion

        public void AddValidation(RkSingleLinkedListNode<T> node, RkSingleLinkedListNode<T> newNode)
        {
            if(node == null)
            {
                throw new ArgumentNullException("node");
            }
            if(newNode == null)
            {
                throw new ArgumentNullException("newNode");
            }
            if(node.Owner != this)
            {
                throw new InvalidOperationException("node is not owened by this list");
            }
            if(newNode.Owner != this)
            {
                throw new InvalidOperationException("newNode is not owened by this list");
            }
        }

        #region Crud Operation Methods
        public RkSingleLinkedListNode<T> AddAfter(RkSingleLinkedListNode<T> node, T value)
        {
            RkSingleLinkedListNode<T> newNode = new RkSingleLinkedListNode<T>(this, value);

            AddValidation(node, newNode);
            if(m_tail != node) {
                RkSingleLinkedListNode<T> nextNode = node.Next;
                node.Next = newNode;
                newNode.Next = nextNode;
            }
            else
            {
                m_tail.Next = newNode;
                m_tail = newNode;
            }

            ++m_count;
            ++m_updateCode;

            return newNode;

        }
        public RkSingleLinkedListNode<T> AddBefore(RkSingleLinkedListNode<T> node, T value)
        {
            RkSingleLinkedListNode<T> newNode = new RkSingleLinkedListNode<T>(this, value);
            AddValidation(node, newNode);

            if(node == m_head)
            {
                newNode.Next = node;
                m_head = newNode;
            }
            else
            {
                RkSingleLinkedListNode<T> beforeNode = m_head;
                while(beforeNode != null && beforeNode.Next != node)
                {
                    beforeNode = beforeNode.Next;
                }

                beforeNode.Next = newNode;
                newNode.Next = node;
            }

            ++m_count;
            ++m_updateCode;

            return newNode;
        }

        public RkSingleLinkedListNode<T> AddToBeginning(T value)
        {
            RkSingleLinkedListNode<T> newNode = new RkSingleLinkedListNode<T>(this, value);
            if (IsEmpty)
            {
                m_head = newNode;
                m_tail = newNode;
            }
            else
            {
                var prevHead = m_head;
                m_head = newNode;
                m_head.Next = prevHead;
            }

            ++m_count;
            ++m_updateCode;

            return newNode;
        }
        
        public RkSingleLinkedListNode<T> AddToEnd(T value)
        {
            RkSingleLinkedListNode<T> newNode = new RkSingleLinkedListNode<T>(this, value);
            if (IsEmpty)
            {
                m_head = newNode;
            }
            else
            {
                m_tail.Next = newNode;
            }
            m_tail = newNode;

            ++m_count;
            ++m_updateCode;

            return newNode;
        }

        public void Clear()
        {
            RkSingleLinkedListNode<T> tmp;
            // Clean up the items in the list
            for (RkSingleLinkedListNode<T> node = m_head; node != null;)
            {
                tmp = node.Next;
                // Change the count and head pointer in case we throw an exception.
                // this way the node is removed before we clear the data
                m_head = tmp;
                --m_count;
                // Erase the contents of the node
                node.Next = null;
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

        public bool Remove(T item)
        {
            return Remove(item, false);
        }

        public bool Remove(T item, bool allOccurrences)
        {
            if (IsEmpty)
            {
                return false;
            }
            RkSingleLinkedListNode<T> prev = null;
            RkSingleLinkedListNode<T> curr = Head;
            bool removed = false;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            // Start traversing the list at the head
            while (curr != null)
            {
                // Check to see if the current node contains the data we are trying to delete
                if (!comparer.Equals(curr.Data, item))
                {
                    // Assign the current node to the previous node and
                    // the previous node to the current node
                    prev = curr;
                    curr = curr.Next;
                    continue;
                }
                // Create a pointer to the next node in the previous node
                if (prev != null)
                {
                    prev.Next = curr.Next;
                }
                if (curr == Head)
                {
                    // If the current node is the head we will have to assign
                    // the next node as the head
                    Head = curr.Next;
                }
                if (curr == Tail)
                {
                    // If the current node is the tail we will have to assign
                    // the previous node as the tail
                    Tail = prev;
                }
                // Save the pointer for clean up later
                RkSingleLinkedListNode<T> tmp = curr;
                // Advance the current to the next node
                curr = curr.Next;

                // Since the node will no longer be used clean up the pointers in it
                tmp.Next = null;
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

        public void Remove(RkSingleLinkedListNode<T> node)
        {
            if (IsEmpty)
            {
                return;
            }
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (node.Owner != this)
            {
                throw new InvalidOperationException("The node doesn't belong to this list.");
            }
            RkSingleLinkedListNode<T> prev = null;
            RkSingleLinkedListNode<T> curr = Head;
            // Find the node located before the specified node by traversing the list.
            while (curr != null && curr != node)
            {
                prev = curr;
                curr = curr.Next;
            }
            // The node has been found if the current node equals the node we are looking for
            if (curr == node)
            {
                // Assign the head to the next node if the specified node is the head
                if (m_head == node)
                {
                    m_head = node.Next;
                }
                // Assign the tail to the previous node if the specified node is the tail
                if (m_tail == node)
                {
                    m_tail = prev;
                }
                // Set the previous node next reference to the removed node's next reference.
                if (prev != null)
                {
                    prev.Next = curr.Next;
                }

                // Null out the removed node's next pointer to be safe.
                node.Next = null;
                node.Owner = null;
                
                --m_count;
                ++m_updateCode;
            }
        }
        #endregion


        #region Helper Methods and Properties
        public bool Contains(T data)
        {
            return Find(data) != null;
        }

        public RkSingleLinkedListNode<T> Find(T data)
        {
            if (IsEmpty)
            {
                return null;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for(RkSingleLinkedListNode<T> curr = m_head; curr != null; curr = curr.Next)
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
            for(RkSingleLinkedListNode<T> curr = m_head; curr != null; curr = curr.Next, ++index)
            {
                m_array[index] = curr.Data;
            }
            return m_array;
        }

        public int Count { 
            get
            {
                return m_count;
            } 
        }
        
        public RkSingleLinkedListNode<T> Head { 
            get
            {
                return m_head;
            }

            private set
            {
                m_head = value;
            }
        }
        
        public bool IsEmpty { 
            get
            {
                return m_count <= 0;
            }
        }
        public RkSingleLinkedListNode<T> Tail {
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
