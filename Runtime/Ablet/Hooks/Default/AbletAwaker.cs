using System;
using UnityEngine;

namespace Ablet.Hooks.Default
{
    [AddComponentMenu("")]
    [DefaultExecutionOrder(-9995)]
    public class AbletAwaker : MonoBehaviour
    {
        public static event Action? OnAbletAwake;
        
        void Awake()
        {
            OnAbletAwake?.Invoke();
            OnAbletAwake = null;
        }
    }
}
