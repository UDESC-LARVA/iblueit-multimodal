using Ibit.Plataform.Manager.Score;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Plataform.UI
{
    public class GameScoreUI : MonoBehaviour
    {
        private Scorer scorer;

        private void Awake() => scorer = FindObjectOfType<Scorer>();

        private void Update() => GetComponent<Text>().text = $"{scorer.Score:####}";
    }
}