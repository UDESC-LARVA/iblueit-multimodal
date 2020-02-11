/*
    Autor: Jhonatan Thallisson Cabral Néry
*/

using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class ChangeColorOnCalibrationPitaco : MonoBehaviour
    {
        private void Update()
        {
            if(Pacient.Loaded != null && Pacient.Loaded.CalibrationPitacoDone)
		    {
			    GetComponent<Animator>().SetBool("Calib", true);
		    }
        }
    }
}