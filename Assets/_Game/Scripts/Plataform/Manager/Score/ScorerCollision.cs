using Ibit.Plataform.Manager.Spawn;
using UnityEngine;

namespace Ibit.Plataform.Manager.Score
{
    public partial class Scorer
    {
        /// <summary>
        /// Obstacle Scoring
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("AirTarget") || collision.gameObject.CompareTag("WaterTarget"))
            {
                FindObjectOfType<Spawner>().PerformanceOnPlayerMiss(collision.gameObject.tag);
                return;
            }

            if (!collision.gameObject.CompareTag("AirObstacle") && !collision.gameObject.CompareTag("WaterObstacle"))
                return;

            if (collision.gameObject.GetComponent<Obstacle>().HeartPoint < 1)
                return;

            FindObjectOfType<Spawner>().PerformanceOnPlayerMiss(collision.gameObject.tag);
            score += collision.gameObject.GetComponent<Obstacle>().Score;
        }

        /// <summary>
        /// Target Scoring
        /// </summary>
        /// <param name="hit"></param>
        private void ScoreOnPlayerCollision(GameObject hit)
        {
            if (hit.CompareTag("AirTarget") || hit.CompareTag("WaterTarget"))
            {
                score += hit.GetComponent<Target>().Score;
            }
        }
    }
}