using UnityEngine;

namespace Ibit.Plataform.UI
{
    public class Background : MonoBehaviour
    {
        private void Start() => ResizeToCamera();

        private void ResizeToCamera()
        {
            var camPos = UnityEngine.Camera.main.transform.position;
            this.gameObject.transform.position = new Vector3(camPos.x, camPos.y);
            var height = 2f * UnityEngine.Camera.main.orthographicSize;
            var width = height * UnityEngine.Camera.main.aspect;
            this.transform.localScale = new Vector3(width, height);
        }
    }
}