using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSoundTrack : MonoBehaviour
{
    public AudioClip currentArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            AudioManager.instance.ChangeSoundTrack(currentArea);
        }
    }
}
