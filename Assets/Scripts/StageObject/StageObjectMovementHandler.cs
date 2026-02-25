using UnityEngine;
using System.IO;
using HandmadeLibrary.DataBase.Upgrade.Multiply;
using HandmadeLibrary.DataBase.Upgrade.Increment;

public class StageObjectMovementHandler : MonoBehaviour
{
    [Tooltip("ステージオブジェクトが生成されたときにY軸の位置と移動速度を決める.")]
    public bool summary;

    //スクリプトなど
    private Camera mainCamera;
    private PlayerBuffHandler playerBuffHandler;
    private DataSearchHandler dataSearchHandler;
    private UpgradesLevelHandler upgradesLevelHandler;
    //変数
    private int objID;
    private float playerXSpeed;
    private float minWindY = 3f;
    private float minY = 0f;
    private float minBallonPlaneY = 10f;
    private float minBallonX = 3f;
    private float minPlaneX = 10f;
    private bool isInitialized = false;
    private Vector3 moveSpeed;
    private bool[] availableBuff = { false, false, false };
    private int availableBuffCnt = 0;
    private int buffid = 0;
    float speedIncremnt;
    float speedMultiply;
    float maintainSpeedtime;
    int multipleBuffPercentLevel;
    int maxMultipleBuffLevel;
    int increaseBuffEffectLevel;
    int maxMultipleBuff;
    int multipleBuffPercent;

    public void Initialize(int id, float playerSpeed, Camera mainCamera, PlayerBuffHandler playerBuffHandler, DataSearchHandler dataSearchHandler, UpgradesLevelHandler upgradesLevelHandler) // ステージオブジェクトの初期化
    {
        this.objID = id;
        this.playerXSpeed = playerSpeed;
        this.mainCamera = mainCamera ?? Camera.main; // フォールバック
        this.playerBuffHandler = playerBuffHandler ?? null; // フォールバック
        this.dataSearchHandler = dataSearchHandler ?? null; // フォールバック
        this.upgradesLevelHandler = upgradesLevelHandler ?? null; // フォールバック
        //Debug.Log("minWindY" + minWindY + "minBallonPlaneY" + minBallonPlaneY);

        if (this.mainCamera == null)
        {
            Debug.LogWarning($"StageObjectMovementHandler.Initialize: mainCamera が null です。Camera.main も null の場合、Y位置計算が期待通り動作しない可能性があります。objID={id}");
        }

        CalcYAxis();
        CalcMoveSpeed();
        CalcBuff();
        buffid = Random.Range(0, availableBuffCnt - 1); //  与えるバフの種類をここで決める
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

        if (objID == 0) // 花、蜂の巣の場合、地面に接地させる
        {
            float randY = Random.Range(-1.5f, -0.5f);
            transform.position = new Vector3(transform.position.x, randY, transform.position.z);
            return;
        }
        else if (objID == 2) // 蜂の巣の場合、地面から少し浮かせる
        {
            float rand = Random.Range(0f, 3f);
            transform.position = new Vector3(transform.position.x, rand, transform.position.z);
            return;
        }

        if (objID == 1) // 粒の場合、自由に空中に浮かせる
        {
            float rand = Random.Range(0f, 1f);
            float y = (cam != null) ? cam.ViewportToWorldPoint(new Vector3(0, rand, 0)).y : 0f;
            if (y < minY) y = minY;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
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
        // 花、粒、蜂の巣の場合、速度は0
        if (objID == 0 || objID == 1 || objID == 2) moveSpeed = Vector3.zero;
        // 空中の場合、定速
        else if (objID == 3) moveSpeed = new Vector3(1, 0, 0);
        // 風船の場合、プレイヤーの10%
        else if (objID == 4) moveSpeed = new Vector3(Mathf.Max(playerXSpeed / 10f, minBallonX), 0, 0);
        // 飛行機の場合、移動速度の最低値か、プレイヤーの半分(暫定)
        else if (objID == 5) moveSpeed = new Vector3(Mathf.Max(playerXSpeed / 2f, minPlaneX), 0, 0);
    }

    private void CalcBuff() //  自身が消費されたときに与えられ得るバフを計算する
    {
        // 自身が与えるバフ効果の初期値を取得する
        var data = dataSearchHandler.stageObjectStatusDataStore.GetDataByID(objID);
        speedIncremnt     = data.SpeedIncrement;
        speedMultiply     = data.SpeedMultiply;
        maintainSpeedtime = data.SaveSpeedTime;

        if (speedIncremnt > 0) availableBuff[0] = true;
        if (speedMultiply > 0) availableBuff[1] = true;
        if (maintainSpeedtime > 0) availableBuff[2] = true;

        // 自身のバフの性能に関わるアップグレードのレベルを取得する
        increaseBuffEffectLevel = upgradesLevelHandler.stageObjectUpgradeData.increaseBuffEffect;
        maxMultipleBuffLevel = upgradesLevelHandler.stageObjectUpgradeData.maxMultipleBuffLevel;
        multipleBuffPercentLevel = upgradesLevelHandler.stageObjectUpgradeData.maxMultipleBuffLevel;

        // レベルごとのバフの強化を取得する
        MultiplyUpgradeData mulData = dataSearchHandler.upgradeDataStore.GetDataByID(21) as MultiplyUpgradeData;
        float mlt = mulData.MultiplyRate;

        // 実際に付与されるバフの効果を計算する
        speedIncremnt *= Mathf.Pow(mlt, increaseBuffEffectLevel);
        speedMultiply *= Mathf.Pow(mlt, increaseBuffEffectLevel);
        maintainSpeedtime *= Mathf.Pow(mlt, increaseBuffEffectLevel);

        // 一度に与えられるバフの数の最大値
        IIncrementUpgradeData data2 = dataSearchHandler.upgradeDataStore.GetDataByID(20) as IIncrementUpgradeData;
        maxMultipleBuff = maxMultipleBuffLevel >= 1 ? (int)data2.IncrementAmountList[maxMultipleBuffLevel - 1] : 1;

        // 2回以上バフが付与される確率
        IIncrementUpgradeData data3 = dataSearchHandler.upgradeDataStore.GetDataByID(19) as IIncrementUpgradeData;
        multipleBuffPercent = multipleBuffPercentLevel >= 1 ? (int)data3.IncrementAmountList[multipleBuffPercentLevel - 1] : 0;
    }

    private void InvokeBuff()
    {
        while (true)
        {
            int buffId = Random.Range(0, 3);
            //Debug.Log("buffId" + buffId);
            if (buffId == 0 && availableBuff[0])
            {
                playerBuffHandler.IncrementSpeed(speedIncremnt);
                Debug.Log("speedIncremnt" + speedIncremnt);
                return;
            }

            if (buffId == 1 && availableBuff[1])
            {
                playerBuffHandler.MultiplySpeed(speedMultiply);
                Debug.Log("speedMultiply" + speedMultiply);
                return;
            }

            if (buffId == 2 && availableBuff[2])
            {
                playerBuffHandler.UpdateMaintainSpeed(maintainSpeedtime);
                Debug.Log("Added time" + maintainSpeedtime);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //  プレイヤーが当たったときにバフを付与する
    {
        if (collision.CompareTag("Player"))
        {
            int buffCount = 1;
            InvokeBuff();
            for (int i = buffCount; i < maxMultipleBuff; i++)
            {
                if (Random.Range(0, 100) <= multipleBuffPercent)
                {
                    buffCount++;
                    InvokeBuff();
                }
            }
            Destroy(gameObject);
        }
    }
}
