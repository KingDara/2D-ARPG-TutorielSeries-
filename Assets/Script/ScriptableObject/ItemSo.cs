using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "ScriptableObject/Item", order = 1)]
public class ItemSo : ScriptableObject
{
    public string title;
    public string description;
    public Sprite icon;
    public int amount, goldToGive, amountToHeal;
    public bool isStackable;

    [System.Serializable]
    public enum Type
    {
        Quest, Comsommable, Commun
    }

    public Type type;


}
