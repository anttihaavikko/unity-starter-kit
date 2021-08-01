using UnityEngine;

namespace AnttiStarterKit.Extensions
{
    public static class VectorExtension
    {
        public static Vector3 RandomOffset(this Vector3 v, float maxLength)
        {
            v += new Vector3(Random.Range(-maxLength, maxLength), Random.Range(-maxLength, maxLength), 0);
            return v;
        }
    }
}