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
    public TextMeshProUGUI title, descriptionObject;
    public Image iconDescrition;

    [Header("Description")]
    public GameObject holderDescription;
    private int amountToUse;
    [SerializeField] private TextMeshProUGUI valueToUse;
    [SerializeField] private Button plusButton, moinButton;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject removeButton;
    [SerializeField] private GameObject amountToRemove;




    public static InventoryManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
       


        if (Input.GetKeyDown(KeyCode.I) && !inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(true);
            RefreshInventory();

        }
        else if(Input.GetKeyDown(KeyCode.I) && inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(false);
            amountToUse =0;
        }

    }

    private void RefreshInventory()
    {
        if (holderSlot.transform.childCount > 0)
        {
            foreach (Transform item in holderSlot.transform)
            {
                Destroy(item.gameObject);
            }
        }

        for (int i = 0; i < inventoryLenght; i++)
        {


            if (i <= inventory.Count - 1)
            {
                slot = Instantiate(prefabs, transform.position, transform.rotation);
                slot.transform.SetParent(holderSlot.transform);
                TextMeshProUGUI amount = slot.transform.Find("amount").GetComponent<TextMeshProUGUI>();
                Image img = slot.transform.Find("icon").GetComponent<Image>();
                slot.GetComponent<SlotItem>().itemSlot = i;
                amount.text = inventory[i].amount.ToString();
                img.sprite = inventory[i].icon;
            }
            else if (i > inventory.Count - 1)
            {
                slot = Instantiate(prefabs, transform.position, transform.rotation);
                slot.transform.SetParent(holderSlot.transform);
                slot.GetComponent<SlotItem>().itemSlot = i;
                TextMeshProUGUI amount = slot.transform.Find("amount").GetComponent<TextMeshProUGUI>();
                Button iconButton = slot.transform.Find("icon").GetComponent<Button>();
                iconButton.enabled = false;

                amount.gameObject.SetActive(false);
            }
        }
    }

    public void ChargeItem(int i)
    {
        amountToUse =0;
        valueToUse.text = amountToUse + "/" + inventory[i].maxAmount;

        holderDescription.SetActive(true);
        title.text = inventory[i].title;
        descriptionObject.text = inventory[i].description;
        iconDescrition.sprite = inventory[i].icon;

        if (inventory[i].type == ItemSo.Type.Comsommable)
        {
            useButton.SetActive(true);
            removeButton.SetActive(true);
            amountToRemove.SetActive(true);
        }
        else if(inventory[i].type == ItemSo.Type.Quest )
        {
            useButton.SetActive(false);
            removeButton.SetActive(false);
            amountToRemove.SetActive(false);
        }
        else if(inventory[i].type == ItemSo.Type.Commun)
        {
            useButton.SetActive(false);
            removeButton.SetActive(true);
            amountToRemove.SetActive(true);
        }

        plusButton.GetComponent<Button>().onClick.RemoveAllListeners();
        plusButton.GetComponent<Button>().onClick.AddListener(delegate { PlusButton(i); });

        moinButton.GetComponent<Button>().onClick.RemoveAllListeners();
        moinButton.GetComponent<Button>().onClick.AddListener(delegate { MoinsButton(i); });

        useButton.GetComponent<Button>().onClick.RemoveAllListeners();
        useButton.GetComponent<Button>().onClick.AddListener( delegate { UseItem(i); });

        removeButton.GetComponent<Button>().onClick.RemoveAllListeners();
        removeButton.GetComponent<Button>().onClick.AddListener(delegate { RemoveItem(i); });
    }

    public void UseItem(int i )
    {
            for (int x = 0; x < amountToUse; x++)
            {
                PlayerController.instance.currentHealth += inventory[i].amountToHeal;

                if (inventory[i].amount == 1)
                {
                    inventory.Remove(inventory[i]);
                    holderDescription.SetActive(false);

                break;
                }
                else
                {
                    inventory[i].amount--;
                }

            }

            //Refresh l'inventaire
            RefreshInventory();
        amountToUse = 0;
    }

    public void RemoveItem(int i)
    {
        for (int x = 0; x < amountToUse; x++)
        {

            if (inventory[i].amount <= 1)
            {
                inventory.Remove(inventory[i]);
                holderDescription.SetActive(false);
                amountToUse = 0;
                break;
            }
            else
            {
                inventory[i].amount--;
            }

        }
        RefreshInventory();
        amountToUse = 1;

    }

    public  void PlusButton(int i )
    {
        if(amountToUse <= inventory[i].amount -1)
        amountToUse++;
        valueToUse.text = amountToUse + "/" + inventory[i].maxAmount;
    }

    public void MoinsButton(int i)
    {
        if(amountToUse > 0)
        {
            amountToUse -= 1;
        }
        valueToUse.text = amountToUse + "/" + inventory[i].maxAmount;

    }
}
