using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : MonoBehaviour
{
    [SerializeField] private AudioClip _se;
    [SerializeField]private AudioSource _audioSource;

    public void Play()
    {
        _audioSource.PlayOneShot(_se);
    }
}
