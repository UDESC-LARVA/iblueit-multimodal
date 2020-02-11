using Ibit.Plataform.Data;
using UnityEngine;

namespace Ibit.Plataform.UI
{
    public class Wallpaper : MonoBehaviour
    {
        private Vector2 _offset;
        private MeshRenderer _mr;
        private Renderer bgRenderer;

        [SerializeField]
        private bool scroll;

        [SerializeField]
        private float scrollSpeed = 0.1f;

        [SerializeField]
        private Material day;

        [SerializeField]
        private Material day2;

        [SerializeField]
        private Material afternoon;

        [SerializeField]
        private Material night;

        private void Awake()
        {
            bgRenderer = this.GetComponent<Renderer>();
            _mr = this.GetComponent<MeshRenderer>();
        }

        private void Start() => SwitchBackground();

        private void SwitchBackground()
        {
            switch (StageModel.Loaded.Phase)
            {
                default:
                case 1:
                    bgRenderer.material = day;
                    break;

                case 2:
                    bgRenderer.material = day2;
                    break;

                case 3:
                    bgRenderer.material = afternoon;
                    break;

                case 4:
                    bgRenderer.material = night;
                    break;
            }
        }

        private void Update() => Scroll();

        private void Scroll()
        {
            _offset = _mr.material.mainTextureOffset;
            _offset.x += Time.deltaTime / (1f / scrollSpeed);
            _mr.material.mainTextureOffset = _offset;
        }
    }
}