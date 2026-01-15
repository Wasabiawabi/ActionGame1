using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Tooltip("カメラをプレイヤーに合わせて動かす")]
    public bool summary;

    // アタッチ
    [SerializeField] private BackgroundHandler backgroundHandler;
    [SerializeField] private GroundGenerator groundGenerator;

    // 変数
    [SerializeField] private float zOffset;
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;
    [SerializeField] private float minYPos;
    [SerializeField] private float maxYPos;

    private void Start()
    {
        //位置を初期化
        transform.position = new Vector3(0, 0, zOffset);
        CalcBound();
        groundGenerator.Init();
        groundGenerator.GenerateGrounds(minXPos, maxXPos);
    }

    public void Move(float playerPosX ,float playerPosY)// カメラの移動処理
    {
        // カメラがプレイヤーより後ろにいる場合、x座標をプレイヤーと同じにする。
        // y座標は常に同じにする。
        if (transform.position.x < playerPosX)
        {
            transform.position = new Vector3(playerPosX, playerPosY, zOffset);
        }
        else
        {
            transform.position = new Vector3(playerPosX, playerPosY, zOffset);
        }
        CalcBound();

        // 背景と地面の処理をここで実行
        backgroundHandler.CalcBackground(transform.position.x, transform.position.y);
        groundGenerator.GenerateGrounds(maxXPos, minXPos);
    }

    private void CalcBound()// カメラの描画範囲をゲーム内の座標で計算
    {
        Vector3 maxPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Mathf.Abs(zOffset)));
        Vector3 minPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Mathf.Abs(zOffset)));
        maxXPos = maxPos.x;
        maxYPos = maxPos.y;
        minXPos = minPos.x;
        minYPos = minPos.y;
    }

    public float GetMinXPos()
    {
        return minXPos;
    }
    public float GetMaxXPos()
    {
        return maxXPos;
    }
    public float GetMinYPos()
    {
        return minYPos;
    }
    public float GetMaxYPos()
    {
        return maxYPos;
    }

    public float GetScreenWidth()
    {
        return maxXPos - minXPos;
    }

    public float GetCameraPosX()
    {
        return transform.position.x;
    }

    public float GetCameraPosY()
    {
        return transform.position.y;
    }
}
