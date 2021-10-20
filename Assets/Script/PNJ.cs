using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PNJ : MonoBehaviour
{
    [SerializeField]
    string[] sentences;
    [SerializeField]
    string characterName;
    int index;
    bool isOndial, canDial;

    HUDManager manager => HUDManager.instance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canDial)
        {
            StartDialogue();
            manager.continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
            manager.continueButton.GetComponent<Button>().onClick.AddListener(delegate { NextLine(); });
        }
    }

    public void StartDialogue()
    {
        manager.dialogueHolder.SetActive(true);
        PlayerController.instance.canMove = false;
        PlayerController.instance.canAttack = false;
        isOndial = true;
        TypingText(sentences);
    }


    void TypingText(string[] sentence)
    {
        manager.nameDisplay.text = "";
        manager.textDisplay.text = "";

        manager.nameDisplay.text = characterName;
        manager.textDisplay.text = sentence[index];

        if (manager.textDisplay.text == sentence[index])
        {
            manager.continueButton.SetActive(true);
        }
    }

    public void NextLine()
    {
        manager.continueButton.SetActive(false);

        if(isOndial && index < sentences.Length - 1)
        {
            index++;
            manager.textDisplay.text = "";
            TypingText(sentences);
        }
        else if(isOndial && index == sentences.Length - 1)
        {
            isOndial = false;
            index = 0;
            manager.textDisplay.text = "";
            manager.nameDisplay.text = "";
            manager.dialogueHolder.SetActive(false);

            PlayerController.instance.canMove = true;
            PlayerController.instance.canAttack = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canDial = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canDial = false;
        }
    }


}
