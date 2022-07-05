using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnttiStarterKit.Extensions
{
    public static class ListExtension
    {
        public static T Random<T>(this IList<T> list)
        {
            return list.Any() ? list[UnityEngine.Random.Range(0, list.Count)] : default;
        }
        
        public static T RandomWeighted<T>(this IList<T> list) where T : IWeighted
        {
            return list.Any() ? list.OrderByDescending(c => c.Weight()).ThenBy(_ => UnityEngine.Random.value).First() : default;
        }
    }
}