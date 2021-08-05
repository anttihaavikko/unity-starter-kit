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

        public static Vector3 Where(this Vector3 v, float? x, float? y, float? z)
        {
            return new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
        }
    }
}