using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class RemoveTest
    {
        [TestMethod]
        public void EmptyTreeTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Remove(1);
            Assert.AreEqual(tree.Size,0);
        }

        [TestMethod]
        public void SingleTreeTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(1);
            tree.Remove(1);
            Assert.IsTrue(tree.IsEmpty);
        }

        [TestMethod]
        public void NotExistKeyRemoveTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(1);
            tree.Remove(2);
            Assert.AreEqual(tree.Size, 1);
        }

        [TestMethod]
        public void RightChildRemoveTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(1);
            tree.Insert(2);

            tree.Remove(1);
            Assert.AreEqual(tree.Root.Key, 2);
        }


        [TestMethod]
        public void LeftChildRemoveTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(2);
            tree.Insert(1);

            tree.Remove(2);
            Assert.AreEqual(tree.Root.Key, 1);
        }


        [TestMethod]
        public void NotBreakTest()
        {
            var tree = new SplayTree<int, int>();
            var n1 = tree.Insert(1);
            var n2 = tree.Insert(2);
            var n3 = tree.Insert(3);

            tree.Remove(2);
            Assert.AreEqual(n2.Key, 2);
            Assert.AreEqual(n3.Key, 3);
        }

        [TestMethod]
        public void PopTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(2);
            tree.Insert(1);
            tree.Remove(2);

            var removed = tree.Pop();

            Assert.AreEqual(removed.Key, 1);
           Assert.IsNull(tree.Pop());
        }


        [TestMethod]
        public void ClearTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(2);
            tree.Insert(1);
            tree.Remove(2);

            tree.Clear();
            Assert.IsNull(tree.Root);

            Assert.AreEqual(tree.Size, 0);
        }
    }
}
