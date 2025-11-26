using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDestroyer : MonoBehaviour
{
    [Tooltip("画面外に出た地面を破壊する.\n他のオブジェクトにも適応可能.")]
    public bool summary;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
