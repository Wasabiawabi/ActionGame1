using UnityEngine;

namespace HandmadeLibrary.DataBase.InitialStatus.Player
{
    /// <summary>
    /// PlayerStatusDataBase は PlayerInitialStatusData を要素とするデータベース
    /// </summary>
    [CreateAssetMenu(menuName = "DataBase/PlayerStatus")]
    public class PlayerStatusDataBase: BaseOfDataBase<PlayerInitialStatusData> { }
}