using System.IO;
using UnityEngine;
using HandmadeLibrary.Json;

[System.Serializable]
public class PlayerUpgradeData
{
    // プレイヤーのアップグレードレベル
    [HideInInspector] public int increaseMaxSpeedLevel;
    [HideInInspector] public int increaceMaxInitialMovementSpeedLevel;
    [HideInInspector] public int decreaceSpeedDumpingLevel;
    
    // アップグレードコスト（レベルごと）
    public int[] increaseMaxSpeedCosts = { 100, 200, 400, 800, 1600 };
    public int[] increaceMaxInitialMovementSpeedCosts = { 150, 300, 600, 1200, 2400 };
    public int[] decreaceSpeedDumpingCosts = { 120, 240, 480, 960, 1920 };
}

[System.Serializable]
public class StageObjectUpgradeData
{
    // ステージオブジェクトのアップグレードレベル
    [HideInInspector] public int increaseInstantiateProbabiltyPerFrameLevel;
    [HideInInspector] public int increaseMultipleBuffProbablityLevel;
    [HideInInspector] public int maxMultipleBuffLevel;
    [HideInInspector] public int increaseBuffEffect;
    
    // アップグレードコスト（レベルごと）
    public int[] increaseInstantiateProbabiltyPerFrameCosts = { 200, 400, 800, 1600, 3200 };
    public int[] increaseMultipleBuffProbablityCosts = { 250, 500, 1000, 2000, 4000 };
    public int[] maxMultipleBuffCosts = { 300, 600, 1200, 2400, 4800 };
    public int[] increaseBuffEffectCosts = { 180, 360, 720, 1440, 2880 };
}

public class UpgradesLevelHandler : MonoBehaviour
{
    [Tooltip("プレイヤーやステージオブジェクトの現在のレベルをJson形式で管理する.")]
    public bool summary;

    //必要なスクリプト
    public PlayerUpgradeData playerUpgradeData;
    public StageObjectUpgradeData stageObjectUpgradeData;
    private JsonFileHandler jsonFileHandler = new JsonFileHandler();

    [SerializeField] private PlayerStatus playerStatus;

    [SerializeField] private MoneyManager moneyManager;

    //Pathを指定
    string path_playerData = Path.Combine(Application.dataPath, "Data/Json/playerData.json");
    string path_stageObjectData = Path.Combine(Application.dataPath, "Data/Json/stageObjectData.json");

    // 最大レベル
    const int maxLevel = 4;

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

