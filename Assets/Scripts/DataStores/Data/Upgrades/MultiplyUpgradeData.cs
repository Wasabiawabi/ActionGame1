using UnityEngine;

namespace HandmadeLibrary.DataBase.Upgrade.Multiply
{
    /// <summary>
    /// 加算方式のアップグレードに関するデータ
    /// レベルアップごとの倍率
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Upgrade/Multiply")]
    public class MultiplyUpgradeData : BaseOfUpgradeData
    {
        public float MultiplyRate
        {
            get => multiplyRate;
        }
        [SerializeField]
        private float multiplyRate;
    }
}
