using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAnimator : MonoBehaviour
{
    public int fps = 1;
    private int cnt = 0;
    public float offset = 0f;
    public Image image;
    public Sprite[] frames;

    private void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }
    private void Update()
    {
        cnt++;
        if (cnt % fps == 0)
        {
            int frameIdx = ((cnt / fps) + (int)offset) % frames.Length;
            image.sprite = frames[frameIdx];
        }
    }
}
