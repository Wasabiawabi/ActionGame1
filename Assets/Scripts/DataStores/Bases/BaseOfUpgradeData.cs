using System.Collections.Generic;
using UnityEngine;

namespace HandmadeLibrary.DataBase.Upgrade
{
    /// <summary>
    /// アップグレードに関するデータの基盤
    /// アップグレードの対象(Player もしくは StageObject)、レベル上限、アップグレードの強化形式(Increment もしくは Multiply)、レベルごとの強化コスト(強化コストは演算を用いず、手動で決める)
    /// </summary>
    public abstract class BaseOfUpgradeData : BaseOfData
    {
        //アップグレードの対象
        public enum TargetType { Player, StageObject }
        public TargetType TargetName
        {
            get => targetName;
            set => targetName = value;
        }
        [SerializeField]
        private TargetType targetName;


        //レベル上限
        public int MaxLevel
        {
            get => maxLevel;
            set => maxLevel = value;
        }
        [SerializeField]
        private int maxLevel;

        //アップグレードの強化形式
        public enum UpgradeType { Increment, Multiply }
        public UpgradeType upgradeType
        {
            get => _upgradeType;
            set => _upgradeType = value;
        }
        [SerializeField]
        private UpgradeType _upgradeType;

        //レベルごとの強化コスト
        public List<int> UpgradeCostList
        {
            get => upgradeCostList;
        }
        [SerializeField]
        private List<int> upgradeCostList = new List<int>();
    }
}