using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class MinMaxTest
    {
        [TestMethod]
        public void MaxKeyTest()
        {
            var tree = new SplayTree<int,int>();
            tree.Insert(3);
            tree.Insert(5);
            tree.Insert(1);
            tree.Insert(4);
            tree.Insert(2);
            Assert.AreEqual(tree.MaxKey, 5);
        }

        [TestMethod]
        public void EmptyMaxKeyTest()
        {
            var tree = new SplayTree<string, int>();
            Assert.AreEqual(tree.MaxKey, default);
        }

        [TestMethod]
        public void MinKeyTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(3);
            tree.Insert(5);
            tree.Insert(1);
            tree.Insert(4);
            tree.Insert(2);
            Assert.AreEqual(tree.MinKey, 1);
        }

        [TestMethod]
        public void EmptyMinKeyTest()
        {
            var tree = new SplayTree<string, int>();
            Assert.AreEqual(tree.MinKey, default);
        }

        [TestMethod]
        public void MaxNodeTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(3);
            tree.Insert(5,10);
            tree.Insert(1);
            tree.Insert(4);
            tree.Insert(2);
            var node = tree.MaxNode();
            Assert.AreEqual(node.Key, 5);
            Assert.AreEqual(node.Data, 10);
        }

        [TestMethod]
        public void EmptyMaxNodeTest()
        {
            var tree = new SplayTree<int, int>();
            var node = tree.MaxNode();
            Assert.IsNull(node);
        }

        [TestMethod]
        public void MinNodeTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(3);
            tree.Insert(5);
            tree.Insert(1,20);
            tree.Insert(4);
            tree.Insert(2);
            var node = tree.MinNode();
            Assert.AreEqual(node.Key, 1);
            Assert.AreEqual(node.Data, 20);
        }

        [TestMethod]
        public void EmptyMinNodeTest()
        {
            var tree = new SplayTree<int, int>();
            var node = tree.MinNode();
            Assert.IsNull(node);
        }

        [TestMethod]
        public void RemoveMinNodeTest()
        {
            var tree = new SplayTree<int, int>();
            tree.Insert(3);
            tree.Insert(5);
            tree.Insert(1);
            tree.Insert(4);
            tree.Insert(2);
            var node = tree.Pop();
            Assert.AreEqual(node.Key, 1);
            Assert.AreEqual(tree.Size, 4);
        }

    }
}
