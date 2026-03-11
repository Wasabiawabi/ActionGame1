using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameoveredSummaryManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    [SerializeField] private TextMeshProUGUI distanceMovedText;

    public void SetupSummary(float totalMoney, float distanceMoved)
    {
        totalMoneyText.text = "Total Distanse Point\n" + totalMoney.ToString("F2");
        distanceMovedText.text = "Distance Moved\n" + distanceMoved.ToString("F2") + "\nunits";
    }
}
