using UnityEngine;
using UnityEngine.UI;

namespace Ibit.CakeGame
{
    public class BarScript : MonoBehaviour
    {
        [SerializeField]
        private Image content;

        [SerializeField]
        private float fillAmount;

        [SerializeField]
        private float lerpSpeed;

        public float maxValue { get; set; }

        public float value
        {
            set
            {
                fillAmount = Map(value, 0, maxValue, 0, 1);
            }
        }

        private void HandleBar()
        {
            if (fillAmount != content.fillAmount)
            {
                content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, lerpSpeed);
            }
        }

        private float Map(float val, float inMin, float inMax, float outMin, float outMax) =>
            (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

        private void Update() => HandleBar();
    }
}