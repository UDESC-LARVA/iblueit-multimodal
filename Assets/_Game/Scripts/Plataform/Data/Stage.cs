using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ibit.Plataform.Data
{
    [Serializable]
    public class StageModel
    {
        public static StageModel Loaded;

        [Header("Info")]

        public int Id;
        public int Phase;
        public int Level;
        public float ObjectSpeedFactor;
        public int Loops;

        [Space(5)]
        [Header("Target Settings")]

        public float HeightIncrement;
        public float HeightUpThreshold;
        public float HeightDownThreshold;

        [Space(5)]
        [Header("Obstacle Settings")]

        public float SizeIncrement;
        public float SizeUpThreshold;
        public float SizeDownThreshold;

        [Space(5)]
        [Header("Stage Object List")]

        public List<ObjectModel> ObjectModels;

        public StageModel()
        {
            ObjectModels = new List<ObjectModel>();
        }
    }
}