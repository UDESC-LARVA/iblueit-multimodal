using Ibit.Core.Data;
using Ibit.Core.Util;
using System;
using Assets._Game.Scripts.Core.Api.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Assets._Game.Scripts.Core.Api.Dto;
using Ibit.Core.Data.Manager;

namespace Ibit.MainMenu.UI.Canvas
{
    public partial class CanvasManager
    {
        public async void CreatePacient()
        {
            var bDay = GameObject.Find("LabelBDay").GetComponent<Text>().text;
            var bMonth = GameObject.Find("LabelBMonth").GetComponent<Text>().text;
            var bYear = GameObject.Find("LabelBYear").GetComponent<Text>().text;

            DateTime birthday;
            try
            {
                birthday = new DateTime(int.Parse(bYear), int.Parse(bMonth), int.Parse(bDay));
            }
            catch (ArgumentOutOfRangeException)
            {
                SysMessage.Warning("Data invalida!");
                return;
            }

            var playerName = GameObject.Find("InputFieldName").GetComponent<InputField>().text;

            if (playerName.Length == 0)
            {
                SysMessage.Warning("Nome de jogador indefinido!");
                return;
            }

            var healthy = GameObject.Find("ToggleHealthy").GetComponent<Toggle>().isOn;
            var obstructive = GameObject.Find("ToggleObstructive").GetComponent<Toggle>().isOn;
            var restrictive = GameObject.Find("ToggleRestrictive").GetComponent<Toggle>().isOn;

            if (healthy == false && obstructive == false && restrictive == false)
            {
                SysMessage.Warning("Condição Indefinida!");
                return;
            }

            var disfunction = restrictive ? ConditionType.Restrictive
                : (obstructive ? ConditionType.Obstructive : ConditionType.Healthy);

            var male = GameObject.Find("ToggleMale").GetComponent<Toggle>().isOn;
            var female = GameObject.Find("ToggleFemale").GetComponent<Toggle>().isOn;

            if (male == false && female == false)
            {
                SysMessage.Warning("Sexo não selecionado.");
                return;
            }

            var observations = GameObject.Find("Observations").GetComponent<InputField>().text;

            var ethnicity = GameObject.Find("EthnicityLabel").GetComponent<Text>().text;

            float weight;
            try
            {
                weight = Parsers.Float(GameObject.Find("WeightText").GetComponent<Text>().text);

                if (weight < 10)
                    throw new Exception();
            }
            catch (Exception)
            {
                SysMessage.Warning("Peso inválido");
                return;
            }

            float height;
            try
            {
                height = Parsers.Float(GameObject.Find("HeightText").GetComponent<Text>().text);

                if (height < 70f || height > 250f)
                    throw new Exception();
            }
            catch (Exception)
            {
                SysMessage.Warning("Altura inválida");
                return;
            }

            float threshold;
            try
            {
                threshold = Parsers.Float(GameObject.Find("ThresholdText").GetComponent<Text>().text);

                if (threshold < 0)
                    throw new Exception();
            }
            catch (Exception)
            {
                SysMessage.Warning("Threshold inválido");
                return;
            }


            //TODO: removido a criação do jogador para ser guardado via CSV. Foi criado um DTO para que sejam feitas maniupulações nos envios/responstas da API.
            //Caso seja necessário, criar um objeto do tipo Pacient e continuar o processamento como segue.

            var plr = new PacientSendDto
            {
                Name = playerName,
                Birthday = birthday,
                Condition = disfunction.GetDescription(),
                Sex = male ? Sex.Male.GetDescription() : Sex.Female.GetDescription(),
                Observations = observations,
                CapacitiesPitaco = new CapacitiesDto(),
                CapacitiesMano = new CapacitiesDto(),
                CapacitiesCinta = new CapacitiesDto(),
                UnlockedLevels = 1,
                Ethnicity = ethnicity,
                Height = height,
                PitacoThreshold = threshold,
                ManoThreshold = threshold,
                CintaThreshold = threshold,
                Weight = weight
            };

            //TODO: verificação não é mais necessária para a API
            //var tmpPlr = PacientDb.Instance.GetPacient(playerName);

            //if (plr.Name.Equals(tmpPlr?.Name)
            //    && plr.Birthday.Equals(tmpPlr?.Birthday)
            //    && plr.Condition.Equals(tmpPlr?.Condition))
            //{
            //    SysMessage.Warning("Jogador existente!");
            //    return;
            //}

            GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);

            var apiResponse = await DataManager.Instance.SavePacient(plr);

            GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);

            if (apiResponse.ApiResponse != null)
            {
                var playerInstanceModel = Pacient.MapFromDto(apiResponse.ApiResponse.Data);
                Pacient.Loaded = playerInstanceModel;
                SysMessage.Info("Jogador criado com sucesso!");
            }
            else
            {
                SysMessage.Info("Erro ao salvar na nuvem!\n Os dados poderão ser enviados posteriormente.");
                Pacient.Loaded = new Pacient
                {
                    IdApi = $"n_{Guid.NewGuid()}",
                    Name = playerName,
                    Birthday = birthday,
                    Condition = disfunction,
                    Sex = male ? Sex.Male : Sex.Female,
                    Observations = observations,
                    CapacitiesPitaco = new Capacities(),
                    CapacitiesMano = new Capacities(),
                    CapacitiesCinta = new Capacities(),
                    UnlockedLevels = 1,
                    Ethnicity = ethnicity,
                    Height = height,
                    PitacoThreshold = threshold,
                    ManoThreshold = threshold,
                    CintaThreshold = threshold,
                    Weight = weight,
                };
            }

            GameObject.Find("Canvas").transform.Find("New Menu").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("Player Menu").gameObject.SetActive(true);
            //GameObject.Find("Canvas").transform.Find("Parameters Menu").gameObject.SetActive(true);

        }
    }
}