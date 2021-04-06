using System;

namespace SplayTree
{
    public class SplayTreeNode<TKey, TData> where TKey : IComparable, IComparable<TKey>
    {
        public TKey Key { get; set; }

        public TData Data { get; set; }

        public SplayTreeNode<TKey, TData> Left { get; set; }

        public SplayTreeNode<TKey, TData> Right { get; set; }

        public SplayTreeNode<TKey, TData> Next { get; set; }

        public SplayTreeNode(TKey key, TData data)
        {
            this.Key = key;
            this.Data = data;
        }

        public SplayTreeNode()
        {

        }

        public override string ToString()
        {
            return $"{{{this.Key},{this.Data}}}";
        }
    }
}