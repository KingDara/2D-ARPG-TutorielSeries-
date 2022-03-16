using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TP_Zone : MonoBehaviour
{
    public string nextZone;
    public string actualZone;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
           
                GameManager.instance.previousZone = actualZone;
                SceneManager.LoadScene(nextZone);

           
        }
    }
}
