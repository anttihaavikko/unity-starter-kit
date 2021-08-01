using UnityEngine;

namespace AnttiStarterKit.Extensions
{
    public static class ColorExtension
    {
        public static Color RandomTint(this Color color, float maxLength)
        {
            color += new Color(Random.Range(-maxLength, maxLength), Random.Range(-maxLength, maxLength), Random.Range(-maxLength, maxLength), 0);
            return color;
        }
    }
}