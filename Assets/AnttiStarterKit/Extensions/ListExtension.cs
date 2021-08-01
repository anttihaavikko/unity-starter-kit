using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnttiStarterKit.Extensions
{
    public static class ListExtension
    {
        public static T Random<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}