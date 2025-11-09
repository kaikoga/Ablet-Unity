using System;
using UnityEngine;

namespace Ablet.Hooks
{
    [DefaultExecutionOrder(-9995)]
    public class AbletAwaker : MonoBehaviour
    {
        public static Action OnAbletAwake;
        
        void Awake()
        {
            OnAbletAwake?.Invoke();
            OnAbletAwake = null;
        }
    }
}
