using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScene : MonoBehaviour
{

    [SerializeField] private UpgradesLevelHandler upgradesLevelHandler;
    public void ReloadCurrentScene()
    {
        upgradesLevelHandler.SaveUpgradesDataFile();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
