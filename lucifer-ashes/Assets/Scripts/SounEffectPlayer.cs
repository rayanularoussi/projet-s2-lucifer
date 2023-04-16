using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SounEffectPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1, sfx2;
    public AudioClip bg_music;

    public void Button()
    {
        src.clip = sfx1;
        src.Play();
    }

    public void Button2()
    {
        src.clip = sfx2;
        src.Play();
    }

}
