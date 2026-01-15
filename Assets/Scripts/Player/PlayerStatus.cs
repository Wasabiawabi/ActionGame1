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
    public List<string> playerStatusName = new List<string>
    { "maxSpeed", "speedDampingPerSecond", "maxInitialMovementSpeed", "verticalMovementSpeed", "maxNakama", "buffDampingWithNakama", "speedDampingReductionWithNakama", "playerHp", "rewardLate" };
    public List<float> playerStatusValue = new List<float> { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };

    private void Start()
    {
        GetPlayerInitialStatus();
        GetPlayerUpgradeName();
        UpgradePlayerStatus();
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
        playerStatusValue[3] = playerInitialStatusData.VerticalMovementSpeed;
        playerStatusValue[4] = playerInitialStatusData.MaxCompanion;
        playerStatusValue[5] = playerInitialStatusData.BuffDampingWithCompanion;
        playerStatusValue[6] = playerInitialStatusData.SpeedDampingReductionWithCompanion;
        playerStatusValue[7] = playerInitialStatusData.PlayerHp;
        playerStatusValue[8] = playerInitialStatusData.RewardLate;
        //foreach(var i in playerStatusValue)
          //  Debug.Log(i);
    }

    private void UpgradePlayerStatus()//プレイヤーのステータスのアップグレードの情報から、ステータスを変更する
    {
        if (dataSearchHandler == null || upgradesLevelHandler == null) return;
        var dbStore = dataSearchHandler.upgradeDataStore;
        if (dbStore == null || dbStore.DataBase == null) return;

        // playerStatusValue の各インデックスとアップグレードIDは一致している想定
        for (int i = 0; i < playerStatusValue.Count; i++)
        {
            // ID==i のアップグレードを取得
            var upgrade = dbStore.GetDataByID(i);
            if (upgrade == null) continue;
            if (upgrade.TargetName != BaseOfUpgradeData.TargetType.Player) continue;

            int level = GetLevelForUpgrade(upgrade);
            if (level <= 0) continue; // レベル0は演算しない

            if (upgrade.upgradeType == BaseOfUpgradeData.UpgradeType.Increment)
            {
                if (upgrade is IncrementUpgradeData inc)
                {
                    // 配列はゼロベース、指定は「レベルを1減らしたものを配列の値」
                    int index = Mathf.Clamp(level - 1, 0, (inc.IncrementAmountList?.Count ?? 0) - 1);
                    if (inc.IncrementAmountList != null && inc.IncrementAmountList.Count > 0 && index >= 0)
                    {
                        float add = inc.IncrementAmountList[index];
                        playerStatusValue[i] += add;
                    }
                }
            }
            else if (upgrade.upgradeType == BaseOfUpgradeData.UpgradeType.Multiply)
            {
                if (upgrade is MultiplyUpgradeData mul)
                {
                    // レベル分だけ倍率をかける（累乗）
                    float rate = mul.MultiplyRate;
                    float factor = Mathf.Pow(rate, level);
                    playerStatusValue[i] *= factor;
                }
            }
        }

        // デバッグ出力：適用後のステータス
        /*
        Debug.Log("Upgraded Player Status Values:");
        for (int i = 0; i < playerStatusValue.Count; i++)
        {
            Debug.Log($"{playerStatusName[i]} = {playerStatusValue[i]}");
        }*/
    }

    // アップグレードごとのレベルを取得する。フィールド名が「{UpgradeName}Level」の形式を優先して探し、
    // 見つからなければ宣言順の整数フィールド配列からIDによる索引で取得するフォールバックを行う。
    private int GetLevelForUpgrade(BaseOfUpgradeData upgrade)
    {
        var pd = upgradesLevelHandler.playerUpgradeData;
        if (pd == null) return 0;

        var t = pd.GetType();
        var intFields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .Where(f => f.FieldType == typeof(int)).ToArray();

        // 1) {Name}Level に一致するフィールドを探す（大文字小文字無視）
        string candidate1 = upgrade.Name + "Level";
        var f = intFields.FirstOrDefault(fi => string.Equals(fi.Name, candidate1, StringComparison.OrdinalIgnoreCase));
        if (f != null)
        {
            try { return (int)f.GetValue(pd); } catch { return 0; }
        }

        // 2) フィールド名にアップグレード名を含む（かつ "Level" を含む）ものを探す
        var f2 = intFields.FirstOrDefault(fi =>
            fi.Name.IndexOf(upgrade.Name, StringComparison.OrdinalIgnoreCase) >= 0 &&
            fi.Name.EndsWith("Level", StringComparison.OrdinalIgnoreCase));
        if (f2 != null)
        {
            try { return (int)f2.GetValue(pd); } catch { return 0; }
        }

        // 3) フォールバック：宣言順（MetadataTokenが宣言順に近い）で ID をインデックスとして使う
        var ordered = intFields.OrderBy(fi => fi.MetadataToken).ToArray();
        int id = upgrade.ID;
        if (id >= 0 && id < ordered.Length)
        {
            try { return (int)ordered[id].GetValue(pd); } catch { return 0; }
        }

        return 0;
    }

    public Vector3 GetPlayerPos()
    {
        return transform.position;
    }

    public float GetPlayerHeight()
    {
        return transform.position.y;
    }
}
