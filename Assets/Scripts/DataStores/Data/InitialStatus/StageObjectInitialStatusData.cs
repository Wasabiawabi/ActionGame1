using UnityEngine;

namespace HandmadeLibrary.DataBase.InitialStatus.StageObject
{
    /// <summary>
    /// ステージに登場するオブジェクトのステータスの初期値
    /// オブジェクトの出現確率の最低値、フレームごとの加算値
    /// 消費されたときのプレイヤーのスピードの加算量、乗算量、維持時間
    /// 蜂が出現するかどうか
    /// </summary>
    [CreateAssetMenu(menuName = "Data/InitialStatus/StageObject")]
    public class StageObjectInitialStatusData : BaseOfInitialStatusData
    {
        public float MinInstantiateProbability
        {
            get => minInstantiateProbability;
        }
        [SerializeField]
        private float minInstantiateProbability;

        public float InstantiateProbabilityIncrementPerSecond
        {
            get => instantiateProbabilityIncrementPerSecond;
        }
        [SerializeField]
        private float instantiateProbabilityIncrementPerSecond;

        public float SpeedIncrement
        {
            get => speedIncrement;
        }
        [SerializeField]
        private float speedIncrement;

        public float SpeedMultiply
        {
            get => speedMultiply;
        }
        [SerializeField]
        private float speedMultiply;

        public float SaveSpeedTime
        {
            get => saveSpeedTime;
        }
        [SerializeField]
        private float saveSpeedTime;

        public bool IsBeeAppear
        {
            get => isBeeAppear;
        }
        [SerializeField]
        private bool isBeeAppear;
    }
}