using Ibit.Plataform.Camera;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Spawn;
using Ibit.Plataform.UI;
using UnityEngine;
using Ibit.Core.Database;

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
            Score = (Mathf.Abs(this.transform.position.y) * (1f + _model.DifficultyFactor) * (1 + StageModel.Loaded.ObjectSpeedFactor))*ParametersDb.parameters.ScoreCalculationFactor; // ScoreCalculationFactor = Fator de Cálculo da Pontuação
        }

        private void CalculateHeight(float performanceAccumulator = 0)
        {
            var tmpPos = this.transform.position;

            tmpPos.y = (1f + performanceAccumulator) * CameraLimits.Boundary * _model.DifficultyFactor;
            // Debug.Log($"Alvos - Altura antes: {tmpPos.y}");

            tmpPos.y = _model.PositionYFactor > 0 ? // Normal
                Mathf.Clamp(tmpPos.y, 0f, CameraLimits.Boundary) :
                Mathf.Clamp(-tmpPos.y, -CameraLimits.Boundary, 0f);

            
            if (ResultScreenUI.numberFailures >= ParametersDb.parameters.lostWtimes)  // Perdeu W vezes
            {
                tmpPos.y = _model.PositionYFactor > 0 ?
                    Mathf.Clamp(tmpPos.y*ParametersDb.parameters.decreaseHeight, 0f, CameraLimits.Boundary) :
                    Mathf.Clamp(tmpPos.y*ParametersDb.parameters.decreaseHeight, -CameraLimits.Boundary, 0f); // decreaseHeight =  Valor de decremento da ALTURA dos Alvos, vindo de _parametersList.csv
                // Debug.Log($"Alvos - Altura depois: {tmpPos.y}");
            }
            
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