using Ibit.Plataform.Manager.Spawn;
using UnityEngine;

namespace Ibit.Plataform
{
    public partial class Obstacle
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                TakeDamage();
            }
        }

        private void TakeDamage()
        {
            heartPoint--;
        }
    }
}