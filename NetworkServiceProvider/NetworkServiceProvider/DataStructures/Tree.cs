// Location: NetworkServiceProvider/DataStructures/Tree.cs

using System;
using System.Collections.Generic;

namespace NetworkServiceProvider.DataStructures
{
    public class Tree<T> where T : IComparable<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public Node(T data)
            {
                Data = data;
                Left = null;
                Right = null;
            }
        }

        private Node root;
        private int count;

        public int Count => count;

        public void Add(T data)
        {
            root = AddRecursive(root, data);
            count++;
        }

        private Node AddRecursive(Node node, T data)
        {
            if (node == null)
            {
                return new Node(data);
            }

            if (data.CompareTo(node.Data) < 0)
                node.Left = AddRecursive(node.Left, data);
            else if (data.CompareTo(node.Data) > 0)
                node.Right = AddRecursive(node.Right, data);

            return node;
        }

        public bool Contains(T data)
        {
            return ContainsRecursive(root, data);
        }

        private bool ContainsRecursive(Node node, T data)
        {
            if (node == null)
                return false;

            if (data.CompareTo(node.Data) == 0)
                return true;

            return data.CompareTo(node.Data) < 0
                ? ContainsRecursive(node.Left, data)
                : ContainsRecursive(node.Right, data);
        }

        public List<T> InOrderTraversal()
        {
            List<T> result = new List<T>();
            InOrderTraversalRecursive(root, result);
            return result;
        }

        private void InOrderTraversalRecursive(Node node, List<T> result)
        {
            if (node != null)
            {
                InOrderTraversalRecursive(node.Left, result);
                result.Add(node.Data);
                InOrderTraversalRecursive(node.Right, result);
            }
        }
    }
}