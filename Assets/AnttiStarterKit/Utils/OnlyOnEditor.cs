using System;
using UnityEngine;

namespace AnttiStarterKit.Utils
{
    public class OnlyOnEditor : MonoBehaviour
    {
        private void Awake()
        {
            if(!Application.isEditor) gameObject.SetActive(false);
        }
    }
}