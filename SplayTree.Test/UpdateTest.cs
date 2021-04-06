using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class UpdateTest
    {
        static SplayTree<TKey, int>  CreateTree<TKey>(List<TKey> values)
        where TKey: IComparable, IComparable<TKey>
        {
            var t = new SplayTree<TKey, int>();
            values.ForEach((v) => t.Insert(v));
            return t;
        }
        static List<int> ToArray(SplayTreeNode<int,int> tree, List<int> arr = null)
        {
            arr ??= new List<int>();

            if (tree != null)
            {
                ToArray(tree.Left, arr);

                arr.Add(tree.Key);
                ToArray(tree.Right, arr);
            }

            return arr;
        }

        [TestMethod]
        public void SplitTest()
        {
            (SplayTreeNode<int, int> left, SplayTreeNode<int, int> right) split;

            SplayTree<int, int> t;
            t = CreateTree(new List<int>(){ 1, 2, 3 });
            split = t.Split(0);
            Assert.IsNull(split.left, null);
            CollectionAssert.AreEqual(ToArray(split.right), new List<int>(){ 1, 2, 3 });

            t = CreateTree(new List<int>() { 1, 2, 3 });
            split = t.Split(2, SplitPosition.Left);
            CollectionAssert.AreEqual(ToArray(split.left), new List<int>() { 1,2 });
            CollectionAssert.AreEqual(ToArray(split.right), new List<int>() { 3 });

            t = CreateTree(new List<int>() { 1, 2, 3 });
            split = t.Split(2, SplitPosition.Right);
            CollectionAssert.AreEqual(ToArray(split.left), new List<int>() { 1});
            CollectionAssert.AreEqual(ToArray(split.right), new List<int>() { 2, 3 });


            t = CreateTree(new List<int>() { 1, 2, 3 });
            split = t.Split(2);
            CollectionAssert.AreEqual(ToArray(split.left), new List<int>() { 1 });
            CollectionAssert.AreEqual(ToArray(split.right), new List<int>() {  3 });

            t = CreateTree(new List<int>() { 1, 2, 3 });
            split = t.Split(1);
            Assert.AreEqual(ToArray(split.left).Count, 0);
            CollectionAssert.AreEqual(ToArray(split.right), new List<int>(){ 2, 3 });

            t = CreateTree(new List<int>() { 1, 2, 3 });
            split = t.Split(3);
            CollectionAssert.AreEqual(ToArray(split.left), new List<int>() {1,2 });
            Assert.AreEqual(ToArray(split.right).Count, 0);
        }

        [TestMethod]
        public void MergeTest()
        {
            var t = CreateTree(new List<int>(){ 1, 2, 3, 4, 5 });
            t.Update(3, 6);
            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>()
            {
                1, 2, 4, 5, 6
            });


            t.Update(2, 0);
            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>()
            {
                0, 1, 4, 5, 6
            });

            t.Update(0, 7);
            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>()
            {
                1, 4, 5, 6, 7
            });


            t.Update(7, -3);
            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>()
            {
                -3, 1, 4, 5, 6
            });
        }

    }
}
