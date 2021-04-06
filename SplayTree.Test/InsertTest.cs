using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class InsertTest
    {
        [TestMethod]
        public void SizeTest()
        {
            var tree = new SplayTree<int, string>();
            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(4);
            tree.Insert(5);
            Assert.AreEqual(tree.Size, 5);
        }

        [TestMethod]
        public void ReturnPointerTest()
        {
            var tree = new SplayTree<int, string>();
            var n1 = tree.Insert(1);
            var n2 = tree.Insert(2);
            var n3 = tree.Insert(3);

            Assert.AreEqual(n1.Key, 1);
            Assert.AreEqual(n2.Key, 2);
            Assert.AreEqual(n3.Key, 3);
        }
    }
}
