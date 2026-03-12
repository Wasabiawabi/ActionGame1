using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandmadeLibrary.Json;


[System.Serializable]
public class TotalMoney
{
    [HideInInspector] public float totalMoney;
}

public class MoneyManager : MonoBehaviour
{
    [Tooltip("金を管理する.")]
    bool summary;

    private JsonFileHandler jsonFileHandler = new JsonFileHandler();
    private TotalMoney totalMoneyData = new TotalMoney();

    [HideInInspector]public float totalMoney;
    string path_totalMoney = Path.Combine(Application.dataPath, "Data/Json/totalMoney.json");

    private void Start()
    {
        totalMoneyData = jsonFileHandler.ReadFromJson<TotalMoney>(path_totalMoney, totalMoneyData);
        jsonFileHandler.SaveToJson<TotalMoney>(path_totalMoney, totalMoneyData);
        totalMoney = totalMoneyData.totalMoney;
    }

    public void SaveTotalMoney()
    {
        totalMoneyData.totalMoney = totalMoney;
        Debug.Log("totalMoneyData :" + totalMoneyData + " total :" + totalMoneyData.totalMoney);
        jsonFileHandler.SaveToJson<TotalMoney>(path_totalMoney, totalMoneyData);
    }
}
