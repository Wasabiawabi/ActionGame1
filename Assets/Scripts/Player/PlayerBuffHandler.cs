using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerBuffHandler : MonoBehaviour
{
    [Tooltip("プレイヤーのバフの効果と残り時間を管理する.")]
    public bool summary;

    //スクリプト
    [SerializeField] private PlayerMovementHandler playerMovementHandler;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private BuffReminderHandler buffReminderHandler;
    [SerializeField] private AudioSource audioSource;


    //変数
    public float maintainSpeedDurationSum;
    public bool maintaining = false;

    // Update is called once per frame
    void Update()
    {
        //スピードの残り維持時間を計算
        maintaining = maintainSpeedDurationSum > 0;
        maintainSpeedDurationSum = Mathf.Max(0, maintainSpeedDurationSum - Time.deltaTime);
    }

    public void IncrementSpeed(float incrementNum)//移動速度の加算
    {
        int idx = playerStatus.playerStatusName.IndexOf("buffDampingWithNakama");
        float buffDampingWithNakama = playerStatus.playerStatusValue[idx];
        playerMovementHandler.playerSpeed += incrementNum / (1 + buffDampingWithNakama);
        buffReminderHandler.PushBuffSummary("Speed+", incrementNum);
    }

    public void MultiplySpeed(float multiplyNum)//移動速度の倍率強化
    {
        int idx = playerStatus.playerStatusName.IndexOf("buffDampingWithNakama");
        float buffDampingWithNakama = playerStatus.playerStatusValue[idx];
        Debug.Log(buffDampingWithNakama);
        playerMovementHandler.playerSpeed *= multiplyNum;
        buffReminderHandler.PushBuffSummary("Speed×", multiplyNum);
    }

    public void UpdateMaintainSpeed(float dulation)
    {
        maintainSpeedDurationSum += dulation;
        buffReminderHandler.PushBuffSummary("Maintain+", dulation);
    }

    public void PlaySound(AudioClip clip)
    {
    audioSource.PlayOneShot(clip);
    }
}
