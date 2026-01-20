using System.IO;
using UnityEngine;
using HandmadeLibrary.Json;

[System.Serializable]
public class PlayerUpgradeData
{
    // プレイヤーのアップグレードレベル
    public int increaseMaxSpeedLevel;
    public int increaceMaxInitialMovementSpeedLevel;
    public int decreaceSpeedDumpingLevel;
    
}

[System.Serializable]
public class StageObjectUpgradeData
{
    // ステージオブジェクトのアップグレードレベル
    public int increaseInstantiateProbabiltyPerFrameLevel;
    public int increaseMultipleBuffProbablityLevel;
    public int maxMultipleBuffLevel;
    public int increaseBuffEffect;
}

public class UpgradesLevelHandler : MonoBehaviour
{
    [Tooltip("プレイヤーやステージオブジェクトの現在のレベルをJson形式で管理する.")]
    public bool summary;

    //必要なスクリプト
    public PlayerUpgradeData playerUpgradeData;
    public StageObjectUpgradeData stageObjectUpgradeData;
    private JsonFileHandler jsonFileHandler = new JsonFileHandler();

    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private DataSearchHandler dataSearchHandler;

    //Pathを指定
    string path_playerData = Path.Combine(Application.dataPath, "Data/Json/playerData.json");
    string path_stageObjectData = Path.Combine(Application.dataPath, "Data/Json/stageObjectData.json");

    private void Start()
    { 
        ReadUpgradesDataFile();
        SaveUpgradesDataFile();
    }

    private void ReadUpgradesDataFile()
    {
        //データを読み込む
        playerUpgradeData = jsonFileHandler.ReadFromJson<PlayerUpgradeData>(path_playerData, playerUpgradeData);
        stageObjectUpgradeData = jsonFileHandler.ReadFromJson<StageObjectUpgradeData>(path_stageObjectData, stageObjectUpgradeData);
    }

    public void SaveUpgradesDataFile()
    {
        //データを保存する
        jsonFileHandler.SaveToJson<PlayerUpgradeData>(path_playerData, playerUpgradeData);
        jsonFileHandler.SaveToJson<StageObjectUpgradeData>(path_stageObjectData, stageObjectUpgradeData);

        /*
        //保存されたデータを確認する
        Debug.Log("Player Upgrade Data Saved:");
        Debug.Log(JsonUtility.ToJson(playerUpgradeData, true));
        Debug.Log("Stage Object Upgrade Data Saved:");
        Debug.Log(JsonUtility.ToJson(stageObjectUpgradeData, true));
        */
    }

    public void UpgradePlayerMaxSpeed()
    {
        if(moneyManager.totalMoney < dataSearchHandler.upgradeDataStore.GetDataByName("IncreaseMaxSpeed").UpgradeCostList[playerUpgradeData.increaseMaxSpeedLevel])
        {
            Debug.Log("Not enough money to upgrade Max Speed.");
            return;
        }
        playerUpgradeData.increaseMaxSpeedLevel += 1;
    }

    public void UpgradePlayerMaxInitialMovementSpeed()
    {
        if (moneyManager.totalMoney < dataSearchHandler.upgradeDataStore.GetDataByName("IncreaseMaxInitialMovementSpeed").UpgradeCostList[playerUpgradeData.increaceMaxInitialMovementSpeedLevel])
        {
            Debug.Log("Not enough money to upgrade Max Initial Movement Speed.");
            return;
        }
        playerUpgradeData.increaceMaxInitialMovementSpeedLevel += 1;
    }

    public void UpgradePlayerSpeedDumping()
    {
        if (moneyManager.totalMoney < dataSearchHandler.upgradeDataStore.GetDataByName("DecreaceSpeedDumping").UpgradeCostList[playerUpgradeData.decreaceSpeedDumpingLevel])
        {
            Debug.Log("Not enough money to upgrade Speed Dumping.");
            return;
        }
        playerUpgradeData.decreaceSpeedDumpingLevel += 1;
    }

    public void UpgradeStageObjectInstantiateProbabilityPerFrame()
    {
        if (moneyManager.totalMoney < dataSearchHandler.upgradeDataStore.GetDataByName("IncreaseInstantiateProbabilityPerFrame").UpgradeCostList[stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel])
        {
            Debug.Log("Not enough money to upgrade Instantiate Probability Per Frame.");
            return;
        }
        stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel += 1;
    }

    public void UpgradeStageObjectMultipleBuffProbability()
    {
        if (moneyManager.totalMoney < dataSearchHandler.upgradeDataStore.GetDataByName("IncreaseMultipleBuffProbability").UpgradeCostList[stageObjectUpgradeData.increaseMultipleBuffProbablityLevel])
        {
            Debug.Log("Not enough money to upgrade Multiple Buff Probability.");
            return;
        }
        stageObjectUpgradeData.increaseMultipleBuffProbablityLevel += 1;
    }

    public void UpgradeStageObjectMaxMultipleBuff()
    {
        if (moneyManager.totalMoney < dataSearchHandler.upgradeDataStore.GetDataByName("MaxMultipleBuff").UpgradeCostList[stageObjectUpgradeData.maxMultipleBuffLevel])
        {
            Debug.Log("Not enough money to upgrade Max Multiple Buff.");
            return;
        }
        stageObjectUpgradeData.maxMultipleBuffLevel += 1;
    }

    public void UpgradeStageObjectBuffEffect()
    {
        if (moneyManager.totalMoney < dataSearchHandler.upgradeDataStore.GetDataByName("IncreaseBuffEffect").UpgradeCostList[stageObjectUpgradeData.increaseBuffEffect])
        {
            Debug.Log("Not enough money to upgrade Buff Effect.");
            return;
        }
        stageObjectUpgradeData.increaseBuffEffect += 1;
    }
}
