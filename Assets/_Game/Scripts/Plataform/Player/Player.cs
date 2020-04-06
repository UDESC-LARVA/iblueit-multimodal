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


            scp.OnSerialMessageReceived += PositionOnSerial;
            scp.OnSerialMessageReceived += AnimatePitaco;

            scm.OnSerialMessageReceived += PositionOnSerial;
            scm.OnSerialMessageReceived += AnimateMano;

            scc.OnSerialMessageReceived += PositionOnSerial;
            scc.OnSerialMessageReceived += AnimateCinta;

        }

        private void Update()
        {
#if UNITY_EDITOR
            Move();
#endif
        }
    }
}