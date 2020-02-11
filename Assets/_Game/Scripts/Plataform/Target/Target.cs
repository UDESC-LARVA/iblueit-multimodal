using Ibit.Plataform.Camera;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Spawn;
using UnityEngine;

namespace Ibit.Plataform
{
    public partial class Target : MonoBehaviour
    {
        [SerializeField] private ObjectModel _model;

        public float Score { get; private set; }

        public void Build(ObjectModel model)
        {
            _model = model;
            CalculateHeight();
            CalculateScore();
            FindObjectOfType<Spawner>().OnUpdatedPerformanceTarget += OnUpdatedPerformance;
        }

        private void OnUpdatedPerformance(float insAcc, float expAcc)
        {
            CalculateHeight(_model.PositionYFactor > 0 ? insAcc : expAcc);
        }

        private void CalculateScore()
        {
            Score = Mathf.Abs(this.transform.position.y) * (1f + _model.DifficultyFactor) * (1 + StageModel.Loaded.ObjectSpeedFactor);
        }

        private void CalculateHeight(float performanceAccumulator = 0)
        {
            var tmpPos = this.transform.position;

            tmpPos.y = (1f + performanceAccumulator) * CameraLimits.Boundary * _model.DifficultyFactor;

            tmpPos.y = _model.PositionYFactor > 0 ?
                Mathf.Clamp(tmpPos.y, 0f, CameraLimits.Boundary) :
                Mathf.Clamp(-tmpPos.y, -CameraLimits.Boundary, 0f);

            this.transform.position = tmpPos;
        }

        private void OnDestroy()
        {
            var spwn = FindObjectOfType<Spawner>();
            if (spwn != null)
                spwn.OnUpdatedPerformanceTarget -= OnUpdatedPerformance;
        }
    }
}