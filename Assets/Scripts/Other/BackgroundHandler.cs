using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHandler : MonoBehaviour
{
    [Tooltip("カメラの座標を基に背景を動かす.\nレイヤーごとにスピードを変える.\n")]
    public bool summary;

    // アタッチ
    [SerializeField] private MoveCamera moveCamera;
    [SerializeField] private GameObject[] backgroundPrefabs;

    // 変数
    [SerializeField] private float gamma; // 背景ごとの移動スピードの減衰（0~1 程度を想定）
    private float[] backgroundWidth; // 背景ごとの幅（ワールド単位）
    private float[] nextBackgroundPosX; // 次に生成する背景の中心 x
    private float[] nextBackgroundPosY; // 次に生成する背景の中心 y
    private List<List<GameObject>> generatedBackgrounds; // 生成済み背景のリスト（レイヤーごと）
    [SerializeField] private float backgroundYOffset; // 背景の y オフセット
    private float previousCameraX;
    private float previousCameraY;

    private void Start()
    {
        // 参照チェック
        if (moveCamera == null)
        {
            Debug.LogError("BackgroundHandler: moveCamera が設定されていません。Inspectorで設定してください。");
            enabled = false;
            return;
        }
        if (backgroundPrefabs == null || backgroundPrefabs.Length == 0)
        {
            Debug.LogError("BackgroundHandler: backgroundPrefabs が設定されていません。");
            enabled = false;
            return;
        }

        Initialize();
    }

    // カメラの移動処理時に呼び出す（MoveCamera などから）
    public void CalcBackground(float cameraPosX, float cameraPosY)
    {
        GenerateBackground(cameraPosX, cameraPosY);
        MoveBackground(cameraPosX, cameraPosY);
        DestroyBackground(cameraPosX, cameraPosY);
    }

    // カメラ差分に応じて背景をパララックス移動させ、次の生成位置を更新
    private void MoveBackground(float cameraPosX, float cameraPosY)
    {
        float deltaX = cameraPosX - previousCameraX;
        float deltaY = cameraPosY - previousCameraY;

        for (int i = 0; i < generatedBackgrounds.Count; i++)
        {
            float layerFactor = Mathf.Pow(gamma, i); // 遠景ほど小さくなる想定
            // カメラ差分に基づく横移動（パララックス）
            float moveX = deltaX * (1f - layerFactor);
            // 縦位置はカメラYに合わせて少し追随（layerFactorで調整）
            float targetY = cameraPosY * layerFactor + backgroundYOffset;

            // 各生成済みオブジェクトに移動を適用
            var list = generatedBackgrounds[i];
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j] == null) continue;
                Vector3 pos = list[j].transform.position;
                pos.x += moveX;
                pos.y = targetY;
                list[j].transform.position = pos;
            }

            // 次に生成する位置もカメラ差分分だけ動かす（生成位置の追随）
            nextBackgroundPosX[i] += moveX;
            nextBackgroundPosY[i] = targetY;
        }

        previousCameraX = cameraPosX;
        previousCameraY = cameraPosY;
    }

    // カメラ範囲（左端〜右端）を埋めるように背景を生成（重複なし）
    private void GenerateBackground(float cameraPosX, float cameraPosY)
    {
        // 各レイヤーごとに画面範囲（中心位置）を計算して生成
        for (int i = 0; i < backgroundPrefabs.Length; i++)
        {
            float width = backgroundWidth[i];
            if (width <= 0f) continue;

            // 画面左端・右端（ワールド座標）
            float left = moveCamera.GetMinXPos();
            float right = moveCamera.GetMaxXPos();

            // 余白を若干取る（画面外からはみ出しても生成しておく）
            float margin = width * 1.0f;
            float generateLeft = left - margin;
            float generateRight = right + margin;

            // nextBackgroundPosX[i] を基に右側へ生成
            while (nextBackgroundPosX[i] <= generateRight)
            {
                Vector3 spawnPos = new Vector3(nextBackgroundPosX[i], nextBackgroundPosY[i], 0f);
                var prefab = backgroundPrefabs[Mathf.Clamp(i, 0, backgroundPrefabs.Length - 1)];
                if (prefab != null)
                {
                    
                    GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
                    generatedBackgrounds[i].Add(go);
                }
                // 次の中心位置へ
                nextBackgroundPosX[i] += width;
            }
        }
    }

    // 画面外に出た背景を破棄してリストから削除
    private void DestroyBackground(float cameraPosX, float cameraPosY)
    {
        for (int i = 0; i < generatedBackgrounds.Count; i++)
        {
            float width = backgroundWidth[i];
            if (width <= 0f) continue;

            float left = moveCamera.GetMinXPos();
            float right = moveCamera.GetMaxXPos();
            float margin = width * 2f;
            float destroyLeft = left - margin;
            float destroyRight = right + margin;

            var list = generatedBackgrounds[i];
            // 前からチェック（左側）
            for (int j = list.Count - 1; j >= 0; j--)
            {
                var go = list[j];
                if (go == null)
                {
                    list.RemoveAt(j);
                    continue;
                }
                float x = go.transform.position.x;
                if (x < destroyLeft || x > destroyRight)
                {
                    Destroy(go);
                    list.RemoveAt(j);
                }
                // 右側は同じループで判定済み
            }

            // 保証：もしリストが空なら nextBackgroundPosX を現在カメラ左寄りにリセットして再生成を容易にする
            if (list.Count == 0)
            {
                nextBackgroundPosX[i] = left;
                nextBackgroundPosY[i] = moveCamera.GetCameraPosY() + backgroundYOffset;
            }
            else
            {
                // 保守的に nextBackgroundPosX を最新の右端に揃える（右側生成の起点）
                var maxX = float.NegativeInfinity;
                foreach (var go in list) if (go != null) maxX = Mathf.Max(maxX, go.transform.position.x);
                nextBackgroundPosX[i] = float.IsNegativeInfinity(maxX) ? nextBackgroundPosX[i] : maxX + width;
            }
        }
    }

    // 初期化：幅や配列の準備、最初に画面を埋める
    private void Initialize()
    {
        int layers = backgroundPrefabs.Length;
        backgroundWidth = new float[layers];
        nextBackgroundPosX = new float[layers];
        nextBackgroundPosY = new float[layers];
        generatedBackgrounds = new List<List<GameObject>>(layers);

        // 初期の座標やオブジェクトの基本情報を取得する
        for (int i = 0; i < layers; i++)
        {
            // 各レイヤーの幅はレンダラーのバウンドから取得（SpriteRenderer / MeshRenderer 対応）
            float width = 0f;
            var prefab = backgroundPrefabs[i];
            if (prefab != null)
            {
                var spr = prefab.GetComponentInChildren<SpriteRenderer>();
                if (spr != null)
                {
                    width = spr.bounds.size.x;
                }
                else
                {
                    var rend = prefab.GetComponentInChildren<Renderer>();
                    if (rend != null) width = rend.bounds.size.x;
                }
            }
            if (width <= 0f)
            {
                width = 10f; // フォールバック値（プロジェクトに合わせて調整）
                Debug.LogWarning($"BackgroundHandler: backgroundPrefabs[{i}] の幅を取得できませんでした。デフォルト幅 {width} を使用します。");
            }
            backgroundWidth[i] = width;

            // 生成リストを用意
            generatedBackgrounds.Add(new List<GameObject>());

            // 初期の next pos はカメラ左端から
            float left = moveCamera.GetMinXPos();
            nextBackgroundPosX[i] = left;
            nextBackgroundPosY[i] = moveCamera.GetCameraPosY() + backgroundYOffset;
        }

        // 初期カメラ位置を保存
        previousCameraX = moveCamera.GetCameraPosX();
        previousCameraY = moveCamera.GetCameraPosY();

        // 最初に画面全体を埋める
        GenerateBackground(previousCameraX, previousCameraY);
    }
}