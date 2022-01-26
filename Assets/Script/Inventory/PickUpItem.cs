using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public ItemSo item;

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

            for (int i = 0; i < InventoryManager.instance.inventory.Count; i++)
            {
                if (item.title == InventoryManager.instance.inventory[i].title && item.isStackable && InventoryManager.instance.inventory.Count > 0)
                {
                    item.amount += InventoryManager.instance.inventory[i].amount;
                    InventoryManager.instance.inventory.Remove(InventoryManager.instance.inventory[i]);
                }
                else
                {
                    item.amount += InventoryManager.instance.inventory[i].amount;
                }
                
            }


           
            InventoryManager.instance.inventory.Add(item);
            Destroy(gameObject);

        }
        
    }
}
