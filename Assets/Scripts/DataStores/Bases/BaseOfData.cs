using UnityEngine;

namespace HandmadeLibrary.DataBase
{
    /// <summary>
    /// データの基盤
    /// 名前とID
    /// </summary>
    public abstract class BaseOfData : ScriptableObject
    {
        public string Name
        {
            get => name;
            set => name = value;
        }
        [SerializeField]
        private new string name;

        public int ID
        {
            get => id;
            set => id = value;
        }
        [SerializeField]
        private int id;
    }
}