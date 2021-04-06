using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class KeysValusTest
    {
        [TestMethod]
        public void SortedKeysTest1()
        {
            var t = new SplayTree<int, int>();
            t.Insert(5);
            t.Insert(-10);
            t.Insert(0);
            t.Insert(33);
            t.Insert(2);
            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>() {-10, 0, 2, 5, 33});
        }

        [TestMethod]
        public void SortedKeysTest2()
        {
            var t = new SplayTree<int, int>(true);
            t.Insert(5);
            t.Insert(-10);
            t.Insert(0);
            t.Insert(33);
            t.Insert(2);

            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>() {33, 5, 2, 0, -10});
        }


        [TestMethod]
        public void SortedValuesTest1()
        {
            var t = new SplayTree<int, char>();
            t.Insert(5, 'D');
            t.Insert(-10, 'A');
            t.Insert(0, 'B');
            t.Insert(33, 'E');
            t.Insert(2, 'C');

            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>()
            {
                -10, 0, 2, 5, 33
            });

            CollectionAssert.AreEqual(t.Values.ToList(), new List<char>()
            {
                'A', 'B', 'C', 'D', 'E'
            });
        }

        [TestMethod]
        public void SortedValuesTest2()
        {
            var t = new SplayTree<int, char>(true);
            t.Insert(5, 'D');
            t.Insert(-10, 'A');
            t.Insert(0, 'B');
            t.Insert(33, 'E');
            t.Insert(2, 'C');

            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>()
            {
                33, 5, 2, 0, -10
            });
            CollectionAssert.AreEqual(t.Values.ToList(), new List<char>()
            {
                'E', 'D', 'C', 'B', 'A'
            });
        }


 [TestMethod]
        public void BulkInsertTest1()
        {
            var t = new SplayTree<int, char>();
            t.Load(new List<int>()
                {
                    5, -10, 0, 33, 2
                },
                new List<char>()
                {
                    'D', 'A', 'B', 'E', 'C'
                }, true);


            CollectionAssert.AreEqual(t.Keys.ToList(), new List<int>()
            {
                -10, 0, 2, 5, 33
            });
            CollectionAssert.AreEqual(t.Values.ToList(), new List<char>()
            {
                'A', 'B', 'C', 'D', 'E'
            });
        }

        [TestMethod]
        public void BulkInsertTest2()
        {
            var t = new SplayTree<int, char>();
            var keys = Enumerable.Range(0, 10000).ToList();
            t.Load(keys);
            CollectionAssert.AreEqual(t.Keys.Take(20).ToList(), keys.Take(20).ToList());
        }

    }
}
