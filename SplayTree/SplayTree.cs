using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SplayTree
{

    public enum SplitPosition
    {
        None = 0,
        Left = 1,
        Right = 2
    }

    /// <summary>
    /// https://github.com/w8r/splay-tree
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class SplayTree<TKey, TData> : IEnumerable<SplayTreeNode<TKey, TData>>
        where TKey : IComparable, IComparable<TKey>
    {
        public int Size { get; private set; }

        private SplayTreeNode<TKey, TData> _root = null;
        public SplayTreeNode<TKey, TData> Root => _root;

        private bool _reverseKeyOrder;

        public SplayTree(bool reverseKeyOrder = false)
        {
            _reverseKeyOrder = reverseKeyOrder;
        }


        /// <summary>
        /// Inserts a key, allows duplicates
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public SplayTreeNode<TKey, TData> Insert(TKey key, TData data)
        {
            this.Size++;
            return this._root = Insert(key, data, this._root, _reverseKeyOrder);
        }

        /// <summary>
        /// Inserts a key, with default(TData) allows duplicates
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SplayTreeNode<TKey, TData> Insert(TKey key)
        {
            return Insert(key, default);
        }

        /// <summary>
        /// Adds a key, if it is not present in the tree
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public SplayTreeNode<TKey, TData> Add(TKey key, TData data)
        {
            var node = new SplayTreeNode<TKey, TData>(key, data);

            if (this._root == null)
            {
                node.Left = node.Right = null;
                this.Size++;
                this._root = node;
            }

            var t = Splay(key, this._root, _reverseKeyOrder);
            var cmp = key.CompareTo(t.Key, _reverseKeyOrder);
            if (cmp == 0)
            {
                this._root = t;
            }
            else
            {
                if (cmp < 0)
                {
                    node.Left = t.Left;
                    node.Right = t;
                    t.Left = null;
                }
                else
                {
                    node.Right = t.Right;
                    node.Left = t;
                    t.Right = null;
                }

                this.Size++;
                this._root = node;
            }

            return this._root;
        }

        /// <summary>
        /// Adds a key with default value, if it is not present in the tree
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SplayTreeNode<TKey, TData> Add(TKey key)
        {
            return Add(key, default);
        }


        public void Remove(TKey key)
        {
            this._root = this.Remove(key, this._root);
        }

        private SplayTreeNode<TKey, TData> Remove(TKey i, SplayTreeNode<TKey, TData> t)
        {
            SplayTreeNode<TKey, TData> x;
            if (t == null) return null;
            t = Splay(i, t, _reverseKeyOrder);
            var cmp = i.CompareTo(t.Key, _reverseKeyOrder);
            if (cmp == 0)
            {
                /* found it */
                if (t.Left == null)
                {
                    x = t.Right;
                }
                else
                {
                    x = Splay(i, t.Left, _reverseKeyOrder);
                    x.Right = t.Right;
                }

                this.Size--;
                return x;
            }

            return t; /* It wasn't there */
        }

        /// <summary>
        /// Removes and returns the node with smallest key
        /// </summary>
        /// <returns></returns>
        public SplayTreeNode<TKey, TData> Pop()
        {
            var node = this._root;
            if (node == null)
                return default;

            while (node.Left != null) node = node.Left;
            this._root = Splay(node.Key, this._root, _reverseKeyOrder);
            this._root = this.Remove(node.Key, this._root);
            return node;
        }

        /// <summary>
        /// Find without splaying
        /// </summary>
        public SplayTreeNode<TKey, TData> FindStatic(TKey key)
        {
            var current = this._root;

            while (current != null)
            {
                var cmp = key.CompareTo(current.Key, _reverseKeyOrder);
                if (cmp == 0) return current;
                else if (cmp < 0) current = current.Left;
                else current = current.Right;
            }

            return null;
        }

        /// <summary>
        /// Find with splaying
        /// </summary>
        public SplayTreeNode<TKey, TData> Find(TKey key)
        {
            if (this._root != null)
            {
                this._root = Splay(key, this._root, _reverseKeyOrder);
                if (key.CompareTo(this._root.Key, _reverseKeyOrder) != 0) return null;
            }

            return this._root;
        }

        public bool Contains(TKey key)
        {
            return FindStatic(key) != null;
        }

        public bool IsEmpty => this._root == null;

        /// <summary>
        /// Walk key range from `low` to `high`. Stops if `fn` returns a value.
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="func"></param>
        public void Range(TKey low, TKey high, Func<SplayTreeNode<TKey, TData>, bool> func)
        {
            var q = new Stack<SplayTreeNode<TKey, TData>>();
            var node = this._root;

            while (q.Count != 0 || node != null)
            {
                if (node != null)
                {
                    q.Push(node);
                    node = node.Left;
                }
                else
                {
                    node = q.Pop();
                    var cmp = node.Key.CompareTo(high, _reverseKeyOrder);
                    if (cmp > 0)
                    {
                        break;
                    }

                    if (node.Key.CompareTo(low, _reverseKeyOrder) >= 0)
                    {
                        var stop = func(node);
                        if (stop)
                        {
                            break;
                        }
                    }

                    node = node.Right;
                }
            }
        }

        /// <summary>
        /// Walk key range from `low` to `high`. Stops if `fn` returns a value.
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="action"></param>
        public void Range(TKey low, TKey high, Action<SplayTreeNode<TKey, TData>> action)
        {
            var q = new Stack<SplayTreeNode<TKey, TData>>();
            var node = this._root;
            var cmp = 0;

            while (q.Count != 0 || node != null)
            {
                if (node != null)
                {
                    q.Push(node);
                    node = node.Left;
                }
                else
                {
                    node = q.Pop();
                    cmp = node.Key.CompareTo(high, _reverseKeyOrder);
                    if (cmp > 0)
                    {
                        break;
                    }
                    else if (node.Key.CompareTo(low, _reverseKeyOrder) >= 0)
                    {
                        action(node);
                    }

                    node = node.Right;
                }
            }
        }


        public IEnumerable<TKey> Keys => this.Select(e => e.Key);

        public IEnumerable<TData> Values => this.Select(e => e.Data);


        public IEnumerator<SplayTreeNode<TKey, TData>> GetEnumerator()
        {
            var current = this._root;
            var splayTreeNodes = new Stack<SplayTreeNode<TKey, TData>>();
            var done = false;

            while (!done)
            {
                if (current != null)
                {
                    splayTreeNodes.Push(current);
                    current = current.Left;
                }
                else
                {
                    if (splayTreeNodes.Count != 0)
                    {
                        current = splayTreeNodes.Pop();
                        yield return current;
                        current = current.Right;
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TKey MinKey
        {
            get
            {
                var node = MinNode();
                if (node == null)
                {
                    return default;
                }

                return node.Key;
            }
        }

        public TKey MaxKey 
        {
            get
            {
                var node = MaxNode();
                if (node == null)
                {
                    return default;
                }

                return node.Key;
            }
        }


        public SplayTreeNode<TKey, TData> MinNode(SplayTreeNode<TKey, TData> node = null)
        {
            node ??= this._root;
            if (node != null)
            {
                while (node.Left != null)
                {
                    node = node.Left;
                }
            }

            return node;
        }

        public SplayTreeNode<TKey, TData> MaxNode(SplayTreeNode<TKey, TData> node = null)
        {
            node ??= this._root;
            if (node != null)
            {
                while (node.Right != null)
                {
                    node = node.Right;
                }
            }

            return node;
        }

        public SplayTreeNode<TKey, TData> At(int index)
        {
            var current = this._root;
            var done = false;
            var i = 0;
            var splayTreeNodes = new Stack<SplayTreeNode<TKey, TData>>();

            while (!done)
            {
                if (current != null)
                {
                    splayTreeNodes.Push(current);
                    current = current.Left;
                }
                else
                {
                    if (splayTreeNodes.Count > 0)
                    {
                        current = splayTreeNodes.Pop();
                        if (i == index) return current;
                        i++;
                        current = current.Right;
                    }
                    else done = true;
                }
            }

            return null;
        }

        public SplayTreeNode<TKey, TData> Next(SplayTreeNode<TKey, TData> node)
        {
            var root = this._root;
            SplayTreeNode<TKey, TData> successor = null;

            if (node.Right != null)
            {
                successor = node.Right;
                while (successor.Left != null) successor = successor.Left;
                return successor;
            }

            while (root != null)
            {
                var cmp = node.Key.CompareTo(root.Key, _reverseKeyOrder);
                if (cmp == 0) break;
                else if (cmp < 0)
                {
                    successor = root;
                    root = root.Left;
                }
                else root = root.Right;
            }

            return successor;
        }

        public SplayTreeNode<TKey, TData> Prev(SplayTreeNode<TKey, TData> node)
        {
            var root = this._root;
            SplayTreeNode<TKey, TData> predecessor = null;

            if (node.Left != null)
            {
                predecessor = node.Left;
                while (predecessor.Right != null) predecessor = predecessor.Right;
                return predecessor;
            }

            while (root != null)
            {
                var cmp = node.Key.CompareTo(root.Key, _reverseKeyOrder);
                if (cmp == 0) break;
                if (cmp < 0)
                {
                    root = root.Left;
                }
                else
                {
                    predecessor = root;
                    root = root.Right;
                }
            }

            return predecessor;
        }

        public void Clear()
        {
            this._root = null;
            this.Size = 0;
        }


        public SplayTree<TKey, TData> Load(List<TKey> keys, List<TData> values, bool presort = false)
        {
            var size = keys.Count;

            // sort if needed
            if (presort) Sort(keys, values, 0, size - 1, _reverseKeyOrder);

            if (this._root == null)
            {
                // empty tree
                this._root = LoadRecursive(keys, values, 0, size);
                this.Size = size;
            }
            else
            {
                // that re-builds the whole tree from two in-order traversals
                var mergedList = MergeLists(ToList(this._root), CreateList(keys, values), _reverseKeyOrder);
                size = this.Size + size;
                this._root = SortedListToBST(new TreeNodeList<TKey, TData>(mergedList), 0, size);
            }

            return this;
        }

        public SplayTree<TKey, TData> Load(List<TKey> keys, bool presort = false)
        {
            var values = keys.Select(e => default(TData)).ToList();
            return Load(keys,values, presort);
        }

        public void Update(TKey key, TKey newKey, TData newData = default)
        {
            var (left, right) = Split(key, this._root, _reverseKeyOrder);
            if (key.CompareTo(newKey, _reverseKeyOrder) < 0)
            {
                right = Insert(newKey, newData, right, _reverseKeyOrder);
            }
            else
            {
                left = Insert(newKey, newData, left, _reverseKeyOrder);
            }

            this._root = Merge(left, right, _reverseKeyOrder);
        }


        public (SplayTreeNode<TKey, TData> Left, SplayTreeNode<TKey, TData> Right) Split(TKey key, SplitPosition splitPosition = SplitPosition.None)
        {
            return Split(key, this._root, _reverseKeyOrder, splitPosition);
        }


        public override string ToString()
        {
            return ToString((node) => node.Key?.ToString() ?? "", node: this.Root);
        }

        public string ToString(Func<SplayTreeNode<TKey, TData>, string> printNode, SplayTreeNode<TKey, TData> node = null)
        {
            node ??= this.Root;
            var output = new List<string>();
            PrintRow(node, "", true, output, printNode);
            return string.Join("", output);
        }

        private static void PrintRow(
            SplayTreeNode<TKey, TData> root, string prefix,
            bool isTail, List<string> output, Func<SplayTreeNode<TKey, TData>,string> printNode)
        {
            if (root != null)
            {
                var str =
                    $"{prefix}{(isTail ? "└── " : "├── ")}{(printNode(root))}\n";

                output.Add(str);
                var indent = prefix + (isTail ? "    " : "│   ");
                if (root.Left != null)
                {
                    PrintRow(root.Left, indent, false, output, printNode);
                }
                if (root.Right != null)
                {
                    PrintRow(root.Right, indent, true, output, printNode);
                }
            }
        }

        #region static

        private static void Sort(List<TKey> keys, List<TData> values, int left, int right, bool reverseKeyOrder)
        {
            if (left >= right) return;

            var pivot = keys[(left + right) >> 1];
            var i = left - 1;
            var j = right + 1;

            while (true)
            {
                do i++;
                while (keys[i].CompareTo(pivot, reverseKeyOrder) < 0);
                do j--;
                while (keys[j].CompareTo(pivot, reverseKeyOrder) > 0);
                if (i >= j) break;

                var tmp = keys[i];
                keys[i] = keys[j];
                keys[j] = tmp;

                var tmpV = values[i];
                values[i] = values[j];
                values[j] = tmpV;
            }

            Sort(keys, values, left, j, reverseKeyOrder);
            Sort(keys, values, j + 1, right, reverseKeyOrder);
        }

        private static SplayTreeNode<TKey, TData> SortedListToBST(TreeNodeList<TKey, TData> list, int start, int end)
        {
            var size = end - start;
            if (size > 0)
            {
                var middle = start + (int) Math.Floor(size / 2.0);
                var left = SortedListToBST(list, start, middle);

                var root = list.Head;
                root.Left = left;

                list.Head = list.Head.Next;

                root.Right = SortedListToBST(list, middle + 1, end);
                return root;
            }

            return null;
        }

        private static SplayTreeNode<TKey, TData> CreateList(List<TKey> keys, List<TData> values)
        {
            var head = new SplayTreeNode<TKey, TData>();
            var p = head;
            for (var i = 0; i < keys.Count; i++)
            {
                p = p.Next = new SplayTreeNode<TKey, TData>(keys[i], values[i]);
            }

            p.Next = null;
            return head.Next;
        }

        private static SplayTreeNode<TKey, TData> ToList(SplayTreeNode<TKey, TData> root)
        {
            var current = root;
            var Q = new Stack<SplayTreeNode<TKey, TData>>();
            var done = false;

            var head = new SplayTreeNode<TKey, TData>();
            var p = head;

            while (!done)
            {
                if (current != null)
                {
                    Q.Push(current);
                    current = current.Left;
                }
                else
                {
                    if (Q.Count > 0)
                    {
                        current = p = p.Next = Q.Pop();
                        current = current.Right;
                    }
                    else done = true;
                }
            }

            p.Next = null; // that'll work even if the tree was empty
            return head.Next;
        }

        private static SplayTreeNode<TKey, TData> LoadRecursive(List<TKey> keys, List<TData> values, int start, in int end)
        {
            var size = end - start;
            if (size > 0)
            {
                var middle = start + (int) Math.Floor(size / 2.0);
                var key = keys[middle];
                var data = values[middle];
                var node = new SplayTreeNode<TKey, TData>(key, data);
                node.Left = LoadRecursive(keys, values, start, middle);
                node.Right = LoadRecursive(keys, values, middle + 1, end);
                return node;
            }

            return null;
        }

        private static SplayTreeNode<TKey, TData> MergeLists(SplayTreeNode<TKey, TData> l1,
            SplayTreeNode<TKey, TData> l2, bool reverseKeyOrder)
        {
            var head = new SplayTreeNode<TKey, TData>(); // dummy
            var p = head;

            var p1 = l1;
            var p2 = l2;

            while (p1 != null && p2 != null)
            {
                if (
                    p1.Key.CompareTo(p2.Key, reverseKeyOrder) < 0)
                {
                    p.Next = p1;
                    p1 = p1.Next;
                }
                else
                {
                    p.Next = p2;
                    p2 = p2.Next;
                }

                p = p.Next;
            }

            if (p1 != null)
            {
                p.Next = p1;
            }
            else if (p2 != null)
            {
                p.Next = p2;
            }

            return head.Next;
        }


        /// <summary>
        ///  Simple top down splay, not requiring i to be in the tree t.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        static SplayTreeNode<TKey, TData> Splay(TKey key, SplayTreeNode<TKey, TData> root, bool reverseKeyOrder)
        {
            var n = new SplayTreeNode<TKey, TData>();
            var l = n;
            var r = n;

            while (true)
            {
                var cmp = key.CompareTo(root.Key, reverseKeyOrder);

                //if (i < t.key) {
                if (cmp < 0)
                {
                    if (root.Left == null) break;
                    //if (i < t.left.key) {
                    if (key.CompareTo(root.Left.Key, reverseKeyOrder) < 0)
                    {
                        var y = root.Left; /* rotate right */
                        root.Left = y.Right;
                        y.Right = root;
                        root = y;
                        if (root.Left == null) break;
                    }

                    r.Left = root; /* link right */
                    r = root;
                    root = root.Left;
                    //} else if (i > t.key) {
                }
                else if (cmp > 0)
                {
                    if (root.Right == null) break;
                    //if (i > t.right.key) {
                    if (key.CompareTo(root.Right.Key, reverseKeyOrder) > 0)
                    {
                        var y = root.Right; /* rotate left */
                        root.Right = y.Left;
                        y.Left = root;
                        root = y;
                        if (root.Right == null) break;
                    }

                    l.Right = root; /* link left */
                    l = root;
                    root = root.Right;
                }
                else break;
            }

            /* assemble */
            l.Right = root.Left;
            r.Left = root.Right;
            root.Left = n.Right;
            root.Right = n.Left;
            return root;
        }

        static SplayTreeNode<TKey, TData> Insert(TKey key, TData data, SplayTreeNode<TKey, TData> root, bool reverseKeyOrder)
        {
            var node = new SplayTreeNode<TKey, TData>(key, data);
            if (root == null)
            {
                return node;
            }

            root = Splay(key, root, reverseKeyOrder);

            var cmp = key.CompareTo(root.Key, reverseKeyOrder);
            if (cmp < 0)
            {
                node.Left = root.Left;
                node.Right = root;
                root.Left = null;
            }
            else
            {
                node.Right = root.Right;
                node.Left = root;
                root.Right = null;
            }

            return node;
        }


        static (SplayTreeNode<TKey, TData>, SplayTreeNode<TKey, TData>)
            Split(TKey key, SplayTreeNode<TKey, TData> v, bool reverseKeyOrder,
                SplitPosition splitPosition = SplitPosition.None)
        {
            SplayTreeNode<TKey, TData> left = null, right = null;

            if (v != null)
            {
                v = Splay(key, v, reverseKeyOrder);

                var cmp = v.Key.CompareTo(key, reverseKeyOrder);
                if (cmp == 0)
                {
                    switch (splitPosition)
                    {
                        case SplitPosition.Left:
                            left = v;
                            right = v.Right;
                            v.Right = null;
                            break;
                        case SplitPosition.Right:
                            left = v.Left;
                            right = v;
                            v.Left = null;
                            break;
                        default:
                            left = v.Left;
                            right = v.Right;
                            break;
                    }
                }
                else if (cmp < 0)
                {
                    right = v.Right;
                    v.Right = null;
                    left = v;
                }
                else
                {
                    left = v.Left;
                    v.Left = null;
                    right = v;
                }
            }

            return (left, right);
        }

        static SplayTreeNode<TKey, TData> Merge
        (SplayTreeNode<TKey, TData> left,
                SplayTreeNode<TKey, TData> right
                , bool reverseKeyOrder)
        {
            if (right == null) return left;
            if (left == null) return right;

            right = Splay(left.Key, right, reverseKeyOrder);
            right.Left = left;
            return right;
        }

        #endregion
    }


    public class TreeNodeList<TKey, TData> where TKey:IComparable, IComparable<TKey>
    {
        public TreeNodeList(SplayTreeNode<TKey, TData> head)
        {
            Head = head;
        }
        public SplayTreeNode<TKey, TData> Head { get; set; }
    }
}