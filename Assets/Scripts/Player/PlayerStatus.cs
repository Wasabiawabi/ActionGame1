using System.Collections;
using System.Collections.Generic;
using HandmadeLibrary.DataBase.InitialStatus.Player;
using UnityEngine;
using System.Linq;
using System.Reflection;
using HandmadeLibrary.DataBase.Upgrade;
using HandmadeLibrary.DataBase.Upgrade.Increment;
using HandmadeLibrary.DataBase.Upgrade.Multiply;
using System;

public class PlayerStatus : MonoBehaviour
{
    [Tooltip("プレイヤーのステータスやバフの効果、仲間の人数やその効果を管理する.")]
    public bool summary;

    //スクリプト
    [SerializeField] private DataSearchHandler dataSearchHandler;
    [SerializeField] private UpgradesLevelHandler upgradesLevelHandler;
    [SerializeField] private PlayerBuffHandler playerBuffHandler;
    //データベースへの接続用
    private int playerId = 0;
    private List<string> playerUpgrades = new List<string>();

    //プレイヤーのステータス(ステータスとそれにかかるアップグレードのIDは一致している)
    public List<string> playerStatusName = new List<string> { "maxSpeed", "speedDampingPerSecond", "maxInitialMovementSpeed", "moveVerticalSpeed" };
    public List<float> playerStatusValue = new List<float> { 0f, 0f, 0f, 0f };

    private int incrementMaxSpeedLevel;
    private int multiplySpeedDampingLevel;
    private int incrementMaxInitialSpeedLevel;


    private void Start()
    {
        GetPlayerInitialStatus();
        GetPlayerUpgradeName();
        //UpgradePlayerStatus(); // データがロードされた後に呼び出す
        //PrintPlayerStatus(); // デバッグ
    }

    public void GetPlayerUpgradeName()
    {
        // アップグレードデータを全部調べて、プレイヤーに関するアップグレードであれば名前を保存しておく
        foreach (var upgrade in dataSearchHandler.upgradeDataStore.DataBase.DataList)
        {
            if(upgrade.TargetName == BaseOfUpgradeData.TargetType.Player)
            {
                playerUpgrades.Add(upgrade.Name);
            }
        }
    }

    private void GetPlayerInitialStatus()//プレイヤーの初期ステータスを取得
    {
        var playerInitialStatusData = dataSearchHandler.playerStatusDataStore.GetDataByID(playerId);
        playerStatusValue[0] = playerInitialStatusData.MaxSpeed;
        playerStatusValue[1] = playerInitialStatusData.SpeedDampingPerSecond;
        playerStatusValue[2] = playerInitialStatusData.MaxInitialMovementSpeed;
        playerStatusValue[3] = playerInitialStatusData.VerticalMovementSpeed;

        //foreach(var i in playerStatusValue)
          //  Debug.Log(i);
    }

    public void UpgradePlayerStatus()//プレイヤーのステータスのアップグレードの情報から、ステータスを変更する
    {
        // レベルごとの強化量(配列)を取得
        IncrementUpgradeData maxSpeedData = dataSearchHandler.upgradeDataStore.GetDataByID(0) as IncrementUpgradeData;
        MultiplyUpgradeData speedDampingData = dataSearchHandler.upgradeDataStore.GetDataByID(1) as MultiplyUpgradeData;
        IncrementUpgradeData maxInitialSpeedData = dataSearchHandler.upgradeDataStore.GetDataByID(2) as IncrementUpgradeData;

        // 各アップグレードの現在のレベルを取得
        incrementMaxSpeedLevel = upgradesLevelHandler.playerUpgradeData.increaseMaxSpeedLevel;
        multiplySpeedDampingLevel = upgradesLevelHandler.playerUpgradeData.decreaceSpeedDumpingLevel;
        incrementMaxInitialSpeedLevel = upgradesLevelHandler.playerUpgradeData.increaceMaxInitialMovementSpeedLevel;

        // アップグレードを適用
        playerStatusValue[0] += incrementMaxSpeedLevel >= 1 ? maxSpeedData.IncrementAmountList[incrementMaxSpeedLevel - 1] : 0;
        playerStatusValue[1] *= MathF.Pow(speedDampingData.MultiplyRate, multiplySpeedDampingLevel);
        playerStatusValue[2] += incrementMaxInitialSpeedLevel >= 1 ? maxInitialSpeedData.IncrementAmountList[incrementMaxInitialSpeedLevel - 1] : 0;

        PrintPlayerStatus(); // デバッグ
    }

    public Vector3 GetPlayerPos()
    {
        return transform.position;
    }

    public float GetPlayerHeight()
    {
        return transform.position.y;
    }

    public void PrintPlayerStatus() // デバッグ用：現在のステータスをコンソールに出力
    {
        String s = "Current Player Status:\n";
        s = String.Concat(s, $"{playerStatusName[0]}:level{incrementMaxSpeedLevel}:{playerStatusValue[0]}\n");
        s = String.Concat(s, $"{playerStatusName[1]}:level{multiplySpeedDampingLevel}:{playerStatusValue[1]}\n");
        s = String.Concat(s, $"{playerStatusName[2]}:level{incrementMaxInitialSpeedLevel}:{playerStatusValue[2]}\n");
        Debug.Log(s);
    }
}
