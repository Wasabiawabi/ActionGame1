using System.Collections.Generic;
using UnityEngine;

namespace HandmadeLibrary.DataBase
{
    /// <summary>
    /// データベースの基盤
    /// </summary>
    /*勉強メモ：where制約
     * この場合は、BaseOfDataもしくはBaseOfDataを継承したクラス型のみを受け付ける
     */

    public abstract class BaseOfDataBase<T> : ScriptableObject where T : BaseOfData
    {
        public List<T> DataList
        {
            get => dataList;
        }
        [SerializeField]
        private List<T> dataList = new List<T>();
    }
}