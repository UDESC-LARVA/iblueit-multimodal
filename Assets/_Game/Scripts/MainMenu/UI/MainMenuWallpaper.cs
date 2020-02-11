using UnityEngine;

public class MainMenuWallpaper : MonoBehaviour
{
    private Vector2 _offset;
    private MeshRenderer _mr;
    private Renderer _bgRenderer;
    [SerializeField] private bool scroll;
    [SerializeField] private float scrollSpeed = 0.1f;
    [SerializeField] private Material day;
    [SerializeField] private Material afternoon;
    [SerializeField] private Material night;

    private void Awake()
    {
        _bgRenderer = this.GetComponent<Renderer>();
        _mr = this.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        SwitchBackground();
    }

    private void SwitchBackground()
    {
        switch (Random.Range(0, 3))
        {
            default:
                case 0:
                    _bgRenderer.material = day;
                break;

                case 1:
                    _bgRenderer.material = afternoon;
                break;

                case 2:
                    _bgRenderer.material = night;
                break;
        }
    }

    private void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        _offset = _mr.material.mainTextureOffset;
        _offset.x += Time.deltaTime / (1f / scrollSpeed);
        _mr.material.mainTextureOffset = _offset;
    }
}