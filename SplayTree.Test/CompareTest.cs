using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplayTree.Test
{
    [TestClass]
    public class CompareTest
    {
        [TestMethod]
        public void ReverseTest()
        {
            var tree = new SplayTree<int, string>(true);
            tree.Insert(2);
            tree.Insert(1);
            tree.Insert(3);
            Assert.AreEqual(tree.Size, 3);
            Assert.AreEqual(tree.MinKey, 3);
            Assert.AreEqual(tree.MaxKey, 1);

            tree.Remove(3);
            Assert.AreEqual(tree.Size, 2);
            Assert.AreEqual(tree.Root.Key, 2);
            Assert.AreEqual(tree.Root.Left, null);
            Assert.AreEqual(tree.Root.Right.Key, 1);
        }

        public static List<T> Shuffle<T>(List<T> data)
        {
            var currentIndex = data.Count;
            var random = new Random();
            while (0 != currentIndex)
            {
                var randomIndex = (int)Math.Floor(random.NextDouble() * currentIndex);
                currentIndex -= 1;
                var temporaryValue = data[currentIndex];
                data[currentIndex] = data[randomIndex];
                data[randomIndex] = temporaryValue;
            }
            return data;
        }

        [TestMethod]
        public void CustomKeyTest()
        {
            var tree = new SplayTree<CustomKey, string>();
            var objects = Enumerable.Range(0, 10)
                .Select(e => new CustomKey(e))
                .ToList();
            Shuffle(objects);

            foreach (var customKey in objects)
            {
                tree.Insert(customKey);
            }

            objects.Sort();
            CollectionAssert.AreEqual(tree.Keys.Select(e=>e.Value).ToList(),
                objects.Select(e=>e.Value).ToList()
                );
        }

        public class CustomKey:IComparable,IComparable<CustomKey>
        {
            public int Value { get; set; }

            public CustomKey(int val)
            {
                Value = val;
            }

            public int CompareTo(object? obj)
            {
                if (obj is CustomKey customKey)
                {
                    return CompareTo(customKey);
                }
                throw new NotImplementedException();
            }

            public int CompareTo(CustomKey other)
            {
                var diff = this.Value - other.Value;
                return diff > 0 ? -1 : diff == 0 ? 0 : 1;
            }
        }
    }
}
