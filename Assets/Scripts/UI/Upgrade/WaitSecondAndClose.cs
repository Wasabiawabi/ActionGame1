using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class WaitSecondAndClose : MonoBehaviour
{
    public float seconds = 0;
    private float count = 0;

    private void OnEnable()
    {
        count = 0;
    }

    private void Update()
    {
        if(seconds < count) gameObject.SetActive(false);
        count += Time.deltaTime;
    }
}
