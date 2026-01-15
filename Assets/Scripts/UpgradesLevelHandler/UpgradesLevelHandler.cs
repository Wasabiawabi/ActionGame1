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
}
