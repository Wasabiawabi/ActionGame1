using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// アップグレードボタンに表示されるUI(描画)を管理する.
/// アップグレードのレベル、アップグレードのコストを取得して、対応する画像や文字を描画する
/// </summary>

public class ButtonUIMaker : MonoBehaviour
{
    // アップグレードに関する情報
    public UpgradesLevelHandler upgradesLevelHandler;

    // プレイヤー関連のアップグレードを取得するかどうか
    public bool maxInitialMovementSpeed = false;
    public bool maxSpeed = false;
    public bool speedDumping = false;

    // アイテム関連のアップグレードを取得するかどうか
    public bool buffEffect = false;
    public bool instantiateProbabiltyPerFrame = false;
    public bool multipleBuffProbablity = false;
    public bool maxMultipleBuff = false;

    // 取得したい値
    private int level = 0;
    private int cost = 0;

    // 描画につかうオブジェクト・コンポーネント
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image levelImage;
    [SerializeField] private Sprite[] image;

    private void Start()
    {
        LoadCostAndLevel();
    }

    private void Update()
    {
        LoadCostAndLevel();
        // コストテキスト描画
        if (cost != -1)
        {
            costText.text = cost.ToString();
        }
        else
        {
            costText.text = "MAX";
        }

        // レベルイメージ描画
        if (level != 0)
        {
            levelImage.color = new Color(1, 1, 1, 1);
            levelImage.sprite = image[level - 1];
            levelImage.preserveAspect = true;
        }
        else
        {
            levelImage.color = new Color(1, 1, 1, 0); // レベル0の場合は透明
        }
    }

    private void LoadCostAndLevel()
    {
        // レベルとコストを取得
        if (maxInitialMovementSpeed)
        {
            level = upgradesLevelHandler.playerUpgradeData.increaceMaxInitialMovementSpeedLevel;
            cost = upgradesLevelHandler.playerUpgradeData.increaceMaxInitialMovementSpeedCosts[level];
        }
        else if (maxSpeed)
        {
            level = upgradesLevelHandler.playerUpgradeData.increaseMaxSpeedLevel;
            cost = upgradesLevelHandler.playerUpgradeData.increaseMaxSpeedCosts[level];
        }
        else if (speedDumping)
        {
            level = upgradesLevelHandler.playerUpgradeData.decreaceSpeedDumpingLevel;
            cost = upgradesLevelHandler.playerUpgradeData.decreaceSpeedDumpingCosts[level];
        }
        else if (buffEffect)
        {
            level = upgradesLevelHandler.stageObjectUpgradeData.increaseBuffEffect;
            cost = upgradesLevelHandler.stageObjectUpgradeData.increaseBuffEffectCosts[level];
        }
        else if (instantiateProbabiltyPerFrame)
        {
            level = upgradesLevelHandler.stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel;
            cost = upgradesLevelHandler.stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameCosts[level];
        }
        else if (multipleBuffProbablity)
        {
            level = upgradesLevelHandler.stageObjectUpgradeData.increaseMultipleBuffProbablityLevel;
            cost = upgradesLevelHandler.stageObjectUpgradeData.increaseMultipleBuffProbablityCosts[level];
        }
        else if (maxMultipleBuff)
        {
            level = upgradesLevelHandler.stageObjectUpgradeData.maxMultipleBuffLevel;
            cost = upgradesLevelHandler.stageObjectUpgradeData.maxMultipleBuffCosts[level];
        }
    }
}
