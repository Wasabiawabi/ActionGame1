using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class OpenInfo : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    public void Open()
    {
        infoPanel.SetActive(true);
    }

    public void Close()
    {
        infoPanel.SetActive(false);
    }

    public async void WaitSeconds(float seconds)
    {
        // 指定した時間関数を終わらせない
        await Task.Delay((int)(seconds * 1000));
    }
}
