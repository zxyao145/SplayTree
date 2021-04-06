using System;
using System.Collections.Generic;
using System.Text;

namespace SplayTree
{
    static class Extensions
    {
        public static int CompareTo<TKey>(this TKey left, TKey right, bool reverse = false) where TKey : IComparable, IComparable<TKey>
        {
            return reverse ? right.CompareTo(left): left.CompareTo(right);
        }
    }
}
