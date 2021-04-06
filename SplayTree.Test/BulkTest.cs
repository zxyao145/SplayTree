using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class BulkTest
    {
        [TestMethod]
        public void InsertTest()
        {
            var tree = new SplayTree<int, int>();
            var keys = new List<int>
            {
                1, 2, 3, 4
            };
            var values = new List<int>
            {
                4, 3, 2, 1
            };

            tree.Load(keys, values);

            CollectionAssert.AreEqual(tree.Keys.ToList(), keys);
            CollectionAssert.AreEqual(tree.Values.ToList(), values);
        }

        [TestMethod]
        public void InsertWithoutValuesTest()
        {
            var tree = new SplayTree<int, string>();
            var keys = new List<int>
            {
                1, 2, 3, 4
            };

            tree.Load(keys);

            CollectionAssert.AreEqual(tree.Keys.ToList(), keys);
            CollectionAssert.AreEqual(tree.Values.ToList(), keys.Select(e => default(string)).ToList());
        }

        [TestMethod]
        public void InsertWithContentsTest()
        {
            var tree = new SplayTree<int, string>();
            var keys1 = new List<int>
            {
                22, 56, 0, -10, 12
            };

            tree.Load(keys1, true);
            var keys2 = new List<int>
            {
                100,500, -400, 20, 10
            };
            tree.Load(keys2, true);

            var needKeys = new List<int>(keys1);
            needKeys.AddRange(keys2);
            needKeys.Sort();
            CollectionAssert.AreEqual(tree.Keys.ToList(), needKeys.ToList());
        }

        [TestMethod]
        public void InsertWithMoreContentsTest()
        {
            var tree = new SplayTree<int, string>();
            var keys1 = new List<int>
            {
                100, 500, -400, 20, 10
            };

            tree.Load(keys1, true);
            var keys2 = new List<int>
            {
                22
            };
            tree.Load(keys2, true);

            var needKeys = new List<int>(keys1);
            needKeys.AddRange(keys2);
            needKeys.Sort();
            CollectionAssert.AreEqual(tree.Keys.ToList(), needKeys.ToList());
        }

        [TestMethod]
        public void InsertWithLessContentsTest()
        {
            var tree = new SplayTree<int, string>();
            var keys1 = new List<int>
            {
                22
            };

            tree.Load(keys1, true);
            var keys2 = new List<int>
            {
                100, 500, -400, 20, 10
            };
            tree.Load(keys2, true);

            var needKeys = new List<int>(keys1);
            needKeys.AddRange(keys2);
            needKeys.Sort();
            CollectionAssert.AreEqual(tree.Keys.ToList(), needKeys.ToList());
        }

        [TestMethod]
        public void InsertWithContentsInterleaveTest()
        {
            var tree = new SplayTree<int, string>();
            var keys1 = Enumerable.Range(0, 10).Select(e => e * 10).ToList();

            tree.Load(keys1, true);
            var keys2 = Enumerable.Range(0, 10).Select(e => e * 10 + 5).ToList();
            tree.Load(keys2, true);

            var needKeys = Enumerable.Range(0, 20).Select(e => e * 5).ToList();

            CollectionAssert.AreEqual(tree.Keys.ToList(), needKeys.ToList());
        }
    }
}
