using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{

    [SerializeField] private UpgradesLevelHandler upgradesLevelHandler;
    [SerializeField] private MoneyManager moneyManager;
    
    public void SaveUpgradesData()
    {
        Debug.Log("saved");
        upgradesLevelHandler.SaveUpgradesDataFile();
    }

    public void SaveMoneyData()
    {
        upgradesLevelHandler.SaveUpgradesDataFile();
    }

    public void SaveAll()
    {
        SaveUpgradesData();
        SaveMoneyData();
    }
}
