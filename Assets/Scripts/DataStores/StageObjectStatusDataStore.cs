using HandmadeLibrary.DataBase.InitialStatus.StageObject;

namespace HandmadeLibrary.DataStore.StageObject
{
    /// <summary>
    /// StageObjectStatusDataStore は BaseOfDataStore<T, U> を継承する
    /// T は BaseOfInitialStatusData を継承した型のみを受け付ける
    /// </summary>
    public class StageObjectStatusDataStore : BaseOfDataStore<StageObjectStatusDataBase, StageObjectInitialStatusData> { }
    /*勉強メモ：MonoBehaviourの継承
     * ジェネリック型のクラスには、MonoBehaviourを継承させることができない。
     * 最終的にスクリプトとしてUnity側のコンポーネントにアタッチされるクラスでは、ジェネリック型の使用を避ける必要がある。
     */
}