using Ibit.Core.Serial;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Plataform
{
    public partial class Player : MonoBehaviour
    {
        public int HeartPoins => heartPoints;

        [SerializeField]
        [BoxGroup("Properties")]
        private int heartPoints = 5;

        private void Awake()
        {
            var scp = FindObjectOfType<SerialControllerPitaco>();
            var scm = FindObjectOfType<SerialControllerMano>();
            var scc = FindObjectOfType<SerialControllerCinta>();
            var sco = FindObjectOfType<SerialControllerOximetro>();


            scp.OnSerialMessageReceived += PositionOnSerialPitaco;
            scp.OnSerialMessageReceived += AnimatePitaco;

            scm.OnSerialMessageReceived += PositionOnSerialMano;
            scm.OnSerialMessageReceived += AnimateMano;

            scc.OnSerialMessageReceived += PositionOnSerialCinta;
            scc.OnSerialMessageReceived += AnimateCinta;

            sco.OnSerialMessageReceived += PositionOnSerialOximetro;

        }

        private void Update()
        {
#if UNITY_EDITOR
            Move();
#endif
        }
    }
}