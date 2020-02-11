using UnityEngine;

namespace Ibit.Plataform
{
    public partial class Player
    {
        private void Move() => this.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 5f, Input.GetAxis("Vertical") * Time.deltaTime * 5f, 0f);
    }
}