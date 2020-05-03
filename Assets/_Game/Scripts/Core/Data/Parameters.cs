using System;
using Ibit.Core.Util;

namespace Ibit.Core.Data
{
    [Serializable]
    public class Parameters
    {
        public static Parameters Loaded;

        public string device;
        public int lostWtimes;
        public float additionalHeight;
        public float additionalSize;
        public int LostXtimes;
        public float AdditionalDistance;
        public bool CalibrateStartBelt;
        public string FusionType;
        public float FusionPrefPitaco;
        public float FusionPrefMano;
        public float FusionPrefCinta;
        public float ScoreCalculationFactor;
        public float MinimumExtensionBelt;
        public int MinimumNormalOxygenation;
        public int MinimumRegularOxygenation;



#if UNITY_EDITOR
        static Parameters()
        {
            if (Loaded == null)
                Loaded = new Parameters
                {
                    device = "P", // Dispositivos
                    lostWtimes = 2, // Perdeu W vezes (Alt. e Tam.)
                    additionalHeight = 0, // Altura adicional dos Alvos
                    additionalSize = 0, // Tamanho adicional dos Obstáculos
                    LostXtimes = 3, // Perdeu X vezes (Recalibrar disp.)
                    AdditionalDistance = 0.3f, // Distância adicional entre Objetos
                    CalibrateStartBelt = false, // Calibrar Cinta no início?
                    FusionType = "", // Fusão de Sinais
                    FusionPrefPitaco = 0, // PrefPitaco
                    FusionPrefMano = 0, // PrefMano
                    FusionPrefCinta = 0, // PrefCinta
                    ScoreCalculationFactor = 0, // Fator de Cálculo da Pontuação
                    MinimumExtensionBelt = 0, // Valor mínimo exigido da Cinta Extensora
                    MinimumNormalOxygenation = 0, // Oxigenação Normal Mínima
                    MinimumRegularOxygenation = 0 // Oxigenação Regular Mínima
                };
        }
#endif

    }

    // public enum DevicesCombination
    // {
    //     P = 1,
    //     M = 2,
    //     C = 3,
    //     PC = 4,
    //     MC = 5,
    //     PO = 6,
    //     MO = 7,
    //     CO = 8,
    //     PCO = 9,
    //     MCO = 10
    // }

    // public enum FusionType
    // {
    //     Sum = 1,
    //     Subtraction = 2,
    //     Preferential = 3
    // }

}