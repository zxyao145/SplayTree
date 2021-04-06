using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class PrintTest
    {
        [TestMethod]
        public void Test1()
        {
            var tree = new SplayTree<int,int>();
            for (var i = 0; i < 3; i++) tree.Insert(i);

            tree.Find(2);
            //└── 2
            //   ├── 1
            //   │   ├── 0
            var str = "└── 2\n    ├── 1\n    │   ├── 0\n";
            Assert.AreEqual(tree.ToString(), str);
        }

        [TestMethod]
        public void Test2()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 3; i++) tree.Insert(i);

            tree.Find(1);

            //└── 1
            //   ├── 0
            //   │   ├── 2
            var str = "└── 1\n    ├── 0\n    └── 2\n";
            Assert.AreEqual(tree.ToString(), str);
        }


        [TestMethod]
        public void CustomPrinterTest()
        {
            var tree = new SplayTree<int, int>();
            for (var i = 0; i < 3; i++) tree.Insert(i, i+1);

            //└── 1:2
            //   ├── 0:1
            var str = "└── 1:2\n    ├── 0:1\n";
            var a = tree.ToString((node) => $"{node.Key}:{node.Data}", tree.FindStatic(1));
            Assert.AreEqual(a, str);
        }
    }
}
