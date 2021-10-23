using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestID : MonoBehaviour
{
    public int questId;

    public void ChargeQuest()
    {
        QuestManager.instance.ChargeQuest(questId);
    }
}
