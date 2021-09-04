using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AnttiStarterKit.Managers;
using AnttiStarterKit.ScriptableObjects;
using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SoundComposition))]
    public class SoundCompositionEditor : UnityEditor.Editor
    {
        private Vector2 _listScroll, _playerScroll;
        private Texture2D _black;
        private SoundComposition _object;

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
            var amount = 0.15f;
            _black = MakeTex(600, 1, new Color(amount, amount, amount));
            _object = (SoundComposition)target;

            if (_object && _object.rows == null)
            {
                _object.rows = new List<SoundCompositionRow>();
                return;
            }

            // _sounds = _object.rows.Select(r => r.clip).ToList();
            // _soundVolumes = _object.rows.Select(r => r.volume).ToList();
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            // DrawDefaultInspector();
            
            EditorGUILayout.BeginVertical(new GUIStyle
            {
                fixedHeight = 500
            });

            _listScroll = GUILayout.BeginScrollView(_listScroll);
            EditorGUILayout.Space();
            
            var eventStyle = new GUIStyle
            {
                normal = new GUIStyleState
                {
                    background = _black
                }
            };
            
            var clips = GetAtPath<AudioClip>("Sounds");
            var row = 0;
            foreach (var clip in clips)
            {
                EditorGUILayout.BeginHorizontal(row % 2 == 0 ? eventStyle : new GUIStyle());
                EditorGUILayout.LabelField(clip.name);

                if (GUILayout.Button("Add", GUILayout.MaxWidth(70.0f)))
                {
                    _object.rows.Add(new SoundCompositionRow
                    {
                        clip = clip,
                        volume = 1f
                    });
                }
                
                EditorGUILayout.EndHorizontal();

                row++;
            }
            
            EditorGUILayout.Space();
            GUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
            
            DrawPlayer();
        }

        private void DrawPlayer()
        {
            EditorGUILayout.BeginVertical();

            _playerScroll = GUILayout.BeginScrollView(_playerScroll, new GUIStyle
            {
                padding = new RectOffset(0, 0, 20, 0)
            });

            for (var i = 0; i < _object.rows.Count; i++)
            {
                var row = _object.rows[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label(row.clip.name, EditorStyles.boldLabel, GUILayout.Width(80.0f));
                row.volume = EditorGUILayout.Slider("", row.volume, 0f, 5f);
                if (GUILayout.Button("Play", GUILayout.MaxWidth(70.0f)))
                {
                    if (AudioManager.Instance)
                    {
                        AudioManager.Instance.PlayEffectAt(row.clip, Vector3.zero, row.volume);
                    }
                }
                if (GUILayout.Button("Remove", GUILayout.MaxWidth(70.0f)))
                {
                    _object.rows.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
            {
                _object.Play();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            GUILayout.EndScrollView();
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