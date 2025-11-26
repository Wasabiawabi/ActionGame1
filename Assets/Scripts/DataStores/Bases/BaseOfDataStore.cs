using HandmadeLibrary.DataBase;
using UnityEngine;

namespace HandmadeLibrary.DataStore
{
    /// <summary>
    /// データベースを外部からの参照するための基盤
    /// </summary>
    public abstract class BaseOfDataStore<T, U> : MonoBehaviour where T : BaseOfDataBase<U> where U : BaseOfData
    {
        public T DataBase
        {
            get => dataBase;
        }
        [SerializeField]
        protected T dataBase;//継承しているクラスではアクセスできるようにする

        /// <summary>
        /// 名前からデータベースを取得する
        /// </summary>

        public U GetDataByName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return default; }

            return DataBase.DataList.Find(data => data.Name == name);
        }

        /// <summary>
        /// Idを用いてデータベースを取得
        /// </summary>
        
        public U GetDataByID(int id)
        {
            return DataBase.DataList.Find(data => data.ID == id);
        }
    }
}