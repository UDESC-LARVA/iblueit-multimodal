using System;
using Ibit.Plataform.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Plataform.Manager.Spawn
{
    public partial class Spawner
    {
        #region Performance

        // Targets

        public int TargetsSucceeded => TargetsExpSucceeded + TargetsInsSucceeded;
        public int TargetsFailed => TargetsExpFailed + TargetsInsFailed;

        public int TargetsInsSucceeded { get; private set; }
        public int TargetsInsFailed { get; private set; }
        public int TargetsExpSucceeded { get; private set; }
        public int TargetsExpFailed { get; private set; }

        // Obstacles

        public int ObstaclesSucceeded => ObstaclesExpSucceeded + ObstaclesInsSucceeded;
        public int ObstaclesFailed => ObstaclesExpFailed + ObstaclesInsFailed;

        public int ObstaclesInsSucceeded { get; private set; }
        public int ObstaclesInsFailed { get; private set; }
        public int ObstaclesExpSucceeded { get; private set; }
        public int ObstaclesExpFailed { get; private set; }

        #endregion Performance

        [ShowNonSerializedField] private float expHeightAcc;
        [ShowNonSerializedField] private float expSizeAcc;
        [ShowNonSerializedField] private float insHeightAcc;
        [ShowNonSerializedField] private float insSizeAcc;

        private int airTargetsHit;
        private int airObstaclesHit;
        private int waterTargetsHit;
        private int waterObstaclesHit;

        public event Action<float, float> OnUpdatedPerformanceTarget;
        public event Action<float, float> OnUpdatedPerformanceObstacle;

        private void PerformanceOnPlayerHit(GameObject hit)
        {
            switch (hit.tag)
            {
                case "AirTarget":
                    airTargetsHit++;
                    TargetsInsSucceeded++;
                    if (airTargetsHit >= StageModel.Loaded.HeightUpThreshold)
                    {
                        IncrementInsHeight();
                        airTargetsHit = 0;
                    }
                    break;

                case "WaterTarget":
                    waterTargetsHit++;
                    TargetsExpSucceeded++;
                    if (waterTargetsHit >= StageModel.Loaded.HeightUpThreshold)
                    {
                        IncrementExpHeight();
                        waterTargetsHit = 0;
                    }
                    break;

                case "AirObstacle":
                    airObstaclesHit--;
                    ObstaclesExpFailed++;
                    if (airObstaclesHit <= -StageModel.Loaded.SizeDownThreshold)
                    {
                        DecrementExpSize();
                        airObstaclesHit = 0;
                    }
                    break;

                case "WaterObstacle":
                    waterObstaclesHit--;
                    ObstaclesInsFailed++;
                    if (waterObstaclesHit <= -StageModel.Loaded.SizeDownThreshold)
                    {
                        DecrementInsSize();
                        waterObstaclesHit = 0;
                    }
                    break;
            }
        }

        public void PerformanceOnPlayerMiss(string objectTag)
        {
            switch (objectTag)
            {
                case "AirTarget":
                    airTargetsHit--;
                    TargetsInsFailed++;
                    if (airTargetsHit <= -StageModel.Loaded.HeightDownThreshold)
                    {
                        DecrementInsHeight();
                        airTargetsHit = 0;
                    }
                    break;

                case "WaterTarget":
                    waterTargetsHit--;
                    TargetsExpFailed++;
                    if (waterTargetsHit <= -StageModel.Loaded.HeightDownThreshold)
                    {
                        DecrementExpHeight();
                        waterTargetsHit = 0;
                    }
                    break;

                case "AirObstacle":
                    airObstaclesHit++;
                    ObstaclesExpSucceeded++;
                    if (airObstaclesHit >= StageModel.Loaded.SizeUpThreshold)
                    {
                        IncrementExpSize();
                        airObstaclesHit = 0;
                    }
                    break;

                case "WaterObstacle":
                    waterObstaclesHit++;
                    ObstaclesInsSucceeded++;
                    if (waterObstaclesHit >= StageModel.Loaded.SizeUpThreshold)
                    {
                        IncrementInsSize();
                        waterObstaclesHit = 0;
                    }
                    break;
            }
        }

        public void IncrementInsHeight()
        {
            if (StageModel.Loaded.HeightIncrement == 0)
                return;

            insHeightAcc += StageModel.Loaded.HeightIncrement;
            OnUpdatedPerformanceTarget?.Invoke(insHeightAcc, expHeightAcc);
        }

        public void DecrementInsHeight()
        {
            if (StageModel.Loaded.HeightIncrement == 0)
                return;

            insHeightAcc -= StageModel.Loaded.HeightIncrement;
            insHeightAcc = insHeightAcc < 0f ? 0f : insHeightAcc;
            OnUpdatedPerformanceTarget?.Invoke(insHeightAcc, expHeightAcc);
        }

        public void IncrementExpHeight()
        {
            if (StageModel.Loaded.HeightIncrement == 0)
                return;

            expHeightAcc += StageModel.Loaded.HeightIncrement;
            OnUpdatedPerformanceTarget?.Invoke(insHeightAcc, expHeightAcc);
        }

        public void DecrementExpHeight()
        {
            if (StageModel.Loaded.HeightIncrement == 0)
                return;

            expHeightAcc -= StageModel.Loaded.HeightIncrement;
            expHeightAcc = expHeightAcc < 0f ? 0f : expHeightAcc;
            OnUpdatedPerformanceTarget?.Invoke(insHeightAcc, expHeightAcc);
        }

        public void IncrementInsSize()
        {
            if (StageModel.Loaded.SizeIncrement == 0)
                return;

            insSizeAcc += StageModel.Loaded.SizeIncrement;
            OnUpdatedPerformanceObstacle?.Invoke(insSizeAcc, expSizeAcc);
        }

        public void DecrementInsSize()
        {
            if (StageModel.Loaded.SizeIncrement == 0)
                return;

            insSizeAcc -= StageModel.Loaded.SizeIncrement;
            insSizeAcc = insSizeAcc < 0f ? 0f : insSizeAcc;
            OnUpdatedPerformanceObstacle?.Invoke(insSizeAcc, expSizeAcc);
        }

        public void IncrementExpSize()
        {
            if (StageModel.Loaded.SizeIncrement == 0)
                return;

            expSizeAcc += StageModel.Loaded.SizeIncrement;
            OnUpdatedPerformanceObstacle?.Invoke(insSizeAcc, expSizeAcc);
        }

        public void DecrementExpSize()
        {
            if (StageModel.Loaded.SizeIncrement == 0)
                return;

            expSizeAcc -= StageModel.Loaded.SizeIncrement;
            expSizeAcc = expSizeAcc < 0f ? 0f : expSizeAcc;
            OnUpdatedPerformanceObstacle?.Invoke(insSizeAcc, expSizeAcc);
        }
    }
}