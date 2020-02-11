using UnityEngine;

namespace Ibit.Plataform
{
    public class MoveObject : MonoBehaviour
    {
        public float Speed { get; set; } = 1;

        private void Update() => this.transform.Translate(new Vector3(-Time.deltaTime * Speed, 0f));
    }
}