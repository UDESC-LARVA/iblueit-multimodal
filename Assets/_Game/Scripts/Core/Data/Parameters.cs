using System;
using Ibit.Core.Util;

namespace Ibit.Core.Data
{
    [Serializable]
    public class Parameters
    {
        public static Parameters Loaded;

        public string device;
        public string FusionType;
        public string FusionSubDevice;
        public float FusionPrefPitaco;
        public float FusionPrefMano;
        public float FusionPrefCinta;
        public string FusionFunctIns;
        public string FusionFunctExp;

        public int lostWtimes;
        public float decreaseHeight;
        public float decreaseSize;
        public int lostXtimes;
        public float AdditionalDistance;
        public float ObjectsSpeedFactor;
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
                    FusionType = "", // Tipo de Fusão
                    FusionSubDevice = "", // Fusão de Sinais, Dispositivo a ser subtraído
                    FusionPrefPitaco = 0, // PrefPitaco
                    FusionPrefMano = 0, // PrefMano
                    FusionPrefCinta = 0, // PrefCinta
                    FusionFunctIns = "", // Fusão de Funções: Dispositivo responsável por controlar a Inspiração
                    FusionFunctExp = "", // Fusão de Funções: Dispositivo responsável por controlar a Expiração

                    lostWtimes = 1, // Perdeu W vezes (Alt. e Tam.)
                    decreaseHeight = 1, // // Valor de decremento da ALTURA dos Alvos
                    decreaseSize = 1, // Valor de decremento do TAMANHO dos Obstáculos
                    lostXtimes = 2, // Perdeu X vezes (Recalibrar disp.)
                    AdditionalDistance = 1, // Distância adicional entre Objetos
                    ObjectsSpeedFactor = 0, // Fator de cálculo da velocidade de movimento dos objetos de jogo (Alvos e Obstáculos)
                    ScoreCalculationFactor = 0, // Fator de Cálculo da Pontuação
                    MinimumExtensionBelt = 0, // Valor mínimo exigido da Cinta de Pressão
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