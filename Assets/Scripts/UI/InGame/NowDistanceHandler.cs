using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowDistanceHandler : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの移動距離を表示するUIを管理する.
    /// </summary>
    
    // スクリプト
    [SerializeField] private PlayerMovementHandler playerMovementHandler;

    // テキスト
    [SerializeField] private TMPro.TextMeshProUGUI nowDistanceText;

    private void Update()
    {
        nowDistanceText.text = MathF.Floor(Mathf.Abs(playerMovementHandler.playerPos.x + playerMovementHandler.offSet.x)).ToString() + " m"; // 移動距離の計算 + " m";
    }
}
