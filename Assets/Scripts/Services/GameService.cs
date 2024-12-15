using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GetGameService
{
    private readonly string dataPath;

    public GetGameService(string dataPath)
    {
        this.dataPath = dataPath;
    }

    public void GetGame(System.Action<BlockDataList> onSuccess, System.Action<ValidationResult> onError)
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            var blockDataList = JsonUtility.FromJson<BlockDataList>(json);

            int rows = blockDataList.blocks.Max(b => b.R);
            int cols = blockDataList.blocks.Max(b => b.C);

            ValidationResult result = Utils.ValidateJSONData(blockDataList.blocks, rows, cols);
            if (result == ValidationResult.SUCCESS)
            {
                onSuccess?.Invoke(blockDataList);
            }else
            {
                onError?.Invoke(result);
            }
        }else
        {
            AlertManager.Instance.ShowAlert(ALERT.ERROR, "Algo anda mal....", "Archivo JSON no encontrado!", new Dictionary<string, UnityAction>());
        }
    }
}
