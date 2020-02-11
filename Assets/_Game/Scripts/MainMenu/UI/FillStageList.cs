using System.Collections;
using System.Linq;
using Ibit.Core.Data;
using Ibit.Core.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class FillStageList : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Scrollbar scrollbar;
        private bool _populated;

        private void OnEnable()
        {
            if (_populated)
            {
                var children = (from Transform child in transform select child.gameObject).ToList();
                children.ForEach(Destroy);
            }

            StageDb.Instance.LoadStages();

            foreach (var stage in StageDb.Instance.StageList)
            {
                var item = Instantiate(buttonPrefab);
                item.transform.SetParent(this.transform);
                item.transform.localScale = Vector3.one;
                item.name = $"ITEM_F{stage.Phase}_L{stage.Level}";
                item.AddComponent<StageLoader>().stage = stage;
                item.GetComponentInChildren<Text>().text = $"Fase: {stage.Phase} - Nível: {stage.Level}";
                item.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter; // Texto alinhado na horizontal e vertical
                item.GetComponent<Button>().interactable = Pacient.Loaded.UnlockedLevels >= stage.Id;
            }

            StartCoroutine(AdjustGrip());

            _populated = true;
        }

        private IEnumerator AdjustGrip()
        {
            yield return null;
            scrollbar.value = 1;
        }
    }
}