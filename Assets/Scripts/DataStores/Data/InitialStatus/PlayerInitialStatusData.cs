using UnityEngine;

namespace HandmadeLibrary.DataBase.InitialStatus.Player
{
    /// <summary>
    /// プレイヤーのステータスの初期値
    /// スピード上限、毎秒の速度の減衰量、ゲーム開始時の初速度の最大値、上下移動のスピード
    /// 仲間の人数の上限、仲間がいるときのバフの分散、仲間がいるときの毎秒の速度減衰の減少量
    /// 攻撃で倒される確定数、報酬の割合の増加
    /// </summary>
    [CreateAssetMenu(menuName = "Data/InitialStatus/Player")]
    public class PlayerInitialStatusData : BaseOfInitialStatusData
    {
        public float MaxSpeed
        {
            get => maxSpeed;
        }
        [SerializeField]
        private float maxSpeed;

        public float SpeedDampingPerSecond
        {
            get => speedDampingPerSecond;
        }
        [SerializeField]
        private float speedDampingPerSecond;

        public float MaxInitialMovementSpeed
        {
            get => maxInitialMovementSpeed;
        }
        [SerializeField]
        private float maxInitialMovementSpeed;

        public float VerticalMovementSpeed
        {
            get => verticalMovementSpeed;
        }
        [SerializeField]
        private float verticalMovementSpeed;

        public int MaxCompanion
        {
            get => maxCompanion;
        }
        [SerializeField]
        private int maxCompanion;

        public float BuffDampingWithCompanion
        {
            get => buffDampingWithCompanion;
        }
        [SerializeField]
        private float buffDampingWithCompanion;

        public float SpeedDampingReductionWithCompanion
        {
            get => speedDampingReductionWithCompanion;
        }
        [SerializeField]
        private float speedDampingReductionWithCompanion;

        public int PlayerHp
        {
            get => playerHp;
        }
        [SerializeField]
        private int playerHp;

        public float RewardLate
        {
            get => rewardLate;

        }
        [SerializeField]
        private float rewardLate;
    }
}