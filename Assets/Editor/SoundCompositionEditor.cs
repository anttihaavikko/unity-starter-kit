using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AnttiStarterKit.Managers;
using AnttiStarterKit.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SoundComposition))]
    public class SoundCompositionEditor : UnityEditor.Editor
    {
        private Vector2 _listScroll, _playerScroll;
        private Texture2D _black;
        private string _filter = "";
        private List<GameObject> _garbage = new();

        private static Vector3 SoundPos => Camera.main!.transform.position;

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            var pix = new Color[width * height];
 
            for(var i = 0; i < pix.Length; i++)
                pix[i] = col;
 
            var result = new Texture2D(width, height);
            
            result.SetPixels(pix);
            result.Apply();
 
            return result;
        }

        private void OnEnable()
        {
            const float amount = 0.15f;
            _black = MakeTex(600, 1, new Color(amount, amount, amount));
            var rows = serializedObject.FindProperty("rows");
        }
        
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            var assets = new List<T>();
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var t in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if(asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var rows =  serializedObject.FindProperty("rows");

            EditorGUILayout.BeginVertical(new GUIStyle
            {
                fixedHeight = 350
            });

            DrawFilter();

            _listScroll = GUILayout.BeginScrollView(_listScroll);
            EditorGUILayout.Space();
            
            var eventStyle = new GUIStyle
            {
                normal = new GUIStyleState
                {
                    background = _black
                }
            };
            
            var collections = FindAssetsByType<SoundCollection>();
            var row = 0;
            foreach (var collection in collections)
            {
                if (_filter.Length > 0 && !collection.name.Contains(_filter)) continue;
                
                EditorGUILayout.BeginHorizontal(row % 2 == 0 ? eventStyle : new GUIStyle());
                EditorGUILayout.LabelField("★ " + collection.name);
                
                if (GUILayout.Button("►", GUILayout.MaxWidth(20.0f)))
                {
                    if (AudioManager.Instance)
                    {
                        AudioManager.Instance.PlayEffectFromCollection(collection, SoundPos, 1f);
                    }
                }

                if (GUILayout.Button("Add", GUILayout.MaxWidth(70.0f)))
                {
                    var index = serializedObject.FindProperty("collections").arraySize;
                    serializedObject.FindProperty("collections").InsertArrayElementAtIndex(index);
                    serializedObject.FindProperty("collections").GetArrayElementAtIndex(index).FindPropertyRelative("collection").objectReferenceInstanceIDValue = collection.GetInstanceID();
                    serializedObject.FindProperty("collections").GetArrayElementAtIndex(index).FindPropertyRelative("volume").floatValue = 1f;
                    serializedObject.ApplyModifiedProperties();
                }
                
                EditorGUILayout.EndHorizontal();

                row++;
            }
            
            var clips = GetAtPath<AudioClip>("Sounds");
            row = 0;
            foreach (var clip in clips)
            {
                if (_filter.Length > 0 && !clip.name.Contains(_filter)) continue;
                
                EditorGUILayout.BeginHorizontal(row % 2 == 0 ? eventStyle : new GUIStyle());
                EditorGUILayout.LabelField(clip.name);
                
                if (GUILayout.Button("►", GUILayout.MaxWidth(20.0f)))
                {
                    PlayClip(clip, 1f);
                }

                if (GUILayout.Button("Add", GUILayout.MaxWidth(70.0f)))
                {
                    var index = serializedObject.FindProperty("rows").arraySize;
                    serializedObject.FindProperty("rows").InsertArrayElementAtIndex(index);
                    serializedObject.FindProperty("rows").GetArrayElementAtIndex(index).FindPropertyRelative("clip").objectReferenceInstanceIDValue = clip.GetInstanceID();
                    serializedObject.FindProperty("rows").GetArrayElementAtIndex(index).FindPropertyRelative("volume").floatValue = 1f;
                    serializedObject.ApplyModifiedProperties();
                }
                
                EditorGUILayout.EndHorizontal();

                row++;
            }
            
            EditorGUILayout.Space();
            GUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
            
            DrawPlayer();
        }

        private void DrawFilter()
        {
            EditorGUILayout.BeginHorizontal();

            _filter = EditorGUILayout.TextField(_filter);
            
            if (GUILayout.Button("Clear", GUILayout.MaxWidth(120.0f)))
            {
                _filter = "";
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPlayer()
        {
            EditorGUILayout.BeginVertical(new GUIStyle
            {
                padding = new RectOffset(0, 0, 20, 0)
            });
            
            for (var i = 0; i < serializedObject.FindProperty("collections").arraySize; i++)
            {
                var row = serializedObject.FindProperty("collections").GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("►", GUILayout.MaxWidth(20.0f)))
                {
                    if (AudioManager.Instance)
                    {
                        AudioManager.Instance.PlayEffectFromCollection((SoundCollection)row.FindPropertyRelative("collection").objectReferenceValue, SoundPos, row.FindPropertyRelative("volume").floatValue);
                    }
                }
                GUILayout.Label("★ " + row.FindPropertyRelative("collection").objectReferenceValue.name, EditorStyles.boldLabel, GUILayout.Width(80.0f));
                row.FindPropertyRelative("volume").floatValue = EditorGUILayout.Slider("", row.FindPropertyRelative("volume").floatValue, 0f, 5f);
                if (GUILayout.Button("X", GUILayout.MaxWidth(20.0f)))
                {
                    serializedObject.FindProperty("collections").DeleteArrayElementAtIndex(i);
                }
                GUILayout.EndHorizontal();
            }

            for (var i = 0; i < serializedObject.FindProperty("rows").arraySize; i++)
            {
                var row = serializedObject.FindProperty("rows").GetArrayElementAtIndex(i);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("►", GUILayout.MaxWidth(20.0f)))
                {
                    var clip = (AudioClip)row.FindPropertyRelative("clip").objectReferenceValue;
                    PlayClip(clip, row.FindPropertyRelative("volume").floatValue);
                }
                GUILayout.Label(row.FindPropertyRelative("clip").objectReferenceValue.name, EditorStyles.boldLabel, GUILayout.Width(80.0f));
                row.FindPropertyRelative("volume").floatValue = EditorGUILayout.Slider("", row.FindPropertyRelative("volume").floatValue, 0f, 5f);
                if (GUILayout.Button("X", GUILayout.MaxWidth(20.0f)))
                {
                    serializedObject.FindProperty("rows").DeleteArrayElementAtIndex(i);
                }
                GUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("► Play"))
            {
                var sc = ((SoundComposition)serializedObject.targetObject);
                if (!EditorApplication.isPlaying)
                {
                    _garbage.AddRange(sc.PlayInEditMode(SoundPos));
                }
                else
                {
                    sc.Play(SoundPos);
                }
            }

            if (_garbage.Count > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                
                GUILayout.BeginHorizontal();
                
                GUILayout.Label($"{_garbage.Count} temp garbage in hierarchy!", EditorStyles.boldLabel);
                
                if (GUILayout.Button("Clear"))
                {
                    _garbage.ForEach(DestroyImmediate);
                    _garbage.Clear();
                }
                
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            // GUILayout.EndScrollView();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void PlayClip(AudioClip clip, float volume)
        {
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlayEffectAt(clip, SoundPos, volume);
            }
            else
            {
                _garbage.Add(SoundComposition.PlayInEditMode(clip, SoundPos, volume));
            }
        }

        public static T[] GetAtPath<T> (string path) {
       
            var al = new ArrayList();
            var fileEntries = Directory.GetFiles(Application.dataPath+"/"+path);
            foreach(var fileName in fileEntries)
            {
                var index = fileName.LastIndexOf("/", StringComparison.Ordinal);
                var localPath = "Assets/" + path;
           
                if (index > 0)
                    localPath += fileName.Substring(index);
               
                // var t = Resources.LoadAssetAtPath(localPath, typeof(T));
                var t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

                if(t != null)
                    al.Add(t);
            }
            var result = new T[al.Count];
            for(var i=0;i<al.Count;i++)
                result[i] = (T)al[i];
           
            return result;
        }
    }
}