using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class PacientLoader : MonoBehaviour
    {
        public Pacient pacient;

        private void OnEnable()
        {
            this.GetComponent<Button>().onClick.AddListener(OnPacientSelected);
        }

        private void OnPacientSelected()
        {
            Pacient.Loaded = pacient;
            
            GameObject.Find("Canvas").transform.Find("Load Menu").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("Player Menu").gameObject.SetActive(true);
            //GameObject.Find("Canvas").transform.Find("Parameters Menu").gameObject.SetActive(true);
            Debug.Log($"{pacient.Name} loaded.");
        }
    }
}