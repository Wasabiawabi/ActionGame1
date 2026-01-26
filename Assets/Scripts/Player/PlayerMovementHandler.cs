using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HandmadeLibrary.DataBase.Upgrade.Increment;


public class PlayerMovementHandler : MonoBehaviour
{
    [Tooltip("プレイヤーの実際の動きを作る.")]
    public bool summary;

    //スクリプト
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerBuffHandler playerBuffHandler;
    [SerializeField] private MoveCamera moveCamera;
    [SerializeField] private UpgradesLevelHandler upgradesLevelHandler;
    [SerializeField] private DataSearchHandler dataSearchHandler;
    [SerializeField] private Slider speedGaugeSlider;

    //使う変数
    [SerializeField] public Vector3 offSet = new Vector3(0, 0, 0);
    [HideInInspector] public float initialSpeedPower = 0f;
    public float initialSpeedPowerMultiply = 1f;
    [HideInInspector] public Vector3 playerPos = new Vector3(0, 0, 0);
    public float playerSpeed = 0f;
    [SerializeField] private float incrementSpeedReductionPerHeight = 10f;

    void Start()
    {
        //初めの位置に移動させておく
        transform.position = offSet;
    }

    void Update()
    {
        CalcInitialSpeedPower();
        Move();
    }

    private void Move()
    {
        MoveVertical();
        MoveForward();
        UpdatePlayerSpeed();
    }

    private void CalcInitialSpeedPower()// スタートの瞬間の速度を計算する
    {
        // パワーは0から1の間をとる
        //1に近づくほど値の変化のスピードが上昇する
        //上がったり下がったりを周期的に繰り返す
        initialSpeedPower = 1 - Mathf.Abs(Mathf.Sin(Time.time * initialSpeedPowerMultiply));
        speedGaugeSlider.value = initialSpeedPower;
    }

    public void GameStart()// ゲームスタート瞬間の処理
    {
        if (playerStatus == null) return;
        int idx = playerStatus.playerStatusName.IndexOf("maxInitialMovementSpeed");
        if (idx < 0 || idx >= playerStatus.playerStatusValue.Count) return;

        // 元の maxInitialMovementSpeed を取得
        float maxInitialSpeed = playerStatus.playerStatusValue[idx];

        // Upgrade レベルを取得（存在しない場合は0）
        int levelIncreaseInitial = 0;
        if (upgradesLevelHandler != null && upgradesLevelHandler.playerUpgradeData != null)
        {
            levelIncreaseInitial = upgradesLevelHandler.playerUpgradeData.increaceMaxInitialMovementSpeedLevel;
        }

        // 初速を設定
        playerSpeed = playerStatus.playerStatusValue[idx] * initialSpeedPower;

        // スピードゲージを非表示にする
        speedGaugeSlider.gameObject.SetActive(false);
    }

    private void MoveForward()//プレイヤーの移動処理
    {
        if (playerController == null) return;
        if(!playerController.started) return;//スタートしていなければ実行しない
        if(playerController.stopped) return;//止まっている状態なら実行しない
        transform.position = new Vector3(transform.position.x + playerSpeed * Time.deltaTime, transform.position.y, 0);
        UpdatePlayerPos();
    }

    private void UpdatePlayerSpeed()//プレイヤーの移動速度を減衰もしくは維持させる
    {
        // 参照チェック
        if (playerStatus == null || playerBuffHandler == null) return;
        if (playerController != null && !playerController.started) return; // ゲーム未開始なら処理しない

        // 各ステータス（秒あたりの値）を取得
        int idxDamp = playerStatus.playerStatusName.IndexOf("speedDampingPerSecond");
        int idxMax = playerStatus.playerStatusName.IndexOf("maxSpeed");
        int idxReduce = playerStatus.playerStatusName.IndexOf("speedDampingReductionWithNakama");

        if (idxDamp < 0 || idxMax < 0 || idxReduce < 0) return;
        if (idxDamp >= playerStatus.playerStatusValue.Count ||
            idxMax >= playerStatus.playerStatusValue.Count ||
            idxReduce >= playerStatus.playerStatusValue.Count) return;

        // 各ステータス
        float speedDampingPerSecond = playerStatus.playerStatusValue[idxDamp]; // 秒あたりの減衰量
        float maxSpeed = playerStatus.playerStatusValue[idxMax];
        float speedDampingReductionWithNakamaPerSecond = playerStatus.playerStatusValue[idxReduce];

        // 高さ依存の追加減衰（秒あたり）
        float additionalSpeedDampingPerSecond = 0f;
        try
        {
            additionalSpeedDampingPerSecond = playerStatus.GetPlayerHeight() / incrementSpeedReductionPerHeight;
        }
        catch
        {
            additionalSpeedDampingPerSecond = 0f;
        }

        // 合成して delta（今回フレームで減らす量）を計算：全て秒あたりの値なので最後に Time.deltaTime を掛ける
        float totalPerSecond = speedDampingPerSecond - speedDampingReductionWithNakamaPerSecond + additionalSpeedDampingPerSecond;
        float delta = totalPerSecond * Time.deltaTime;

        // スピード維持フラグが有効なら減衰は0
        if (playerBuffHandler.maintaining) delta = 0f;

        // 減衰を適用（最低0）
        playerSpeed -= delta;
        playerSpeed = Mathf.Max(0f, playerSpeed);

        // 速度制限を超えている場合は超過分に対して追加減衰（スピード維持フラグが有効でも実行する）
        if (playerSpeed > maxSpeed)
        {
            // ここでは超過量に比例して追加で減衰させる（元の意図を保ちつつ Time.deltaTime を適用）
            float over = playerSpeed - maxSpeed;
            float extraPerSecond = speedDampingPerSecond * over; // over が大きいほど強く減衰
            float extraDelta = extraPerSecond * Time.deltaTime;

            // 追加の減衰にもレベルによる緩和を適用
            extraDelta = Mathf.Max(0f, extraDelta - additionalSpeedDampingPerSecond * Time.deltaTime);
            playerSpeed -= extraDelta;
            playerSpeed = Mathf.Max(0f, playerSpeed);

            // 追加の減衰でも仲間による減衰軽減を考慮
            extraDelta = Mathf.Max(0f, extraDelta - speedDampingReductionWithNakamaPerSecond * Time.deltaTime);
            playerSpeed -= extraDelta;
            playerSpeed = Mathf.Max(0f, playerSpeed);
        }
    }

    private void MoveVertical()//上下の移動処理
    {
        if(playerController == null) return;
        if(!playerController.started) return;//スタートしていなければ実行しない
        if (playerController.isTouchingGround && playerController.movingToDownward) return;// 地面に接していて下方向の場合は移動しない
        //操作ごとにベクトルを変える
        //どちらも押されていないか、両方押されている場合は上下の移動は発生しない
        int sign = -1;
        if (playerController.movingToUpward) sign = 1;
        else if (playerController.movingToDownward) sign = -1;
        else return;
        if (playerStatus == null) return;
        int idx = playerStatus.playerStatusName.IndexOf("verticalMovementSpeed");
        if (idx < 0 || idx >= playerStatus.playerStatusValue.Count) return;
        float verticalMovement = playerStatus.playerStatusValue[idx] * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + verticalMovement * sign, 0);
    }

    private void UpdatePlayerPos()// プレイヤーの現在位置を更新する
    {
        playerPos = transform.position;
        if (moveCamera != null) moveCamera.Move(playerPos.x, playerPos.y);
    }
}
