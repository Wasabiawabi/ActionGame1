using UnityEngine;

namespace HandmadeLibrary.DataBase.InitialStatus.StageObject
{
    /// <summary>
    /// StageObjectStatusDataBase は BaseOfDataBase<T> を継承している
    /// T は BaseOfInitialStatusData を継承した型のみ受け付ける
    /// </summary>
    [CreateAssetMenu(menuName = "DataBase/StageObjectStatus")]
    public class StageObjectStatusDataBase : BaseOfDataBase<StageObjectInitialStatusData> { }
}