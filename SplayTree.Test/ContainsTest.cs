using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class ContainsTest
    {
        [TestMethod]
        public void WhetherTest()
        {
            var tree = new SplayTree<int, string>();
            Assert.IsFalse(tree.Contains(1));
            Assert.IsFalse(tree.Contains(2));
            Assert.IsFalse(tree.Contains(3));
            tree.Insert(3);
            tree.Insert(1);
            tree.Insert(2);
            Assert.IsTrue(tree.Contains(1));
            Assert.IsTrue(tree.Contains(2));
            Assert.IsTrue(tree.Contains(3));
        }

        [TestMethod]
        public void NoChildrenTest()
        {
            var tree = new SplayTree<int, string>();
            tree.Insert(2);
            Assert.IsFalse(tree.Contains(1));
            Assert.IsFalse(tree.Contains(3));
        }
    }
}
