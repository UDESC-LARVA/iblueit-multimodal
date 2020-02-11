using UnityEngine;

namespace Ibit.WaterGame
{
    public class CupBehaviour : MonoBehaviour
    {
        public Sprite[] waterLevels;

        private void Start() => FindObjectOfType<Scorer>().ChangeWaterLevelEvent += ChangeWaterSprite;

        private void ChangeWaterSprite(int level)
        {
            // Level 0 -> Pico >= 75% (3 estrelas)
            // Level 1 -> Pico >= 50% (2 estrelas)
            // Level 2 -> Pico >= 25% (1 estrelas)
            // Level 3 -> Pico < 25% (0 estrelas)
            this.gameObject.GetComponent<SpriteRenderer>().sprite = waterLevels[level];
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                ChangeWaterSprite(0);
            }
        }
    }
}