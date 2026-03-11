using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostManager : MonoBehaviour
{
    // コンポーネント
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private MoneyManager _moneyManager;
    public void SetCost() => _text.text = "Balance " + _moneyManager.totalMoney.ToString();

    private void Update()
    {
        SetCost();
    }
}
