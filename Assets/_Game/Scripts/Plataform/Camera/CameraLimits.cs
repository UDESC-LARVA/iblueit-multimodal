using UnityEngine;

namespace Ibit.Plataform.Camera
{
    public class CameraLimits : MonoBehaviour
    {
        [HideInInspector]
        public static float Boundary;

        public void Awake() => Boundary = UnityEngine.Camera.main.orthographicSize * 0.75f;
    }
}