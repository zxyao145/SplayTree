using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class DuplicateTest
    {
        [TestMethod]
        public void AllowInsertDuplicateKeyTest()
        {
            var tree = new SplayTree<int, string>();
            var values = new List<int>()
            {
                2, 12, 1, -6, 1
            };
            foreach (var value in values)
            {
                tree.Insert(value);
            }

            CollectionAssert.AreEqual(tree.Keys.ToList(), new List<int>()
            {
                -6, 1, 1, 2, 12
            });
            Assert.AreEqual(tree.Size, 5);
        }

        [TestMethod]
        public void MultipleDuplicateKeyTest()
        {
            var tree = new SplayTree<int, string>();
            var values = new List<int>()
            {
                2, 12, 1, 1, -6, 2, 1, 1, 13
            };
            foreach (var value in values)
            {
                tree.Insert(value);
            }

            CollectionAssert.AreEqual(tree.Keys.ToList(), new List<int>()
            {
                -6, 1, 1, 1, 1, 2, 2, 12, 13
            });
            Assert.AreEqual(tree.Size, 9);
        }

        [TestMethod]
        public void RemoveDuplicateKeyTest()
        {
            var tree = new SplayTree<int, string>();
            var values = new List<int>()
            {
                2, 12, 1, 1, -6, 1, 1
            };
            foreach (var value in values)
            {
                tree.Insert(value);
            }

            var size = tree.Size;
            for (var i = 0; i < 4; i++)
            {
                tree.Remove(1);

                if (i < 3) Assert.IsTrue(tree.Contains(1));
                Assert.AreEqual(tree.Size, --size);
            }

            Assert.IsFalse(tree.Contains(1));
        }

        [TestMethod]
        public void RemoveMultipleDuplicateKeyTest()
        {
            var tree = new SplayTree<int, string>();
            var values = new List<int>()
            {
                2, 12, 1, 1, -6, 1, 1, 2, 0, 2
            };
            foreach (var value in values)
            {
                tree.Insert(value);
            }

            var size = tree.Size;
            while (!tree.IsEmpty)
            {
                tree.Pop();
                Assert.AreEqual(tree.Size, --size);
            }
        }


        [TestMethod]
        public void DisAllowAddDuplicateKeyTest()
        {
            var tree = new SplayTree<int, string>();
            var values = new List<int>()
            {
                2, 12, 1, -6, 1
            };
            foreach (var value in values)
            {
                tree.Add(value);
            }

            CollectionAssert.AreEqual(tree.Keys.ToList(), new List<int>()
            {
                -6, 1, 2, 12
            });

            Assert.AreEqual(tree.Size, 4);
        }

        [TestMethod]
        public void ShouldAddOnlyTest()
        {
            var tree = new SplayTree<int, string>();
            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);

            var s = tree.Size;
            tree.Add(1);
            Assert.AreEqual(tree.Size, s);
        }
    }
}
