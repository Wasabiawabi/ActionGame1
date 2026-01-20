using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{

    [SerializeField] private UpgradesLevelHandler upgradesLevelHandler;
    
    public void SaveUpgradesData()
    {
        upgradesLevelHandler.SaveUpgradesDataFile();
    }
}
