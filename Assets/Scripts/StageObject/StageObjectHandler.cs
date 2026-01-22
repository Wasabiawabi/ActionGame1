using System.Collections;
using System.Collections.Generic;
using HandmadeLibrary.DataBase.Upgrade.Multiply;
using UnityEngine;

public class StageObjectHandler : MonoBehaviour
{
    [Tooltip("ステージオブジェクトの生成をする.")]
    public bool summary;

    //スクリプト
    [SerializeField] private DataSearchHandler dataSearchHandler;
    [SerializeField] private PlayerMovementHandler playerMovementHandler;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerBuffHandler playerBuffHandler;
    [SerializeField] private UpgradesLevelHandler upgradesLevelHandler; // 追加: レベル参照用
    [SerializeField] private PlayerGameOverAnimationHandler playerGameOverAnimationHandler;

    //プレイヤーの情報など
    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private Camera mainCamera;

    //変数
    [SerializeField] private GameObject[] stageObjectPrefabs;
    private List<float> minSpawnProbablity = new List<float>();
    private List<float> incrementSpawnProbablityPerSecond = new List<float>();
    [SerializeField] private List<float> nowSpawnProbablity = new List<float>();
    [SerializeField] private List<bool> notSpawn = new List<bool>(); // デバッグ用.出現させないオブジェクトにチェックを入れる

    private void Start()
    {
        // 必要な参照がセットされているか確認
        if (dataSearchHandler == null)
        {
            Debug.LogError("StageObjectHandler: dataSearchHandler が割り当てられていません。Inspectorで設定してください。");
            enabled = false;
            return;
        }

        var store = dataSearchHandler.stageObjectStatusDataStore;
        if (store == null)
        {
            Debug.LogError("StageObjectHandler: dataSearchHandler.stageObjectStatusDataStore が null です。");
            enabled = false;
            return;
        }

        var db = store.DataBase;
        if (db == null || db.DataList == null)
        {
            Debug.LogError("StageObjectHandler: stageObjectStatusDataStore.DataBase または DataList が null です。DataBase アセットが設定されているか確認してください。");
            enabled = false;
            return;
        }

        int capacity = db.DataList.Count;

        // リストを初期化（再起動時の残存を防ぐ）
        minSpawnProbablity.Clear();
        incrementSpawnProbablityPerSecond.Clear();
        nowSpawnProbablity.Clear();

        // レベル取得（存在しない場合は0）
        int levelMultiplier = 0;
        if (upgradesLevelHandler != null && upgradesLevelHandler.stageObjectUpgradeData != null)
        {
            levelMultiplier = Mathf.Max(0, upgradesLevelHandler.stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel);
        }

        float multiplier = Mathf.Pow(1.2f, levelMultiplier);

        // それぞれのIdごとの出現確率の初期値を設定する
        for (int i = 0; i < capacity; i++)
        {
            var statusData = store.GetDataByID(i);
            if (statusData == null)
            {
                // null のデータが存在する場合はデフォルトを入れて続行
                minSpawnProbablity.Add(0.0f);
                incrementSpawnProbablityPerSecond.Add(0.0f);
                nowSpawnProbablity.Add(0.0f);
                continue;
            }

            float minVal = statusData.MinInstantiateProbability;
            float incVal = statusData.InstantiateProbabilityIncrementPerSecond;
            int level = upgradesLevelHandler.stageObjectUpgradeData.increaseInstantiateProbabiltyPerFrameLevel;
            MultiplyUpgradeData data = dataSearchHandler.upgradeDataStore.GetDataByID(i) as MultiplyUpgradeData;
            float mlt = data.MultiplyRate;

            // レベルに応じて掛け算
            incVal *= Mathf.Pow(mlt, level);

            minSpawnProbablity.Add(minVal);
            incrementSpawnProbablityPerSecond.Add(incVal);
            nowSpawnProbablity.Add(minSpawnProbablity[i]);
        }
    }

    private void Update()
    {
        if(playerController.started && !playerGameOverAnimationHandler.isgameovered)CalcSpawnProbablity();
    }

    private void CalcSpawnProbablity()//出現確率を計算する
    {
        for (int i = 0; i < nowSpawnProbablity.Count; i++)
        {
            nowSpawnProbablity[i] += incrementSpawnProbablityPerSecond[i] * Time.deltaTime;

            //出現確率が乱数より増えたら出現させる
            float rand = Random.Range(0f, 100f);
            if (nowSpawnProbablity[i] >= rand)
            {
                //画面外の座標を計算する
                if (mainCamera == null || playerCollider == null)
                {
                    Debug.LogWarning("StageObjectHandler: mainCamera または playerCollider が設定されていません。Instantiate をスキップします。");
                    continue;
                }

                Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
                spawnPosition.x += stageObjectPrefabs[i].transform.lossyScale.x;
                spawnPosition.z = 0f;

                //ステージオブジェクトを出現させる
                if (stageObjectPrefabs == null || i >= stageObjectPrefabs.Length || stageObjectPrefabs[i] == null)
                {
                    Debug.LogWarning($"StageObjectHandler: stageObjectPrefabs[{i}] が設定されていないため Instantiate をスキップします。");
                }
                else
                {
                    if (notSpawn[i]) return; // デバッグ用.
                    GameObject stageObject = Instantiate(stageObjectPrefabs[i], spawnPosition, Quaternion.identity, transform);
                    var mover = stageObject.GetComponent<StageObjectMovementHandler>();
                    if (mover == null)
                    {
                        Debug.LogError($"StageObjectHandler: プレハブ '{stageObjectPrefabs[i]?.name}' に StageObjectMovementHandler がアタッチされていません。");
                    }
                    else
                    {
                        // playerMovementHandler があれば playerSpeed を渡す（無ければ 0 を渡す）
                        float playerSpeed = (playerMovementHandler != null) ? playerMovementHandler.playerSpeed : 0f;
                        mover.Initialize(i, playerSpeed, mainCamera, playerBuffHandler, dataSearchHandler, upgradesLevelHandler);
                    }
                }

                //出現確率を初期化する
                nowSpawnProbablity[i] = minSpawnProbablity[i];
            }
        }
    }
}
