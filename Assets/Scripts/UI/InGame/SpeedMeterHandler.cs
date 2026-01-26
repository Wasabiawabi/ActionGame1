using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SpeedMeterHandler : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの移動速度を表示するUIを管理する.
    /// プレイヤーの速度をテキストに反映させる.
    /// </summary>
    // スクリプト
    [SerializeField] private PlayerMovementHandler playerMovementHandler;

    // テキスト
    [SerializeField] private TextMeshProUGUI speedText;

    private void Update()
    {
        speedText.text = MathF.Floor(playerMovementHandler.playerSpeed).ToString() + " m/s";
    }
}
