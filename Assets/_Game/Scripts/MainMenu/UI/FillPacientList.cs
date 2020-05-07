using System.Collections;
using System.Linq;
using Assets._Game.Scripts.Core.Api.Extensions;
using Ibit.Core.Data;
using Ibit.Core.Data.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class FillPacientList : MonoBehaviour
    {
        private bool _populated;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Scrollbar scrollbar;

        private async void OnEnable()
        {
            GameObject.Find("Canvas").transform.Find("LoadingBgPanel").gameObject.SetActive(true);

            if (_populated)
            {
                var children = (from Transform child in transform select child.gameObject).ToList();
                children.ForEach(Destroy);
            }

            var pacientList = await DataManager.Instance.GetPacients();

            var obstructiveTranslation = "Obstrutivo";
            var restrictiveTranslation = "Restritivo";
            var healthyTranslation = "Saudável";

            if (pacientList.Data.Count != 0)
            {

                foreach (var pacient in pacientList.Data.OrderBy(p => p.Name))
                {
                    var item = Instantiate(itemPrefab);
                    item.transform.SetParent(this.transform);
                    item.transform.localScale = Vector3.one;
                    item.name = $"ITEM_{pacient.Id}_{pacient.Name}";

                    var holder = item.AddComponent<PacientLoader>();
                    holder.pacient = Pacient.MapFromDto(pacient);

                    var disfunction = EnumExtensions.GetValueFromDescription<ConditionType>(pacient.Condition) == ConditionType.Healthy ? healthyTranslation :
                        (EnumExtensions.GetValueFromDescription<ConditionType>(pacient.Condition) == ConditionType.Obstructive ? obstructiveTranslation : restrictiveTranslation);

                    item.GetComponentInChildren<Text>().text = $"Nome: {pacient.Name} - {pacient.Birthday:dd/MM/yyyy} - {disfunction}";
                    item.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft; // Texto alinhado na vertical e a esquerda
                }

                StartCoroutine(AdjustGrip());

                _populated = true;

            }

            GameObject.Find("Canvas").transform.Find("LoadingBgPanel").gameObject.SetActive(false);
        }

        private IEnumerator AdjustGrip()
        {
            yield return null;
            scrollbar.value = 1;
        }
    }
}