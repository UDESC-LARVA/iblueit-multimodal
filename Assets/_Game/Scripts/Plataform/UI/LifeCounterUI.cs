using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Plataform.UI
{
    public class LifeCounterUI : MonoBehaviour
    {
        [SerializeField]
        private Image fillImage;

        private float startHP;
        private Ibit.Plataform.Player plr;

        private void Awake()
        {
            plr = FindObjectOfType<Ibit.Plataform.Player>();
            startHP = plr.HeartPoins;
            plr.OnObjectHit += UpdateHeartPoints;
        }

        private void UpdateHeartPoints(GameObject go) => fillImage.fillAmount = plr.HeartPoins / startHP;
    }
}