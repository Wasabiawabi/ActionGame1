using HandmadeLibrary.DataStore.Player;
using HandmadeLibrary.DataStore.StageObject;
using HandmadeLibrary.DataStore.Upgrade;
using UnityEngine;

/// <summary>
/// データを検索する機能
/// </summary>
public class DataSearchHandler : MonoBehaviour
{
    [SerializeField] public PlayerStatusDataStore playerStatusDataStore;
    [SerializeField] public StageObjectStatusDataStore stageObjectStatusDataStore;
    [SerializeField] public UpgradeDataStore upgradeDataStore;
}