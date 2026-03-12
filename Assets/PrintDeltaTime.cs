using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PrintDeltaTime : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private StageObjectHandler stageObjectHandler;

    //void Start(){stageObjectHandler.nowSpawnProbablity = new List<float>();}

    // Update is called once per frame
    void Update()
    {
        String s = stageObjectHandler.nowSpawnProbablity.Count + "\n";
        for (int i = 0; i < stageObjectHandler.nowSpawnProbablity.Count; i++)
        {
            s += stageObjectHandler.nowSpawnProbablity[i] + "\n";
        }
        text.text = s;
    }
}
