using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class EmptyTest
    {
        [TestMethod]
        public void WhetheIsEmpty()
        {
            var tree = new SplayTree<int,int>();
            Assert.IsTrue(tree.IsEmpty);

            tree.Insert(1);
            Assert.IsFalse(tree.IsEmpty);

            tree.Remove(1);
            Assert.IsTrue(tree.IsEmpty);
        }
    }
}
