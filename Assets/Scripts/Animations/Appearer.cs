using Extensions;
using TMPro;
using UnityEngine;

namespace Animations
{
    public class Appearer : MonoBehaviour
    {
        public float appearAfter = -1f;
        public float hideDelay;
        public bool silent;
        public GameObject visuals;

        public TMP_Text text;
        private Vector3 size;

        private void Start()
        {
            var t = transform;
            size = t.localScale;
            t.localScale = Vector3.zero;
            if(visuals) visuals.SetActive(false);

            if (appearAfter >= 0)
                Invoke(nameof(Show), appearAfter);
        }

        public void Show()
        {
            DoSound();

            if(visuals) visuals.SetActive(true);
            Tweener.Instance.ScaleTo(transform, size, 0.3f, 0f, TweenEasings.BounceEaseOut);
        }

        public void Hide()
        {
            CancelInvoke(nameof(Show));

            DoSound();

            Tweener.Instance.ScaleTo(transform, Vector3.zero, 0.2f, 0f, TweenEasings.QuadraticEaseOut);
        
            if(visuals)
                this.StartCoroutine(() => visuals.SetActive(false), 0.2f);
        }

        private void DoSound()
        {
            if (silent) return;
        }

        public void HideWithDelay()
        {
            Invoke(nameof(Hide), hideDelay);
        }

        public void ShowWithText(string t, float delay)
        {
            if (text)
                text.text = t;

            Invoke(nameof(Show), delay);
        }
    }
}
