using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BuffStruct : ScriptableObject
{
    public string buffName;
    public float buffValue;
    public float buffDuration;
}

public class BuffReminderHandler : MonoBehaviour
{
    /// <summary>
    /// バフ効果のリマインダーを管理する.
    /// PlayerBuffHandlerから送られてきた情報を使う.
    /// 一定時間直前のバフの内容を表示する
    /// </summary>
    
    // UIのプレハブ
    [SerializeField] private GameObject buff_Inc_Mlt;
    [SerializeField] private GameObject buff_Maintain;

    // プレハブを入れる子オブジェクト
    [SerializeField] private Transform buffParent;

    // フォント
    [SerializeField] private TMP_FontAsset buffFont;

    // 変数
    private List<BuffStruct> buff = new List<BuffStruct>(); // バフ名と値
    [SerializeField] private List<GameObject> buffObjects = new List<GameObject>(); // 生成したGameObject
    public float displayDuration = 2f; // バフ表示時間
    public bool buffAdded = false;

    public void PushBuffSummary(string buffName, float buffValue)
    {
        BuffStruct newBuff = ScriptableObject.CreateInstance<BuffStruct>();
        newBuff.buffName = buffName;
        newBuff.buffValue = buffValue;
        newBuff.buffDuration = displayDuration;
        buff.Add(newBuff);
        buffAdded = true;

        // buffNameに応じてゲームオブジェクトを生成
        GameObject buffObject = null;
        if (buffName == "Speed+" || buffName == "Speed×")
        {
            buffObject = Instantiate(buff_Inc_Mlt, buffParent);
            char calcChar = buffName[buffName.Length - 1];
            buffObject.GetComponentInChildren<TextMeshProUGUI>().text = calcChar.ToString() + buffValue.ToString("F1") + "m/s";
        }
        else if (buffName == "Maintain+")
        {
            buffObject = Instantiate(buff_Maintain, buffParent);
            buffObject.GetComponentInChildren<TextMeshProUGUI>().text = "+" + buffValue.ToString("F1") + "sec";
        }

        if (buffObject != null)
        {
            buffObject.GetComponentInChildren<TextMeshProUGUI>().font = buffFont;
        }

        if (buffObject != null)
        {
            buffObjects.Add(buffObject);
        }

        Debug.Log("Buff:" + newBuff.buffName);
    }

    private void Update()
    {
        // バフの表示と時間管理
        if (buff.Count > 0)
        {
            for (int i = buff.Count - 1; i >= 0; i--)
            {
                BuffStruct currentBuff = buff[i];
                currentBuff.buffDuration -= Time.deltaTime;

                if (currentBuff.buffDuration <= 0)
                {
                    // ゲームオブジェクトを破棄
                    if (i < buffObjects.Count && buffObjects[i] != null)
                    {
                        Destroy(buffObjects[i]);
                    }
                    buff.RemoveAt(i);
                    buffObjects.RemoveAt(i);
                }
            }
        }

        if (buffAdded)
        {
            if (buff.Count > 0)
            {
                float time = buff[buff.Count - 1].buffDuration;
                for (int i = buff.Count - 2; i >= 0; i--)
                {
                    // 最後に追加されたバフ郡以外は時間が違うはずだから消す
                    if (buff[i].buffDuration != time)
                    {
                        if (i < buffObjects.Count && buffObjects[i] != null)
                        {
                            Destroy(buffObjects[i]);
                        }
                        buff.RemoveAt(i);
                        buffObjects.RemoveAt(i);
                    }
                }
            }
            buffAdded = false;
        }
    }
}
