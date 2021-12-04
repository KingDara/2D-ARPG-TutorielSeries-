using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public GameObject dialogueHolder, continueButton, choice1, choice2;
    public TextMeshProUGUI nameDisplay, textDisplay, moneyText;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //update la money du joueur
        moneyText.text = PlayerController.money.ToString();
    }
}
