using HandmadeLibrary.DataBase.InitialStatus.Player;

namespace HandmadeLibrary.DataStore.Player
{
    /// <summary>
    /// PlayerStatusDataStore は PlayerStatusDataBase と PlayerInitialStatusData を扱う DataStore
    /// </summary>
    public class PlayerStatusDataStore : BaseOfDataStore<PlayerStatusDataBase, PlayerInitialStatusData> { }
    /*勉強メモ：MonoBehaviourの継承
     * ジェネリック型のクラスには、MonoBehaviourを継承させることができない。
     * 最終的にスクリプトとしてUnity側のコンポーネントにアタッチされるクラスでは、ジェネリック型の使用を避ける必要がある。
     */
}