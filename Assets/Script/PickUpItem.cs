using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            foreach (var item in QuestManager.instance.allQuest)
            {
                if(item.statut == QuestSO.Statut.accepter && item.objectTofind == gameObject.name)
                {
                    item.actualAmount++;
                }
            }
            Destroy(gameObject);

        }
    }
}
