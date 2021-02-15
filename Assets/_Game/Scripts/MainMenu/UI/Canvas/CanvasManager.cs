using Ibit.Core.Audio;
using Ibit.Core.Data;
using Ibit.Core.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI.Canvas
{
    public partial class CanvasManager : MonoBehaviour
    {
        public void ShowPlayerInfo ()
        {
            //%%%%%%%%%% Tradução da condição respiratória para português %%%%%%%%%%
            var obstructiveTranslation = "Obstrutivo";
            var restrictiveTranslation = "Restritivo";
            var healthyTranslation = "Saudável";

            var disfunction = Pacient.Loaded.Condition == ConditionType.Healthy ? healthyTranslation :
                (Pacient.Loaded.Condition == ConditionType.Obstructive ? obstructiveTranslation : restrictiveTranslation);
            //////////////////////////////////////////////////////////////////////////////

            GameObject.Find("Canvas").transform.Find("Informations Menu").gameObject.SetActive(true);

            var TextPacient = GameObject.FindWithTag("txPacient").gameObject.GetComponent<Text>();
            var TextPitacoInfos = GameObject.FindWithTag("txPitacoInfos").gameObject.GetComponent<Text>();
            var TextManoInfos = GameObject.FindWithTag("txManoInfos").gameObject.GetComponent<Text>();
            var TextCintaInfos = GameObject.FindWithTag("txCintaInfos").gameObject.GetComponent<Text>();

            TextPacient.text =      $"Jogador: {Pacient.Loaded.Name}\n" +
                                    $"Condição Respiratória: {disfunction}\n" +
                                    $"Partidas Jogadas: {Pacient.Loaded.PlaySessionsDone}\n";

            TextPitacoInfos.text =  $"Pico Exp.: {PitacoFlowMath.ToLitresPerMinute(Pacient.Loaded.CapacitiesPitaco.RawExpPeakFlow):F2} L/min ({Pacient.Loaded.CapacitiesPitaco.RawExpPeakFlow:F2} Pa)\n" +
                                    $"Pico Ins.: {PitacoFlowMath.ToLitresPerMinute(Pacient.Loaded.CapacitiesPitaco.RawInsPeakFlow):F2} L/min ({Pacient.Loaded.CapacitiesPitaco.RawInsPeakFlow:F2} Pa)\n" +
                                    $"Tempo Ins.: {Pacient.Loaded.CapacitiesPitaco.RawInsFlowDuration / 1000f:F1} s\n" +
                                    $"Tempo Exp.: {Pacient.Loaded.CapacitiesPitaco.RawExpFlowDuration / 1000f:F1} s\n" +
                                    $"Tins/Texp: {((Pacient.Loaded.CapacitiesPitaco.RawInsFlowDuration / 1000f) / (Pacient.Loaded.CapacitiesPitaco.RawExpFlowDuration / 1000f)):F1}\n" +
                                    $"Freq. Resp. Média: {Pacient.Loaded.CapacitiesPitaco.RawRespRate * 60f:F1} rpm\n";
            
            TextManoInfos.text =    $"Pico Exp.: {ManoFlowMath.ToCentimetersofWater(Pacient.Loaded.CapacitiesMano.RawExpPeakFlow):F2} cmH2O ({Pacient.Loaded.CapacitiesMano.RawExpPeakFlow:F2} Pa)\n" +
                                    $"Pico Ins.: {ManoFlowMath.ToCentimetersofWater(Pacient.Loaded.CapacitiesMano.RawInsPeakFlow):F2} cmH2O ({Pacient.Loaded.CapacitiesMano.RawInsPeakFlow:F2} Pa)\n" +
                                    $"Tempo Ins.: {Pacient.Loaded.CapacitiesMano.RawInsFlowDuration / 1000f:F1} s\n" +
                                    $"Tempo Exp.: {Pacient.Loaded.CapacitiesMano.RawExpFlowDuration / 1000f:F1} s\n" +
                                    $"Tins/Texp: {((Pacient.Loaded.CapacitiesMano.RawInsFlowDuration / 1000f) / (Pacient.Loaded.CapacitiesMano.RawExpFlowDuration / 1000f)):F1}\n";
            
            
            TextCintaInfos.text =   $"Pico Exp.: {Pacient.Loaded.CapacitiesCinta.RawExpPeakFlow:F2} Pa\n" +
                                    $"Pico Ins.: {Pacient.Loaded.CapacitiesCinta.RawInsPeakFlow:F2} Pa\n" +
                                    $"Tempo Ins.: {Pacient.Loaded.CapacitiesCinta.RawInsFlowDuration / 1000f:F1} s\n" +
                                    $"Tempo Exp.: {Pacient.Loaded.CapacitiesCinta.RawExpFlowDuration / 1000f:F1} s\n" +
                                    $"Tins/Texp: {((Pacient.Loaded.CapacitiesCinta.RawInsFlowDuration / 1000f) / (Pacient.Loaded.CapacitiesCinta.RawExpFlowDuration / 1000f)):F1}\n" +
                                    $"Freq. Resp. Média: {Pacient.Loaded.CapacitiesCinta.RawRespRate * 60f:F1} rpm";

        }

        private void Awake ()
        {
            if (Pacient.Loaded == null)
                return;

            GameObject.Find ("Canvas").transform.Find ("Start Panel").gameObject.SetActive (false);
            GameObject.Find ("Canvas").transform.Find ("Player Menu").gameObject.SetActive (true);
        }

        private void PlayClick () => SoundManager.Instance.PlaySound ("BtnClickUI");

        private void Start () => AddClickSfxToButtons ();

        private void AddClickSfxToButtons ()
        {
            foreach (var component in GameObject.Find ("Canvas").GetComponentsInChildren (typeof (Button), true))
            {
                var btn = (Button) component;
                btn.onClick.AddListener (PlayClick);
            }
        }
    }
}