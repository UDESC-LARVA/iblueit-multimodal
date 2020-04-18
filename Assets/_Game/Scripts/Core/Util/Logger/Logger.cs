using System;
using System.Text;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Core.Util
{
    public abstract class Logger<T> : MonoBehaviour
    {
        [SerializeField] protected string FileName;

        protected bool isLogging;
        protected DateTime recordStart, recordStop;
        protected StringBuilder sb = new StringBuilder();

        [Button("Start Logging")]
        public virtual void StartLogging()
        {
            if (isLogging)
                return;

            Debug.Log($"{typeof(T).Name} started.");

            recordStart = DateTime.Now;
    
            isLogging = true;
        }

        [Button("Stop Logging")]
        public virtual void StopLogging()
        {
            if (!isLogging)
                return;

            Debug.Log($"{typeof(T).Name} stopped.");

            isLogging = false;
            recordStop = DateTime.Now;

            Save();

            sb.Clear();
        }

        protected abstract void Awake();

        protected abstract void Save();

        protected virtual void Start() => StartLogging();
    }
}