using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameOverAnimationHandler : MonoBehaviour
{
    [Tooltip("プレイヤーのゲームオーバー判定とアニメーションを管理する.\nゲームオーバー判定は、スタートするまでは行わない.")]
    public bool summary;

    //スクリプト
    [SerializeField] private PlayerMovementHandler playerMovementHandler;
    [SerializeField] private PlayerController playerController;

    //変数
    [SerializeField] private float maxSecondToGameOver = 2f;
    private float secondToGameOver = 2f;
    private bool isgameovered = false;

    private void Update()
    {
        //スタート済みで、移動していなければカウントを行う
        if(playerController.started && playerMovementHandler.playerSpeed == 0)CalcGameOver();

        //ゲームオーバー判定
        if (secondToGameOver <= 0) GameOverAnimation();

        //移動していればリセットし続ける
        if (playerMovementHandler.playerSpeed != 0) secondToGameOver = maxSecondToGameOver;
    }

    private void CalcGameOver()//ゲームオーバーまでの残り時間を計算する
    {
        secondToGameOver -= Time.deltaTime;
    }

    private void GameOverAnimation()//ゲームオーバー時の処理
    {
        Debug.Log("Game Over");
    }
}
