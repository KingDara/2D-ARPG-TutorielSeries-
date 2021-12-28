using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public List<ItemSo> inventory;
    public int inventoryLenght = 63;
    public GameObject inventoryPanel, holderSlot;
    private GameObject slot;
    public GameObject prefabs;

    public static InventoryManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && !inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(true);
            if (holderSlot.transform.childCount > 0)
            {
                foreach (Transform item in holderSlot.transform)
                {
                    Destroy(item.gameObject);
                }
            }

            for (int i = 0; i < inventory.Count; i++)
            {
                slot = Instantiate(prefabs, transform.position, transform.rotation);
                slot.transform.SetParent(holderSlot.transform);

                if (inventory[i] != null)
                {
                    TextMeshProUGUI amount = slot.transform.Find("amount").GetComponent<TextMeshProUGUI>();
                    Image img = slot.transform.Find("icon").GetComponent<Image>();

                    amount.text = inventory[i].amount.ToString();
                    img.sprite = inventory[i].icon;
                }
            }

        }
        else if(Input.GetKeyDown(KeyCode.I) && inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(false);
        }

    }
}
