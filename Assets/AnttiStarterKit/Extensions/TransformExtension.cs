using UnityEngine;

namespace AnttiStarterKit.Extensions
{
    public static class TransformExtension
    {
        public static void Mirror(this Transform t, bool mirror)
        {
            var scale = t.localScale;
            t.localScale = new Vector3((mirror ? -1f : 1f) * scale.x, scale.y, scale.z);
        }
    }
}