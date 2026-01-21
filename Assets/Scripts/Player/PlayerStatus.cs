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
    [SerializeField] private NakamaHandler nakamaHandler;

    //データベースへの接続用
    private int playerId = 0;
    private List<string> playerUpgrades = new List<string>();

    //プレイヤーのステータス(ステータスとそれにかかるアップグレードのIDは一致している)
    public List<string> playerStatusName = new List<string> { "maxSpeed", "speedDampingPerSecond", "maxInitialMovementSpeed", };
    public List<float> playerStatusValue = new List<float> { 0f, 0f, 0f, };

    private void Start()
    {
        GetPlayerInitialStatus();
        GetPlayerUpgradeName();
        UpgradePlayerStatus();
        PrintPlayerStatus(); // デバッグ
    }

    private void GetPlayerUpgradeName()
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
        //foreach(var i in playerStatusValue)
          //  Debug.Log(i);
    }

    private void UpgradePlayerStatus()//プレイヤーのステータスのアップグレードの情報から、ステータスを変更する
    {
        // レベルごとの強化量(配列)を取得
        IncrementUpgradeData maxSpeedData = dataSearchHandler.upgradeDataStore.GetDataByID(0) as IncrementUpgradeData;
        MultiplyUpgradeData speedDampingData = dataSearchHandler.upgradeDataStore.GetDataByID(1) as MultiplyUpgradeData;
        IncrementUpgradeData maxInitialSpeedData = dataSearchHandler.upgradeDataStore.GetDataByID(2) as IncrementUpgradeData;

        // 各アップグレードの現在のレベルを取得
        int incrementMaxSpeedLevel = upgradesLevelHandler.playerUpgradeData.increaseMaxSpeedLevel;
        int multiplySpeedDampingLevel = upgradesLevelHandler.playerUpgradeData.decreaceSpeedDumpingLevel;
        int incrementMaxInitialSpeedLevel = upgradesLevelHandler.playerUpgradeData.increaceMaxInitialMovementSpeedLevel;

        // アップグレードを適用
        playerStatusValue[0] += incrementMaxSpeedLevel >= 1 ? maxSpeedData.UpgradeCostList[incrementMaxSpeedLevel - 1] : 0;
        playerStatusValue[1] *= multiplySpeedDampingLevel >= 1 ? speedDampingData.UpgradeCostList[multiplySpeedDampingLevel - 1] : 1f;
        playerStatusValue[2] += incrementMaxInitialSpeedLevel >= 1 ? maxInitialSpeedData.UpgradeCostList[incrementMaxInitialSpeedLevel - 1] : 0;
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
        for (int i = 0; i < playerStatusName.Count; i++)
        {
            s = String.Concat(s, $"{playerStatusName[i]}: {playerStatusValue[i]}\n");
        }
        Debug.Log(s);
    }
}
