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
                scp.OnSerialMessageReceived += Animate;
            
                scm.OnSerialMessageReceived += PositionOnSerial;
                scm.OnSerialMessageReceived += Animate;

                scc.OnSerialMessageReceived += PositionOnSerial;
                scc.OnSerialMessageReceived += Animate;
            
        }

        private void Update()
        {
#if UNITY_EDITOR
            Move();
#endif
        }
    }
}