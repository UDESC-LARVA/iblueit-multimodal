using System;
using Ibit.Core.Data;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Spawn;
using UnityEngine;

namespace Ibit.Plataform
{
    public partial class Obstacle : MonoBehaviour
    {
        [SerializeField] private int heartPoint = 1;

        [SerializeField] private ObjectModel _model;

        public int HeartPoint => heartPoint;
        public float Score { get; private set; }

        public void Build(ObjectModel model)
        {
            _model = model;
            CalculateSize();
            CalculateNewDistance();
            CalculateScore();
            FindObjectOfType<Spawner>().OnUpdatedPerformanceObstacle += OnUpdatedPerformance;
        }

        private void OnUpdatedPerformance(float insAcc, float expAcc)
        {
            CalculateSize(this._model.PositionYFactor > 0 ? expAcc : insAcc);
            CalculateNewDistance();
        }

        private void CalculateSize(float performanceFactor = 0)
        {
            var tmpScale = this.transform.localScale;

            tmpScale.x = (this._model.PositionYFactor > 0 ? Pacient.Loaded.CapacitiesPitaco.ExpFlowDuration : Pacient.Loaded.CapacitiesPitaco.InsFlowDuration) / 1000f *
                (1f + performanceFactor) * this._model.DifficultyFactor;

            tmpScale.x = tmpScale.x < 1f ? 1f : tmpScale.x;

            this.transform.localScale = new Vector3(tmpScale.x, tmpScale.x);

            var spriteOffset = this.transform.localScale.y / 2f;

            this.transform.position = new Vector3(this.transform.position.x, this._model.PositionYFactor > 0 ? spriteOffset : -spriteOffset);
        }

        private void CalculateNewDistance()
        {
            // order: previous < this < next
            try
            {
                var previousObject = FindObjectOfType<Spawner>().SpawnedObjects.Find(this.transform).Previous.Value;

                if (previousObject.GetComponent<Target>() == null)
                {
                    this.transform.position = new Vector3(previousObject.position.x + previousObject.localScale.x / 2 + _model.PositionXSpacing + this.transform.localScale.x / 2, this.transform.position.y);
                }
                else
                {
                    this.transform.position = new Vector3(previousObject.position.x + _model.PositionXSpacing, this.transform.position.y);
                }
            }
            catch (NullReferenceException) { } // ignore new distances if the previous object was destroyed
        }

        private void CalculateScore()
        {
            Score = 2f * this.transform.localScale.x * (1f + this._model.DifficultyFactor) * (1 + StageModel.Loaded.ObjectSpeedFactor);
        }

        private void OnDestroy()
        {
            var spwn = FindObjectOfType<Spawner>();
            if (spwn != null)
                spwn.OnUpdatedPerformanceObstacle -= OnUpdatedPerformance;
        }
    }
}