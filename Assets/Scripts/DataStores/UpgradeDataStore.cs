using HandmadeLibrary.DataBase.Upgrade;
using UnityEngine;

namespace HandmadeLibrary.DataStore.Upgrade
{
    /// <summary>
    /// UpgradeDataBase を基底クラスのリストに格納できるようにした DataStore
    /// T は BaseOfUpgradeData を継承した型
    /// </summary>
    
    public class UpgradeDataStore : BaseOfDataStore<UpgradeDataBase, BaseOfUpgradeData>{ }
    /*勉強メモ：MonoBehaviourの継承
     * ジェネリック型のクラスには、MonoBehaviourを継承させることができない。
     * 最終的にスクリプトとしてUnity側のコンポーネントにアタッチされるクラスでは、ジェネリック型の使用を避ける必要がある。
     */
}