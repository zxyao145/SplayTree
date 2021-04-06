using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class FindTest
    {
        [TestMethod]
        public void FindWithSplayingTest()
        {
            var tree = new SplayTree<int, int>();
            Assert.AreEqual(tree.Find(1), null);
            Assert.AreEqual(tree.Find(2), null);
            Assert.AreEqual(tree.Find(3), null);
            tree.Insert(1, 4);
            tree.Insert(2, 5);
            tree.Insert(3, 6);

            var root = tree.Root;
            Assert.AreEqual(tree.Find(1).Data, 4);
            root.Should().NotBeEquivalentTo(tree.Root);
            root = tree.Root;

            Assert.AreEqual(tree.Find(2).Data, 5);
            Assert.AreNotEqual(root, tree.Root);
            root = tree.Root;

            Assert.AreEqual(tree.Find(3).Data, 6);
            root.Should().NotBeEquivalentTo(tree.Root);
            root = tree.Root;

            Assert.IsNull(tree.Find(8));
            root.Should().BeEquivalentTo(tree.Root);
        }


        [TestMethod]
        public void FindWithoutSplayingTest()
        {
            var tree = new SplayTree<int, int>();
            Assert.AreEqual(tree.FindStatic(1), null);
            Assert.AreEqual(tree.FindStatic(2), null);
            Assert.AreEqual(tree.FindStatic(3), null);
            tree.Insert(-2, 8);
            tree.Insert(1, 4);
            tree.Insert(2, 5);
            tree.Insert(3, 6);

            tree.Find(2);
            var root = tree.Root;
            Assert.AreEqual(tree.FindStatic(1).Data, 4);
            root.Should().BeEquivalentTo(tree.Root);
            Assert.AreEqual(tree.FindStatic(2).Data, 5);
            root.Should().BeEquivalentTo(tree.Root);
            Assert.AreEqual(tree.FindStatic(3).Data, 6);
            root.Should().BeEquivalentTo(tree.Root);
            Assert.AreEqual(tree.FindStatic(-2).Data, 8);

            tree.Find(2).Should().BeEquivalentTo(tree.Root);
        }
    }
}
