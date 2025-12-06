using System.Collections;
using System.Collections.Generic;
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

    //プレイヤーの情報など
    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private Camera mainCamera;

    //変数
    [SerializeField] private GameObject[] stageObjectPrefabs;
    private List<float> minSpawnProbablity = new List<float>();
    private List<float> incrementSpawnProbablityPerSecond = new List<float>();
    [SerializeField] private List<float> nowSpawnProbablity = new List<float>();

    private void Start()
    {
        // 参照チェック
        if (dataSearchHandler == null)
        {
            Debug.LogError("StageObjectHandler: dataSearchHandler が割り当てられていません。Inspectorで設定してください。");
            enabled = false;
            return;
        }
        if (dataSearchHandler.stageObjectStatusDataStore == null || dataSearchHandler.stageObjectStatusDataStore.DataBase == null)
        {
            Debug.LogError("StageObjectHandler: stageObjectStatusDataStore またはその DataBase が割り当てられていません。");
            enabled = false;
            return;
        }
        if (dataSearchHandler.stageObjectStatusDataStore.DataBase.DataList == null)
        {
            Debug.LogError("StageObjectHandler: DataBase.DataList が null です。");
            enabled = false;
            return;
        }

        int capacity = dataSearchHandler.stageObjectStatusDataStore.DataBase.DataList.Count;

        // リストをクリアしてから初期化（再実行に耐える）
        minSpawnProbablity.Clear();
        incrementSpawnProbablityPerSecond.Clear();
        nowSpawnProbablity.Clear();

        // 各IDごとの出現確率の初期値を設定
        for (int i = 0; i < capacity; i++)
        {
            var statusData = dataSearchHandler.stageObjectStatusDataStore.GetDataByID(i);
            if (statusData == null)
            {
                minSpawnProbablity.Add(0f);
                incrementSpawnProbablityPerSecond.Add(0f);
                nowSpawnProbablity.Add(0f);
                continue;
            }

            float min = statusData.MinInstantiateProbability;
            float inc = statusData.InstantiateProbabilityIncrementPerSecond;

            minSpawnProbablity.Add(min);
            incrementSpawnProbablityPerSecond.Add(inc);
            nowSpawnProbablity.Add(min);
        }
    }

    private void Update()
    {
        if(playerController.started)CalcSpawnProbablity();
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
                        mover.Initialize(i, playerSpeed, mainCamera, playerBuffHandler, dataSearchHandler);
                    }
                }

                //出現確率を初期化する
                nowSpawnProbablity[i] = minSpawnProbablity[i];
            }
        }
    }
}
