using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Tooltip("ボタンを押すなどして関数が実行された場合、任意のシーンへ遷移する")]
    [SerializeField] private bool summary;
    [SerializeField] private string sceneName;

    public void GoToSelectedScene()
    {
            SceneManager.LoadScene(sceneName);
    }
}
