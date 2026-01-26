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
    
    // テキスト
    [SerializeField] private TextMeshProUGUI buffReminderText;

    // 変数
    private List<BuffStruct> buff = new List<BuffStruct>(); // バフ名と値
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
        Debug.Log("Buff:" + newBuff.buffName);
    }

    private void Update()
    {
        // バフの表示と時間管理
        if (buff.Count > 0)
        {
            for (int i = 0; i < buff.Count; i++)// 表示時間内のバフを表示
            {
                BuffStruct currentBuff = buff[i];
                buffReminderText.text = $"{currentBuff.buffName}{currentBuff.buffValue}";
                currentBuff.buffDuration -= Time.deltaTime;

                if (currentBuff.buffDuration <= 0)
                {
                    buff.RemoveAt(i);
                }
            }
        }
        else
        {
            buffReminderText.text = "";
        }

        if (buffAdded)// バフが追加されときは、追加されたバフ以外は消す
        {
            float time = buff[buff.Count-1].buffDuration;// 最後に追加されたバフ郡は全部同じ時間
            for (int i = 0; i < buff.Count - 1; i++)
            {
                // 最後に追加されたバフ郡以外は時間が違うはずだから消す
                if (buff[i].buffDuration != time) buff.RemoveAt(i);
            }
            buffAdded = false;
        }
    }
}
