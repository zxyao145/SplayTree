using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class TraversalTest
    {
        [TestMethod]
        public void ForeachTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(3);
            tree.Insert(1);
            tree.Insert(0);
            tree.Insert(2);

            var i = 0;
            foreach (var splayTreeNode in tree)
            {
                Assert.AreEqual(splayTreeNode.Key, i++);
            }
        }


        [TestMethod]
        public void PredecessorTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);


            for (var i = 1; i < 10; i++)
            {
                Assert.AreEqual(tree.Prev(tree.Find(i)).Key,
                    tree.Find(i - 1).Key);
            }
        }

        [TestMethod]
        public void SuccessorTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);


            for (var i = 0; i < 9; i++)
            {
                Assert.AreEqual(tree.Next(tree.Find(i)),
                    tree.Find(i + 1));
            }
        }

        [TestMethod]
        public void MinNodePredecessorTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);

            var min = tree.MinNode();
            Assert.IsNull(tree.Prev(min));
            tree.Remove(min.Key);
            min = tree.MinNode();
            Assert.IsNull(tree.Prev(min));
        }

        [TestMethod]
        public void MaxNodeSuccessorTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);

            var max = tree.MaxNode();
            Assert.IsNull(tree.Next(max));
            tree.Remove(max.Key);
            max = tree.MaxNode();
            Assert.IsNull(tree.Next(max));
        }

        [TestMethod]
        public void ReachEndTest()
        {
            var tree = new SplayTree<int, int>();
            var keys = new List<int>()
            {
                49153, 49154, 49156, 49157, 49158, 49159, 49160, 49161,
                49163, 49165, 49191, 49199, 49201, 49202, 49203, 49204,
                49206, 49207, 49208, 49209, 49210, 49212
            };
            keys.ForEach(k => { tree.Insert(k); });

            var min = tree.MinNode();

            keys.ForEach((key) =>
            {
                Assert.AreEqual(min.Key, key);
                min = tree.Next(min);
            });

            Assert.IsNull(min);
        }

        [TestMethod]
        public void BidirectionalStepping()
        {
            var tree = new SplayTree<int, int>();
            var keys = new List<int>()
            {
                49153, 49154, 49156, 49157, 49158, 49159, 49160, 49161,
                49163, 49165, 49191, 49199, 49201, 49202, 49203, 49204,
                49206, 49207, 49208, 49209, 49210, 49212
            };

            tree.Load(keys);

            var min = tree.MinNode();

            var i = 0;
            keys.ForEach((key) =>
            {
                Assert.AreEqual(min.Key, key);
                if (i != 0)
                {
                    Assert.AreEqual(tree.Next(tree.Prev(min)).Key, key);
                }

                min = tree.Next(min);
                i += 1;
            });

            Assert.IsNull(min);
        }

        [TestMethod]
        public void TwoNodesTreeTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(5);
            tree.Insert(10);

            var min = tree.MinNode();
            Assert.AreEqual(min.Key, 5);
            Assert.IsNull(tree.Prev(min));
            Assert.AreEqual(tree.Next(min).Key, 10);

            var max = tree.MaxNode();
            Assert.AreEqual(max.Key, 10);
            Assert.IsNull(tree.Next(max));
            Assert.AreEqual(tree.Prev(max).Key, 5);
        }

        [TestMethod]
        public void GetNodesByIndexTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++) tree.Insert(i);


            for (var i = 0; i < 10; i++) Assert.AreEqual(tree.At(i).Key, i);

            Assert.IsNull(tree.At(10));
            Assert.IsNull(tree.At(-1));
        }


        [TestMethod]
        public void RangeWalkingTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);

            var arr = new List<int>();

            tree.Range(3, 8, (node) =>
            {
                arr.Add(node.Key);
            });

            CollectionAssert.AreEqual(arr, new List<int>()
            {
                3, 4, 5, 6, 7, 8
            });
        }


        [TestMethod]
        public void RangeWalkingNotExistLowKeyTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);

            var arr = new List<int>();

            tree.Range(-3, 5, (node) =>
            {
                arr.Add(node.Key);
            });

            CollectionAssert.AreEqual(arr, new List<int>()
            {
                0,1,2,3,4,5
            });
        }

        [TestMethod]
        public void RangeWalkingNotExistHighKeyTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);

            var arr = new List<int>();

            tree.Range(3, 15, (node) =>
            {
                arr.Add(node.Key);
            });

            CollectionAssert.AreEqual(arr, new List<int>()
            {
                3,4,5,6,7,8,9
            });
        }

        [TestMethod]
        public void RangeWalkingOutIndexRangeTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);

            var arr = new List<int>();

            tree.Range(10, 20, (node) =>
            {
                arr.Add(node.Key);
            });
            Assert.AreEqual(arr.Count, 0);

            tree.Range(-10, 20, (node) =>
            {
                arr.Add(node.Key);
            });
            CollectionAssert.AreEqual(arr, tree.Keys.ToList());
        }

        [TestMethod]
        public void RangeWalkingInterruptionTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 10; i++)
                tree.Insert(i);

            var arr = new List<int>();

            tree.Range(2, 8, (node) =>
            {
                arr.Add(node.Key);
                if (node.Key == 5) 
                    return true;
                return false;
            });

            Assert.AreEqual(arr.Count, 4);

            CollectionAssert.AreEqual(arr, new List<int>()
            {
                2,3,4,5
            });
        }
    }
}