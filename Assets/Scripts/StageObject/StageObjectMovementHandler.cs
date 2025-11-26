using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectMovementHandler : MonoBehaviour
{
    [Tooltip("ステージオブジェクトが生成されたときにY軸の位置と移動速度を決める.")]
    public bool summary;

    //スクリプトなど
    private Camera mainCamera;
    //変数
    private int objID;
    private float playerXSpeed;
    private float minWindY = 3f;
    private float minBallonPlaneY = 10f;
    private float minBallonX = 3f;
    private float minPlaneX = 10f;
    private bool isInitialized = false;
    private Vector3 moveSpeed;

    public void Initialize(int id, float playerSpeed, Camera mainCamera) // ステージオブジェクトの初期化
    {
        this.objID = id;
        this.playerXSpeed = playerSpeed;
        this.mainCamera = mainCamera ?? Camera.main; // フォールバック
        //Debug.Log("minWindY" + minWindY + "minBallonPlaneY" + minBallonPlaneY);

        if (this.mainCamera == null)
        {
            Debug.LogWarning($"StageObjectMovementHandler.Initialize: mainCamera が null です。Camera.main も null の場合、Y位置計算が期待通り動作しない可能性があります。objID={id}");
        }

        CalcYAxis();
        CalcMoveSpeed();

        isInitialized = true;
        // デバッグログ（呼び出し確認）
        //Debug.Log($"StageObjectMovementHandler.Initialize called: id={id}, playerSpeed={playerSpeed}, mainCamera={(this.mainCamera!=null?"ok":"null")}");
    }

    private void Update() // ステージオブジェクトの移動処理
    {
        if (!isInitialized) return;
        transform.Translate(moveSpeed * Time.deltaTime, Space.World);
    }

    private void CalcYAxis() // IDごとにY座標の位置を計算する
    {
        // 安全： mainCamera が null の場合は簡易固定 Y を使う
        var cam = mainCamera ?? Camera.main;

        if (objID == 0 || objID == 1 || objID == 2) // 花、サトウキビ、蜂の巣の場合、地面に接地させる
        {
            float randY = Random.Range(-2f, -1f);
            transform.position = new Vector3(transform.position.x, randY, transform.position.z);
            return;
        }

        // ViewportToWorldPoint に適切な z を渡す（カメラとオブジェクトの距離）
        float zDistance = 0f;
        if (cam != null)
        {
            zDistance = Mathf.Abs(cam.transform.position.z - transform.position.z);
        }

        if (objID == 3) // 風の場合、空中に浮かせる
        {
            float rand = Random.Range(0f, 1f);
            float y = (cam != null) ? cam.ViewportToWorldPoint(new Vector3(0, rand, zDistance)).y : 0f;
            if (y < minWindY) y = minWindY;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            return;
        }

        if (objID == 4 || objID == 5) // 風船、飛行機の場合、ある程度高い位置に浮かせる
        {
            float rand = Random.Range(0f, 1f);
            float y = (cam != null) ? cam.ViewportToWorldPoint(new Vector3(0, rand, zDistance)).y : 0f;
            if (y < minBallonPlaneY) y = minBallonPlaneY;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            return;
        }
    }

    private void CalcMoveSpeed() // IDごとに移動速度を計算する
    {
        // 花、サトウキビ、蜂の巣の場合、速度は0
        if (objID == 0 || objID == 1 || objID == 2) moveSpeed = Vector3.zero;
        // 空中の場合、定速
        else if (objID == 3) moveSpeed = new Vector3(1, 0, 0);
        // 風船の場合、プレイヤーの10%
        else if (objID == 4) moveSpeed = new Vector3(Mathf.Max(playerXSpeed / 10f, minBallonX), 0, 0);
        // 飛行機の場合、移動速度の最低値か、プレイヤーの半分(暫定)
        else if (objID == 5) moveSpeed = new Vector3(Mathf.Max(playerXSpeed / 2f, minPlaneX), 0, 0);
    }
}
