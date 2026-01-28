using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Diagnostics;

public class SpeedMeterHandler : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの移動速度を表示するUIを管理する.
    /// プレイヤーの速度をテキストに反映させる.
    /// </summary>
    // スクリプト
    [SerializeField] private PlayerMovementHandler playerMovementHandler;
    [SerializeField] private PlayerBuffHandler playerBuffHandler;
    [SerializeField] private PlayerStatus playerStatus;

    // テキスト
    [SerializeField] private TextMeshProUGUI speedText;

    // UIの画像
    [SerializeField] private Sprite speedMeter;
    [SerializeField] private Sprite cappedSpeedMeter;

    // 画像を適用するコンポーネント
    [SerializeField] private Image speedMeterImage;

    // スライダー
    [SerializeField] private Slider speedSlider;

    private void Update()
    {
        speedText.text = MathF.Floor(playerMovementHandler.playerSpeed).ToString() + " m/s";

        if (playerBuffHandler.maintaining)speedMeterImage.sprite = cappedSpeedMeter;
        else speedMeterImage.sprite = speedMeter;

        speedSlider.value = playerMovementHandler.playerSpeed / playerStatus.playerStatusValue[playerStatus.playerStatusName.IndexOf("maxSpeed")];
    }
}
