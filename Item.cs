using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject {

    [SerializeField]
    private string itemName;
    [SerializeField]
    private string itemDescription;
    [SerializeField]
    private Sprite itemIcon;
    [SerializeField]
    private GameObject itemObject;
    [SerializeField]
    private int maxStack;

    public Sprite GetItemIcon()
    {
        return itemIcon;
    }

    public GameObject GetItemObject()
    {
        return itemObject;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public string GetItemDescription()
    {
        return itemDescription;
    }

    public int GetMaxStack()
    {
        return maxStack;
    }

}
