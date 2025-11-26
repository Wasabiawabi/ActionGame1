using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NakamaHandler : MonoBehaviour
{
    [Tooltip("仲間の効果を決める.また、仲間の人数の増減の管理をする")]
    public bool summary;

    //スクリプト
    [SerializeField] private PlayerStatus playerStatus;

    //変数
    public int nakamaNum;
    public float maxMaintainNakamaDuration;
    public float maintainNakamaDuration;
    public float buffDamping;
    public float speedDampingReduction;

    private void Update()
    {
        CalcMaintainNakamaDuration();
        CalcNakamaEffect();
    }

    public void IncrementNakamaNum()//仲間に触れたときに実行
    {
        int idx = playerStatus.playerStatusName.IndexOf("maxNakama");
        float maxNakama = playerStatus.playerStatusValue[idx];
        nakamaNum = Mathf.Max(nakamaNum + 1, (int)maxNakama);
        maintainNakamaDuration = maxMaintainNakamaDuration;
    }

    private void CalcMaintainNakamaDuration()//仲間が1匹減るまでの時間を計算する
    {
        if (nakamaNum <= 0)//初めの値の入力をしつつ、仲間がいない場合の処理をする
        {
            maintainNakamaDuration = maxMaintainNakamaDuration;
            return;
        }
        maintainNakamaDuration -= Time.deltaTime;
        if(maintainNakamaDuration <= 0)// 残り時間が0になったら一匹減らして時間を更新
        {
            nakamaNum--;
            maintainNakamaDuration = maxMaintainNakamaDuration;
        }
    }

    private void CalcNakamaEffect()//仲間がいることによる効果を計算する.効果は仲間1匹ごとに増える
    {
        int idx = playerStatus.playerStatusName.IndexOf("buffDampingWithNakama");
        float buffDampingWithNakama = playerStatus.playerStatusValue[idx];
        buffDamping = buffDampingWithNakama * nakamaNum;

        idx = playerStatus.playerStatusName.IndexOf("speedDampingReductionWithNakama");
        float speedDampingReductionWithNakama = playerStatus.playerStatusValue[idx];
        speedDampingReduction = speedDampingReductionWithNakama * nakamaNum;
    }
}
