using Ibit.Plataform.Manager.Score;
using UnityEngine;

namespace Ibit.Plataform
{
    public class Destroyer : MonoBehaviour
    {
        private void Start()
        {
            DistantiateFromScorer(2f);
        }

        private void DistantiateFromScorer(float units)
        {
            var x = FindObjectOfType<Scorer>().transform.position.x - units;
            this.transform.position = new Vector3(x, 0, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision) => Destroy(collision.gameObject);
    }
}