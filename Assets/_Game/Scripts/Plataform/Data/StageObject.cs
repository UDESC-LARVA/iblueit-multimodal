using System;

namespace Ibit.Plataform.Data
{
    [Serializable]
    public class ObjectModel
    {
        public int Id;
        public StageObjectType Type;
        public float DifficultyFactor;
        public float PositionYFactor;
        public float PositionXSpacing;
    }
}
