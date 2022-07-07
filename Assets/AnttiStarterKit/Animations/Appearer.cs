using System;
using TMPro;
using UnityEngine;

namespace AnttiStarterKit.Animations
{
    public class Appearer : MonoBehaviour
    {
        public float appearAfter = -1f;
        public float hideDelay;
        public bool silent;
        public bool inScreenSpace;

        public TMP_Text text;

        private Transform parent;
        private GameObject wrap;
        
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
            
            var t = transform;
            var goName = gameObject.name;
            var go = new GameObject(goName + " Appearer Parent");
            wrap = new GameObject(goName + " Appearer Wrap");
            parent = go.transform;
            parent.parent = t.parent;
            wrap.transform.parent = parent;
            t.parent = wrap.transform;
            
            parent.localScale = Vector3.zero;
            wrap.SetActive(false);

            if (appearAfter >= 0)
                Invoke(nameof(Show), appearAfter);
        }

        public void ShowAfter(float delay)
        {
            Invoke(nameof(Show), delay);
        }

        public void Show()
        {
            CancelInvoke(nameof(Hide));
            CancelInvoke(nameof(MakeInactive));
            DoSound();

            wrap.SetActive(true);
            Tweener.Instance.ScaleTo(parent, Vector3.one, 0.3f, 0f, TweenEasings.BounceEaseOut);
        }

        public void Hide()
        {
            CancelInvoke(nameof(Show));
            DoSound();

            Tweener.Instance.ScaleTo(parent, Vector3.zero, 0.2f, 0f, TweenEasings.QuadraticEaseOut);
        
            Invoke(nameof(MakeInactive), 0.2f);
        }

        private void MakeInactive()
        {
            wrap.SetActive(false);
        }

        private void DoSound()
        {
            if (silent) return;

            var p = transform.position;
            var pos = inScreenSpace && cam ? cam.ScreenToWorldPoint(p) : p;

            // TODO SOUND
        }

        public void HideWithDelay()
        {
            Invoke(nameof(Hide), hideDelay);
        }
        
        public void HideWithDelay(float delay)
        {
            Invoke(nameof(Hide), delay);
        }

        public void ShowWithText(string t, float delay)
        {
            if (text)
                text.text = t;

            Invoke(nameof(Show), delay);
        }
    }
}
