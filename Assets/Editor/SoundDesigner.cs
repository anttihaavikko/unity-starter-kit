using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;

public class SoundDesigner : EditorWindow
{
    AudioManager am;
    Vector2 listScroll;
    List<int> sounds = new List<int>();
    List<float> soundVolumes = new List<float>();
    string output;

    [MenuItem("Window/SoundDesigner")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SoundDesigner));
    }

    public void FindAudioManager()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        if (am)
        {
            listScroll = GUILayout.BeginScrollView(listScroll);
            EditorGUILayout.Space();

            for (int i = 0; i < am.effects.Length; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(am.effects[i].name, EditorStyles.boldLabel, GUILayout.MaxWidth(150.0f));
                if(GUILayout.Button("Play", GUILayout.MaxWidth(70.0f)))
                {
                    Play(i);
                }
                if (GUILayout.Button("Add", GUILayout.MaxWidth(70.0f)))
                {
                    sounds.Add(i);
                    soundVolumes.Add(1f);
                }
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            GUILayout.EndScrollView();
        }
        else
        {
            if (GUILayout.Button("Find AudioManager"))
            {
                FindAudioManager();
            }
        }

        if (sounds.Count > 0)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();

            output = "";

            for (int i = 0; i < sounds.Count; i++)
            {
                var sb = sounds[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label(am.effects[sounds[i]].name, EditorStyles.boldLabel, GUILayout.MaxWidth(150.0f));
                soundVolumes[i] = EditorGUILayout.Slider("", soundVolumes[i], 0f, 2f);
                if (GUILayout.Button("Play", GUILayout.MaxWidth(70.0f)))
                {
                    Play(sounds[i]);
                }
                if (GUILayout.Button("Remove", GUILayout.MaxWidth(70.0f)))
                {
                    sounds.RemoveAt(i);
                    soundVolumes.RemoveAt(i);
                }
                GUILayout.EndHorizontal();

                var pars = sounds[i] + ", transform.position, " + soundVolumes[i];
                output += "AudioManager.Instance.PlayEffectAt(" + pars + "f);\n";
            }

            EditorGUILayout.Space();

            if(EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Play in play mode", GUILayout.MaxHeight(50f)))
                {
                    for (int i = 0; i < sounds.Count; i++)
                    {
                        am.PlayEffectAt(sounds[i], Vector3.zero, soundVolumes[i]);
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Play", GUILayout.MaxHeight(50f)))
                {
                    for (int i = 0; i < sounds.Count; i++)
                    {
                        Play(sounds[i]);
                    }
                }
            }

            EditorGUILayout.Space();
            GUILayout.TextArea(output, GUILayout.MinHeight(100f));
            EditorGUILayout.Space();

            if (GUILayout.Button("Clear"))
            {
                sounds.Clear();
                soundVolumes.Clear();
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginHorizontal();
    }

    void Play(int i)
    {
        var path = "Assets/Sounds/" + am.effects[i].name + ".wav";
        var c = (AudioClip)EditorGUIUtility.Load(path);
        PlayClip(c);
    }

    public static void PlayClip(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new System.Type[] {
         typeof(AudioClip)
        },
        null
        );
        method.Invoke(
            null,
            new object[] {
         clip
        }
        );
    }
}