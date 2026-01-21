using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePanel : MonoBehaviour
{
    [SerializeField] private GameObject panelToActivate;
    [SerializeField] private GameObject panelToDeactivate;

    public void Change()
    {
        panelToActivate.SetActive(true);
        panelToDeactivate.SetActive(false);
    }
}
