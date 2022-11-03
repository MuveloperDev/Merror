using System;
using UnityEngine;

namespace EPOOutline
{
    [ExecuteAlways]
    public class TargetStateListener : MonoBehaviour
    {
        public event Action OnVisibilityChanged = null;

        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
        }

        public void ForceUpdate()
        {
            if (OnVisibilityChanged != null)
                OnVisibilityChanged();
        }

        private void OnBecameVisible()
        {
            if (OnVisibilityChanged != null)
                OnVisibilityChanged();
        }

        private void OnBecameInvisible()
        {
            if (OnVisibilityChanged != null)
                OnVisibilityChanged();
        }
    }
}