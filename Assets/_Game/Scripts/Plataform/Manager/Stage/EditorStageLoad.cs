#if UNITY_EDITOR

using Ibit.Core.Database;
using Ibit.Plataform.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Plataform.Manager.Stage
{
    public partial class StageManager
    {
        [SerializeField] private string filename;
        [SerializeField] private StageModel testStage;

        [Button("Load Test Stage File")]
        public void EditorStageLoad()
        {
            if (string.IsNullOrEmpty(filename))
            {
                Debug.LogWarning("Can't load stage. You must specify a filename!");
                return;
            }

            testStage = StageDb.LoadStageFromFile(filename);

            StageModel.Loaded = testStage;

            Debug.Log($"Stage {testStage.Id} loaded!");
        }
    }
}

#endif