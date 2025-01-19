// Location: NetworkServiceProvider/DataStructures/CustomLinkedList.cs

using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkServiceProvider.DataStructures
{
    public class CustomLinkedList<T> : IEnumerable<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }

            public Node(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private Node head;
        private int count;

        public int Count => count;

        public void Add(T data)
        {
            Node newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            count++;
        }

        public bool Remove(T data)
        {
            if (head == null) return false;

            if (head.Data.Equals(data))
            {
                head = head.Next;
                count--;
                return true;
            }

            Node current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Equals(data))
                {
                    current.Next = current.Next.Next;
                    count--;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public bool Contains(T data)
        {
            Node current = head;
            while (current != null)
            {
                if (current.Data.Equals(data)) return true;
                current = current.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}