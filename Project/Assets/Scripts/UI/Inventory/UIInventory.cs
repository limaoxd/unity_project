using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created from article
public class UIInventory : MonoBehaviour
{//to keep track of all UIItems and
 //be able to determine which ones are being visible or not, and where they are
    public List<UIItem> uIItems = new List<UIItem>();
    public GameObject slotPrefab; //spawn the slot dynamically
    public Transform slotPanel;
    public int numberOfSlots = 20;


    //spawn each slot into the panel and save them into UIItem list
    private void Awake()
    { 
        for(int i=0;i<numberOfSlots;i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            uIItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    //show / hide item
    public void UpdateSlot(int slot, Item item)
    {
        uIItems[slot].UpdateItem(item);
    }

    //add a new item, and could insert it finding the first slot with no item
    public void AddNewItem(Item item)
    {
        UpdateSlot(uIItems.FindIndex(i => i.item == null), item);
    }

    //remove item and set that slot to null
    public void RemoveItem(Item item)
    {
        UpdateSlot(uIItems.FindIndex(i => i.item == item), null);
    }
}