        Debug.Log("Upgrades Data Loaded:");
        playerStatus.UpgradePlayerStatus();
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
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is not initialized.");
            return;
        }
        if (playerUpgradeData == null || playerUpgradeData.increaseMaxSpeedCosts == null)
        {
            Debug.LogError("PlayerUpgradeData or increaseMaxSpeedCosts is not initialized.");
            return;
        }
        if (playerUpgradeData.increaseMaxSpeedLevel >= playerUpgradeData.increaseMaxSpeedCosts.Length)
        {
            Debug.Log("Max Speed upgrade is already at maximum level.");
            return;
        }
        int cost = playerUpgradeData.increaseMaxSpeedCosts[playerUpgradeData.increaseMaxSpeedLevel];
        if (moneyManager.totalMoney < cost)
        {
            Debug.Log("Not enough money to upgrade Max Speed. Required: " + cost + ", Available: " + moneyManager.totalMoney);
            return;
        }
        if (playerUpgradeData.increaseMaxSpeedLevel >= maxLevel)
        {
            Debug.Log("Max Speed is already at maximum level.");
            return;
        }

        moneyManager.totalMoney -= cost;
        playerUpgradeData.increaseMaxSpeedLevel += 1;
        Debug.Log("Max Speed upgraded to level " + playerUpgradeData.increaseMaxSpeedLevel);
    }

    public void UpgradePlayerMaxInitialMovementSpeed()
    {
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is not initialized.");
            return;
        }
        if (playerUpgradeData == null || playerUpgradeData.increaceMaxInitialMovementSpeedCosts == null)
        {
            Debug.LogError("PlayerUpgradeData or increaceMaxInitialMovementSpeedCosts is not initialized.");
            return;
        }
        if (playerUpgradeData.increaceMaxInitialMovementSpeedLevel >= playerUpgradeData.increaceMaxInitialMovementSpeedCosts.Length)
        {
            Debug.Log("Max Initial Movement Speed upgrade is already at maximum level.");
            return;
        }
        int cost = playerUpgradeData.increaceMaxInitialMovementSpeedCosts[playerUpgradeData.increaceMaxInitialMovementSpeedLevel];
        if (moneyManager.totalMoney < cost)
        {
            Debug.Log("Not enough money to upgrade Max Initial Movement Speed. Required: " + cost + ", Available: " + moneyManager.totalMoney);
            return;
        }
        if (playerUpgradeData.increaceMaxInitialMovementSpeedLevel >= maxLevel)
        {
            Debug.Log("Max Initial Movement Speed is already at maximum level.");
            return;
        }

        moneyManager.totalMoney -= cost;
        playerUpgradeData.increaceMaxInitialMovementSpeedLevel += 1;
        Debug.Log("Max Initial Movement Speed upgraded to level " + playerUpgradeData.increaceMaxInitialMovementSpeedLevel);
    }

    public void UpgradePlayerSpeedDumping()
    {
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is not initialized.");
            return;
        }
        if (playerUpgradeData == null || playerUpgradeData.decreaceSpeedDumpingCosts == null)
        {
            Debug.LogError("PlayerUpgradeData or decreaceSpeedDumpingCosts is not initialized.");
            return;
        }
        if (playerUpgradeData.decreaceSpeedDumpingLevel >= playerUpgradeData.decreaceSpeedDumpingCosts.Length)
        {
            Debug.Log("Speed Dumping upgrade is already at maximum level.");
            return;
        }
        int cost = playerUpgradeData.decreaceSpeedDumpingCosts[playerUpgradeData.decreaceSpeedDumpingLevel];
        if (moneyManager.totalMoney < cost)
        {
            Debug.Log("Not enough money to upgrade Speed Dumping. Required: " + cost + ", Available: " + moneyManager.totalMoney);
            return;
        }
        if (playerUpgradeData.decreaceSpeedDumpingLevel >= maxLevel)
        {
            Debug.Log("Speed Dumping is already at maximum level.");
            return;
        }

        moneyManager.totalMoney -= cost;
        playerUpgradeData.decreaceSpeedDumpingLevel += 1;
        Debug.Log("Speed Dumping upgraded to level " + playerUpgradeData.decreaceSpeedDumpingLevel);
    }

    public void UpgradeStageObjectInstantiateProbabilityPerFrame()
    {
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is not initialized.");
            return;
        }
        if (stageObjectUpgradeData == null || stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameCosts == null)
        {
            Debug.LogError("StageObjectUpgradeData or increaseInstantiateProbabiltyPerFrameCosts is not initialized.");
            return;
        }
        if (stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel >= stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameCosts.Length)
        {
            Debug.Log("Instantiate Probability Per Frame upgrade is already at maximum level.");
            return;
        }
        int cost = stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameCosts[stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel];
        if (moneyManager.totalMoney < cost)
        {
            Debug.Log("Not enough money to upgrade Instantiate Probability Per Frame. Required: " + cost + ", Available: " + moneyManager.totalMoney);
            return;
        }
        if (stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel >= maxLevel)
        {
            Debug.Log("Instantiate Probability Per Frame is already at maximum level.");
            return;
        }

        moneyManager.totalMoney -= cost;
        stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel += 1;
        Debug.Log("Instantiate Probability Per Frame upgraded to level " + stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel);
    }

    public void UpgradeStageObjectMultipleBuffProbability()
    {
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is not initialized.");
            return;
        }
        if (stageObjectUpgradeData == null || stageObjectUpgradeData.increaseMultipleBuffProbablityCosts == null)
        {
            Debug.LogError("StageObjectUpgradeData or increaseMultipleBuffProbablityCosts is not initialized.");
            return;
        }
        if (stageObjectUpgradeData.increaseMultipleBuffProbablityLevel >= stageObjectUpgradeData.increaseMultipleBuffProbablityCosts.Length)
        {
            Debug.Log("Multiple Buff Probability upgrade is already at maximum level.");
            return;
        }
        int cost = stageObjectUpgradeData.increaseMultipleBuffProbablityCosts[stageObjectUpgradeData.increaseMultipleBuffProbablityLevel];
        if (moneyManager.totalMoney < cost)
        {
            Debug.Log("Not enough money to upgrade Multiple Buff Probability. Required: " + cost + ", Available: " + moneyManager.totalMoney);
            return;
        }
        if (stageObjectUpgradeData.increaseMultipleBuffProbablityLevel >= maxLevel)
        {
            Debug.Log("Multiple Buff Probability is already at maximum level.");
            return;
        }

        moneyManager.totalMoney -= cost;
        stageObjectUpgradeData.increaseMultipleBuffProbablityLevel += 1;
        Debug.Log("Multiple Buff Probability upgraded to level " + stageObjectUpgradeData.increaseMultipleBuffProbablityLevel);
    }

    public void UpgradeStageObjectMaxMultipleBuff()
    {
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is not initialized.");
            return;
        }
        if (stageObjectUpgradeData == null || stageObjectUpgradeData.maxMultipleBuffCosts == null)
        {
            Debug.LogError("StageObjectUpgradeData or maxMultipleBuffCosts is not initialized.");
            return;
        }
        if (stageObjectUpgradeData.maxMultipleBuffLevel >= stageObjectUpgradeData.maxMultipleBuffCosts.Length)
        {
            Debug.Log("Max Multiple Buff upgrade is already at maximum level.");
            return;
        }
        int cost = stageObjectUpgradeData.maxMultipleBuffCosts[stageObjectUpgradeData.maxMultipleBuffLevel];
        if (moneyManager.totalMoney < cost)
        {
            Debug.Log("Not enough money to upgrade Max Multiple Buff. Required: " + cost + ", Available: " + moneyManager.totalMoney);
            return;
        }
        if (stageObjectUpgradeData.maxMultipleBuffLevel >= maxLevel)
        {
            Debug.Log("Max Multiple Buff is already at maximum level.");
            return;
        }

        moneyManager.totalMoney -= cost;
        stageObjectUpgradeData.maxMultipleBuffLevel += 1;
        Debug.Log("Max Multiple Buff upgraded to level " + stageObjectUpgradeData.maxMultipleBuffLevel);
    }

    public void UpgradeStageObjectBuffEffect()
    {
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is not initialized.");
            return;
        }
        if (stageObjectUpgradeData == null || stageObjectUpgradeData.increaseBuffEffectCosts == null)
        {
            Debug.LogError("StageObjectUpgradeData or increaseBuffEffectCosts is not initialized.");
            return;
        }
        if (stageObjectUpgradeData.increaseBuffEffect >= stageObjectUpgradeData.increaseBuffEffectCosts.Length)
        {
            Debug.Log("Buff Effect upgrade is already at maximum level.");
            return;
        }
        int cost = stageObjectUpgradeData.increaseBuffEffectCosts[stageObjectUpgradeData.increaseBuffEffect];
        if (moneyManager.totalMoney < cost)
        {
            Debug.Log("Not enough money to upgrade Buff Effect. Required: " + cost + ", Available: " + moneyManager.totalMoney);
            return;
        }
        if (stageObjectUpgradeData.increaseBuffEffect >= maxLevel)
        {
            Debug.Log("Buff Effect is already at maximum level.");
            return;
        }

        moneyManager.totalMoney -= cost;
        stageObjectUpgradeData.increaseBuffEffect += 1;
        Debug.Log("Buff Effect upgraded to level " + stageObjectUpgradeData.increaseBuffEffect);
    }
}
