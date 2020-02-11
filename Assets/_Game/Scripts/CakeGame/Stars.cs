using UnityEngine;
using UnityEngine.UI;

namespace Ibit.CakeGame
{
    public class Stars : MonoBehaviour
    {
        /// <summary>
        /// Conta quantas estrelas o jogador alcançou.
        /// </summary>
        public int score;

        /// <summary>
        /// Ajusta os sprites de estrela vazia.
        /// </summary>
        public Sprite[] star = new Sprite[3];

        public Sprite starOn;

        

        [SerializeField]
        public ScoreMenu scoreMenu;

        /// <summary>
        /// Ajusta a velocidade de preenchimento.
        /// </summary>
        [SerializeField]
        private float lerpSpeed;

        /// <summary>
        /// Define a imagem de preenchimento.
        /// </summary>
        [SerializeField]
        private Image[] content = new Image[3];

        public void FillStars(int index) => content[index].fillAmount = 1;

        public void UnfillStars()
        {
            content[0].fillAmount = 0;
            content[1].fillAmount = 0;
            content[2].fillAmount = 0;
        }

        public void FillStarsFinal(int starQty)
        {
            for (var i = 0; i < starQty; i++)
            {
                content[i].fillAmount = 1;
            }
        }
        

        public void OnFinish() => scoreMenu.ToggleScoreMenu();
    }
}