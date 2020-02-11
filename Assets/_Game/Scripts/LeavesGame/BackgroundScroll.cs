using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ibit.LeavesGame
{
    public class BackgroundScroll : MonoBehaviour
    {
        public float ScrollSpeed = 0.1f;
        private Renderer bgRenderer;
        Vector2 startPos;
        private Vector2 _offset;
        private MeshRenderer _mr;

        void Awake()
        {
            bgRenderer = this.GetComponent<Renderer>();
            _mr = this.GetComponent<MeshRenderer>();
        }

        void Update()
        {
            Scroll();
        }

        private void Scroll()
        {
            _offset = _mr.material.mainTextureOffset;
            _offset.x += Time.deltaTime / (1f / ScrollSpeed);
            _mr.material.mainTextureOffset = _offset;
        }
    }
}
