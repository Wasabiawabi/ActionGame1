using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class GroundDestroyer : MonoBehaviour
{
    [Tooltip("カメラの左端よりも左に行ったら破壊する\n他のオブジェクトにも適応可能.")]
    public bool summary;

    private Camera cam;
    public RectTransform rectTransform;
    private float cameraMinX;
    private float objLength;
    private void Start()
    {
        objLength = rectTransform.rect.width;
        cam = Camera.main;
        cameraMinX = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.transform.position.z)).x;
    }

    private void Update()
    {
        if (transform.position.x + objLength < cameraMinX) Destroy(gameObject);
        cameraMinX = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.transform.position.z)).x;
    }
}
