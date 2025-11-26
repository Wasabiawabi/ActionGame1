using System.Collections.Generic;
using UnityEngine;

namespace HandmadeLibrary.DataBase.Upgrade.Increment
{
    /// <summary>
    /// 加算方式のアップグレードに関するデータ
    /// レベルアップごとの加算量
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Upgrade/Increment")]
    public class IncrementUpgradeData : BaseOfUpgradeData
    {
        public List<float> IncrementAmountList
        {
            get => incrementAmountList;
            set => incrementAmountList = value;
        }
        [SerializeField]
        private List<float> incrementAmountList;
    }
}
