using Ibit.Core.Database;
using Ibit.Core.Data;
//using Ibit.Core.Serial;
//using Ibit.Core.Util;
using Ibit.Core.Game;
using Ibit.Plataform.Camera;
using UnityEngine;

namespace Ibit.Core.Game
{
    public partial class MixerManager
    {
        private float gameMultiplier = GameManager.CapacityMultiplierPlataform;
        public Vector3 from;
        public Vector3 to;

        // TODO: TESTAR -- FUSÃO SUBTRAÇÃO
        // TODO: TESTAR -- FUSÃO PREFERENCIAL
        // TODO: TESTAR -- FUSÃO FUNÇÃO
        // TODO: AJUSTAR OXÍMETRO

        void FusionManager(float signalPitaco, float signalMano, float signalCinta, float signalOxiSPO, float signalOxiHR)
        {
            var signal = 0f;
            // var signalIns = 0f;
            // var signalExp = 0f;

            var peakPitaco = 0f;
            var peakMano = 0f;
            var peakCinta = 0f;
            var nextPosition = 0f;

            var Player = GameObject.Find("Player");

            // * ------------- NENHUMA FUSÃO (ÚNICO SINAL DE CONTROLE) -------------
            if (ParametersDb.parameters.FusionType.ToLower() == "n")
            {
                switch (ParametersDb.parameters.device)
                {
                    case "P":
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "M":
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "C":
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "PO": //? OXÍMETRO ignorado...
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "MO": //? OXÍMETRO ignorado...
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "CO": //? OXÍMETRO ignorado...
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                }
            }
            
            // * -------------------- SOMA DE SINAIS --------------------
            if (ParametersDb.parameters.FusionType.ToLower() == "soma")
            {
                switch (ParametersDb.parameters.device)
                {
                    case "P":
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "M":
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "C":
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "PC":
                        signal = signalPitaco + signalCinta;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / (peakPitaco + peakCinta); // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "MC":
                        signal = signalMano + signalCinta;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / (peakMano + peakCinta); // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "PO": //? OXÍMETRO ignorado...
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "MO": //? OXÍMETRO ignorado...
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "CO": //? OXÍMETRO ignorado...
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "PCO": //? OXÍMETRO ignorado...
                        signal = signalPitaco + signalCinta;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / (peakPitaco + peakCinta); // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                    case "MCO": //? OXÍMETRO ignorado...
                        signal = signalMano + signalCinta;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / (peakMano + peakCinta); // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                }
            }

            // * -------------------- SUBTRAÇÃO DE SINAIS --------------------
            if (ParametersDb.parameters.FusionType.ToLower() == "subtracao" || ParametersDb.parameters.FusionType.ToLower() == "subtração")
            {
                switch (ParametersDb.parameters.device)
                {
                    case "P":
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "M":
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "C":
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "PC":
                        if (ParametersDb.parameters.FusionSubDevice == "C")
                        {
                            signal = signalPitaco - signalCinta;

                            peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
                            peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                            nextPosition = signal * CameraLimits.Boundary / (peakPitaco - peakCinta); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            from = Player.transform.position;
                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        } else {
                            signal = signalCinta - signalPitaco;

                            peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
                            peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                            nextPosition = signal * CameraLimits.Boundary / (peakCinta - peakPitaco); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            from = Player.transform.position;
                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        }
                    break;

                    case "MC":
                        if (ParametersDb.parameters.FusionSubDevice == "C")
                        {
                            signal = signalMano - signalCinta;

                            peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                            peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                            nextPosition = signal * CameraLimits.Boundary / (peakMano - peakCinta); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            from = Player.transform.position;
                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        } else {
                            signal = signalCinta - signalMano;

                            peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                            peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                            nextPosition = signal * CameraLimits.Boundary / (peakCinta - peakMano); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            from = Player.transform.position;
                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        }
                    break;

                    case "PO": //? OXÍMETRO ignorado...
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "MO": //? OXÍMETRO ignorado...
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "CO": //? OXÍMETRO ignorado...
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "PCO": //? OXÍMETRO ignorado...
                        if (ParametersDb.parameters.FusionSubDevice == "C")
                        {
                            signal = signalPitaco - signalCinta;

                            peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
                            peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                            nextPosition = signal * CameraLimits.Boundary / (peakPitaco - peakCinta); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            from = Player.transform.position;
                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        } else {
                            signal = signalCinta - signalPitaco;

                            peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
                            peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                            nextPosition = signal * CameraLimits.Boundary / (peakCinta - peakPitaco); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            from = Player.transform.position;
                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        }
                    break;

                    case "MCO": //? OXÍMETRO ignorado...
                        if (ParametersDb.parameters.FusionSubDevice == "C")
                        {
                            signal = signalMano - signalCinta;

                            peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                            peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                            nextPosition = signal * CameraLimits.Boundary / (peakMano - peakCinta); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            from = Player.transform.position;
                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        } else {
                            signal = signalCinta - signalMano;

                            peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                            peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                            nextPosition = signal * CameraLimits.Boundary / (peakCinta - peakMano); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            from = Player.transform.position;
                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        }
                    break;
                }
            }

            // * -------------------- SOMA DE SINAIS COM PREFERÊNCIA --------------------
            if (ParametersDb.parameters.FusionType.ToLower() == "preferencial")
            {
                var PrefPitaco = ParametersDb.parameters.FusionPrefPitaco;
                var PrefMano = ParametersDb.parameters.FusionPrefMano;
                var PrefCinta = ParametersDb.parameters.FusionPrefCinta;

                switch (ParametersDb.parameters.device)
                {
                    case "P":
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "M":
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "C":
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "PC":
                        signal = (signalPitaco * PrefPitaco) + (signalCinta * PrefCinta);

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / ((peakPitaco * PrefPitaco) + (peakCinta * PrefCinta)); // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "MC":
                        signal = (signalMano * PrefMano) + (signalCinta * PrefCinta);

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / ((peakMano * PrefMano) + (peakCinta * PrefCinta)); // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "PO": //? OXÍMETRO ignorado...
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "MO": //? OXÍMETRO ignorado...
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "CO": //? OXÍMETRO ignorado...
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "PCO": //? OXÍMETRO ignorado...
                        signal = (signalPitaco * PrefPitaco) + (signalCinta * PrefCinta);

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / ((peakPitaco * PrefPitaco) + (peakCinta * PrefCinta)); // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "MCO": //? OXÍMETRO ignorado...
                        signal = (signalMano * PrefMano) + (signalCinta * PrefCinta);

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / ((peakMano * PrefMano) + (peakCinta * PrefCinta)); // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;
                }
            }

            // * -------------------- FUNÇÃO DE CADA SINAL --------------------
            if (ParametersDb.parameters.FusionType.ToLower() == "funcao" || ParametersDb.parameters.FusionType.ToLower() == "função")
            {
                var DeviceFunctIns = ParametersDb.parameters.FusionFunctIns;
                var DeviceFunctExp = ParametersDb.parameters.FusionFunctExp;

                switch (ParametersDb.parameters.device)
                {
                    case "P":
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "M":
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "C":
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "PC":
                        if (DeviceFunctIns == "C")
                        {
                            // signalIns = signalCinta;
                            // signalExp = signalPitaco;

                            peakCinta = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier;
                            peakPitaco = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier;

                            from = Player.transform.position;

                            nextPosition = from.y < 0 ? (signalCinta * CameraLimits.Boundary / peakCinta) : (signalPitaco * CameraLimits.Boundary / peakPitaco); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);

                        } else {
                            // signalIns = signalPitaco;
                            // signalExp = signalCinta;

                            peakCinta = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier;
                            peakPitaco = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier;

                            from = Player.transform.position;

                            nextPosition = from.y < 0 ? (signalPitaco * CameraLimits.Boundary / peakPitaco) : (signalCinta * CameraLimits.Boundary / peakCinta); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        }
                    break;

                    case "MC":
                        if (DeviceFunctIns == "C")
                        {
                            // signalIns = signalCinta;
                            // signalExp = signalMano;

                            peakCinta = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier;
                            peakMano = Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier;

                            from = Player.transform.position;

                            nextPosition = from.y < 0 ? (signalCinta * CameraLimits.Boundary / peakCinta) : (signalMano * CameraLimits.Boundary / peakMano); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);

                        } else {
                            // signalIns = signalMano;
                            // signalExp = signalCinta;

                            peakCinta = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier;
                            peakMano = Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier;

                            from = Player.transform.position;

                            nextPosition = from.y < 0 ? (signalMano * CameraLimits.Boundary / peakMano) : (signalCinta * CameraLimits.Boundary / peakCinta); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        }
                    break;

                    case "PO": //? OXÍMETRO ignorado...
                        signal = signalPitaco;

                        peakPitaco = signalPitaco > 0 ? (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakPitaco; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "MO": //? OXÍMETRO ignorado...
                        signal = signalMano;

                        peakMano = signalMano > 0 ? (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakMano; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "CO": //? OXÍMETRO ignorado...
                        signal = signalCinta;

                        peakCinta = signalCinta > 0 ? (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier) : (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);

                        nextPosition = signal * CameraLimits.Boundary / peakCinta; // cálculo da posição do blue
                        nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                        from = Player.transform.position;
                        to = new Vector3(Player.transform.position.x, -nextPosition);

                        Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                    break;

                    case "PCO": //? OXÍMETRO ignorado...
                        if (DeviceFunctIns == "C")
                        {
                            // signalIns = signalCinta;
                            // signalExp = signalPitaco;

                            peakCinta = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier;
                            peakPitaco = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier;

                            from = Player.transform.position;

                            nextPosition = from.y < 0 ? (signalCinta * CameraLimits.Boundary / peakCinta) : (signalPitaco * CameraLimits.Boundary / peakPitaco); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);

                        } else {
                            // signalIns = signalPitaco;
                            // signalExp = signalCinta;

                            peakCinta = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier;
                            peakPitaco = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier;

                            from = Player.transform.position;

                            nextPosition = from.y < 0 ? (signalPitaco * CameraLimits.Boundary / peakPitaco) : (signalCinta * CameraLimits.Boundary / peakCinta); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        }
                    break;

                    case "MCO": //? OXÍMETRO ignorado...
                        if (DeviceFunctIns == "C")
                        {
                            // signalIns = signalCinta;
                            // signalExp = signalMano;

                            peakCinta = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier;
                            peakMano = Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier;

                            from = Player.transform.position;

                            nextPosition = from.y < 0 ? (signalCinta * CameraLimits.Boundary / peakCinta) : (signalMano * CameraLimits.Boundary / peakMano); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);

                        } else {
                            // signalIns = signalMano;
                            // signalExp = signalCinta;

                            peakCinta = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier;
                            peakMano = Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier;

                            from = Player.transform.position;

                            nextPosition = from.y < 0 ? (signalMano * CameraLimits.Boundary / peakMano) : (signalCinta * CameraLimits.Boundary / peakCinta); // cálculo da posição do blue
                            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary); // cálculo da posição do blue

                            to = new Vector3(Player.transform.position.x, -nextPosition);

                            Player.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
                        }
                    break;
                }
            }
        } 
    }
}