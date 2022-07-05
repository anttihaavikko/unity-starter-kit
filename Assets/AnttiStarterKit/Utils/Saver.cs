using UnityEngine;

namespace AnttiStarterKit.Utils
{
    public static class Saver 
    {
        private static string Key => Application.productName + " Save";
        
        public static void Save(object data, string keySuffix = "")
        {
            var json = JsonUtility.ToJson(data, true);
            PermaSave.SetString(Key + keySuffix, json);
        }

        public static T Load<T>(string keySuffix = "") where T : class
        {
            if (!PermaSave.HasKey(Key + keySuffix)) return null;
            
            var json = PermaSave.GetString(Key + keySuffix);
            return JsonUtility.FromJson<T>(json);
        }

        public static bool Exists(string keySuffix = "")
        {
            return PermaSave.HasKey(Key + keySuffix);
        }

        public static void Clear(string keySuffix = "")
        {
            if (!PermaSave.HasKey(Key + keySuffix)) return;
            
            PermaSave.DeleteKey(Key + keySuffix);
        }

        public static void Debug(string keySuffix = "")
        {
            UnityEngine.Debug.Log(PermaSave.GetString(Key + keySuffix, "No data"));
        }

        public static T LoadOrCreate<T>(string keySuffix = "") where T : class, new()
        {
            return Exists(keySuffix) ? Load<T>(keySuffix) : new T();
        }
    }
}