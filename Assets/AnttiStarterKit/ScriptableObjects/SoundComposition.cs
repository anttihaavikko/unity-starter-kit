using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
using AnttiStarterKit.Utils;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace AnttiStarterKit.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Sound composition", menuName = "Composite sound", order = 0)]
    public class SoundComposition : ScriptableObject
    {
        public List<SoundCompositionRow> rows;
        public List<SoundCollectionRow> collections;

        public void Play()
        {
            Play(Vector3.zero);
        }

        public List<GameObject> PlayInEditMode(Vector3 pos)
        {
            return rows.Select(r => PlayInEditMode(r.clip, pos, r.volume)).ToList();
        }

        public void Play(Vector3 pos, float volume = 1f)
        {
            var am = AudioManager.Instance;
            if (!am && !EditorApplication.isPlaying) return;
            
            foreach (var row in rows)
            {
                am.PlayEffectAt(row.clip, pos, row.volume * volume);
            }
            
            foreach (var row in collections)
            {
                am.PlayEffectFromCollection(row.collection, pos, row.volume * volume);
            }
        }

        public static GameObject PlayInEditMode(AudioClip clip, Vector3 pos, float volume = 1f)
        {
            var gameObject = new GameObject("One shot audio");
            gameObject.transform.position = pos;
            var actor = (ActAfter) gameObject.AddComponent(typeof(ActAfter));
            var audioSource = (AudioSource) gameObject.AddComponent(typeof (AudioSource));
            audioSource.clip = clip;
            audioSource.spatialBlend = 1f;
            audioSource.volume = volume;
            audioSource.Play();
            var delay = clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale);
            return gameObject;
        }
    }

    [Serializable]
    public class SoundCompositionRow
    {
        public AudioClip clip;
        public float volume = 1f;
    }
    
    [Serializable]
    public class SoundCollectionRow
    {
        public SoundCollection collection;
        public float volume = 1f;
    }
}