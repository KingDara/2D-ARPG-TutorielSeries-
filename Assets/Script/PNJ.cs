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

    public QuestSO quest;

    public Sprite iconQuest1, iconQuest2;
    public SpriteRenderer questSr;

    public GameObject choice1, choice2;

    private void Start()
    {
        choice1 = manager.choice1;
        choice2 = manager.choice2;
        if (quest != null && quest.statut == QuestSO.Statut.none)
        {
            if (!quest.isEnabled)
            {
                questSr.sprite = null;
            }
            else
            {
                questSr.sprite = iconQuest1;
            }

        }
        else if (quest == null)
        {
            questSr.sprite = null;
        }
    }

    private void Update()
    {
        if (quest != null && quest.statut == QuestSO.Statut.none && quest.isEnabled)
        {
            questSr.sprite = iconQuest1;
        }
        else if (quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount < quest.amounToFind)
        {
            questSr.sprite = iconQuest2;
            questSr.color = Color.red;

        }
        else if (quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount >= quest.amounToFind)
        {
            questSr.sprite = iconQuest2;
            questSr.color = Color.yellow;
        }
        else if (quest != null && quest.statut == QuestSO.Statut.complete)
        {
            questSr.sprite = null;
        }





        if (Input.GetKeyDown(KeyCode.E) && canDial)
        {
            if(quest != null && quest.statut == QuestSO.Statut.none && quest.isEnabled)
            {
                StartDialogue(quest.sentences);
                
            }
            else if(quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount < quest.amounToFind && quest.isEnabled)
            {
                StartDialogue(quest.InProgressSentence);
                
                
            }
            else if(quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount >= quest.amounToFind && quest.isEnabled)
            {
                StartDialogue(quest.completeSentence);
                //recompense le joueur est enleve les objets déjà présent dans l'inventaire
                PlayerController.money += quest.goldToGive;
                foreach (var item in QuestManager.instance.allQuest) 
                {
                    if(item.statut == QuestSO.Statut.accepter && item.objectTofind == quest.objectTofind)
                    {
                        item.actualAmount -= quest.amounToFind;
                    }
                }
                //Actualise le statut de quête
                quest.statut = QuestSO.Statut.complete;

                if(quest != null && quest.statut == QuestSO.Statut.complete && quest.isEnabled)
                {
                    foreach (var item in QuestManager.instance.allQuest)
                    {
                        if(item.id == quest.seriesID)
                        {
                            if (quest.hasSeries && quest.thisPnj)
                            {
                                quest = item;
                                quest.isEnabled = true;
                            }
                            else if(quest.hasSeries && !quest.thisPnj)
                            {
                                item.isEnabled = true;
                            }
                            
                        }
                    }
                }
                             

            }
            else if(quest != null && quest.statut == QuestSO.Statut.complete && quest.isEnabled)
            {
               
                    StartDialogue(quest.afterQuestSentence);
            }
            else if(quest == null || !quest.isEnabled)
            {
                StartDialogue(sentences);
            }
            
            
        }
    }

    public void StartDialogue(string[] sentence)
    {
        manager.dialogueHolder.SetActive(true);
        PlayerController.instance.canMove = false;
        PlayerController.instance.canAttack = false;
        isOndial = true;
        TypingText(sentence);
        manager.continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        manager.continueButton.GetComponent<Button>().onClick.AddListener(delegate { NextLine(sentence); });
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

        if (isOndial && index == sentence.Length - 1)
        {
            if (quest != null && quest.statut == QuestSO.Statut.none && quest.isEnabled)
            {
                choice1.SetActive(true);
                choice2.SetActive(true);

                choice1.GetComponent<Button>().onClick.RemoveAllListeners();
                choice2.GetComponent<Button>().onClick.RemoveAllListeners();
                choice1.GetComponent<Button>().onClick.AddListener(delegate { Accepte(); });
                choice2.GetComponent<Button>().onClick.AddListener(delegate { Decline(); });

            }
        }


    }

    public void NextLine(string[] sentence)
    {
        manager.continueButton.SetActive(false);

        if(isOndial && index < sentence.Length -1)
        {
            index++;
            manager.textDisplay.text = "";
            TypingText(sentence);
        }
        else if(isOndial && index == sentence.Length - 1)
        {
            isOndial = false;
            index = 0;
            manager.textDisplay.text = "";
            manager.nameDisplay.text = "";
            manager.dialogueHolder.SetActive(false);

            if(quest != null && quest.statut == QuestSO.Statut.none)
            {
                choice1.SetActive(true);
                choice2.SetActive(true);
                
                choice1.GetComponent<Button>().onClick.AddListener(delegate { Accepte(); });
                choice2.GetComponent<Button>().onClick.AddListener(delegate { Decline(); });
            }

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

    public void Accepte()
    {
        quest.statut = QuestSO.Statut.accepter;
        isOndial = false;
        index = 0;
        manager.textDisplay.text = "";
        manager.nameDisplay.text = "";
        manager.dialogueHolder.SetActive(false);
        choice1.SetActive(false);
        choice2.SetActive(false);
        PlayerController.instance.canMove = true;
        PlayerController.instance.canAttack = true;
    }

    public void Decline()
    {
        quest.statut = QuestSO.Statut.none;
        isOndial = false;
        index = 0;
        manager.textDisplay.text = "";
        manager.nameDisplay.text = "";
        manager.dialogueHolder.SetActive(false);
        choice1.SetActive(false);
        choice2.SetActive(false);
        PlayerController.instance.canMove = true;
        PlayerController.instance.canAttack = true;

    }

}
