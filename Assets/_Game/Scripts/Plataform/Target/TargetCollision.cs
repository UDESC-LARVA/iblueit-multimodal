using UnityEngine;

namespace Ibit.Plataform
{
    public partial class Target
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
                return;

            Destroy(this.gameObject);
        }
    }
}