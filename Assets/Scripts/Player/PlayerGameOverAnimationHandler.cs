using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerGameOverAnimationHandler : MonoBehaviour
{
    [Tooltip("プレイヤーのゲームオーバー判定とアニメーションを管理する.\nゲームオーバー判定は、スタートするまでは行わない.")]
    public bool summary;

    //スクリプト
    [SerializeField] private PlayerMovementHandler playerMovementHandler;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private GameObject gameoveredSummary;

    //変数
    [SerializeField] private float maxSecondToGameOver = 2f;
    private float secondToGameOver = 2f;
    public bool isgameovered = false;

    float totalMoney = 0f; // 追加: ゲームオーバー時の合計金額
    Vector3 offSet;
    private void Update()
    {
        //スタート済みで、移動していなければカウントを行う
        if(playerController.started && playerMovementHandler.playerSpeed == 0 && !isgameovered)CalcGameOver();

        //移動していればリセットし続ける
        if (playerMovementHandler.playerSpeed != 0) secondToGameOver = maxSecondToGameOver;
    }

    private void CalcGameOver()//ゲームオーバーまでの残り時間を計算する
    {
        secondToGameOver -= Time.deltaTime;
        if (secondToGameOver <= 0) 
        {
            secondToGameOver = 0;
            isgameovered = true;
            GameOverAnimation();
        }
    }

    private void GameOverAnimation()//ゲームオーバー時の処理
    {
        Debug.Log("Game Over");

        float distanceMoved = Mathf.Abs(transform.position.x + offSet.x); // 移動距離の計算
        moneyManager.totalMoney = PlayerPrefs.GetFloat("TotalMoney", 0f); // 保存された合計金額を取得
        moneyManager.totalMoney += distanceMoved; // ゲームオーバー時の移動距離を合計金額に加算
        PlayerPrefs.SetFloat("TotalMoney", moneyManager.totalMoney); // 合計金額をPlayerPrefsに保存
        Debug.Log("Total Money Collected: " + moneyManager.totalMoney);

        //ゲームオーバー画面を表示
        gameoveredSummary.SetActive(true);
        gameoveredSummary.GetComponent<GameoveredSummaryManager>().SetupSummary(moneyManager.totalMoney, distanceMoved);
    }
}
