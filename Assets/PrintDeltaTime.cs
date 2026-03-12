using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrintDeltaTime : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerGameOverAnimationHandler playerGameOverAnimationHandler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Time.deltaTime.ToString() + "\n" + playerGameOverAnimationHandler.secondToGameOver.ToString();
    }
}
