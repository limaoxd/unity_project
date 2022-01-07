using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// created from video, edited from article
public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;
    private bool isPause = false;

    //add the item
    public void GiveItem(int id)
    {
        Item itemToAdd = itemDatabase.GetItem(id);
        if (CheckItemInCharacterInventory(itemToAdd) && itemToAdd.stackable)
        {
            for (int i = 0; i < characterItems.Count; i++)
            {
                if(characterItems[i].id == id)
                {
                    UIItem data = inventoryUI.uIItems[i].transform.GetComponent<UIItem>();
                    data.item.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.item.amount.ToString();
                    Debug.Log("Increased item amount: " + itemToAdd.itemname);
                    break;
                }
            }
        }
        else
        {
            characterItems.Add(itemToAdd);
            inventoryUI.AddNewItem(itemToAdd);
            Debug.Log("Added item: " + itemToAdd.itemname);
        }
    }
    //add the item
    public void GiveItem(string itemName)
    {
        Item itemToAdd = itemDatabase.GetItem(itemName);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.itemname);
    }

    //see if the player's inventory holds the item
    public Item CheckForItem(int id)
    {
        return characterItems.Find(item => item.id == id);
    }
    public bool CheckItemInCharacterInventory(Item item)
    {
        for(int i=0;i<characterItems.Count;i++)
        {
            if (characterItems[i].id == item.id)
                return true;
        }
        return false;
    }

    //remove player inventory's item
    public void RemoveItem(int id)
    {
        Item itemToRemove = CheckForItem(id);
        if(itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Item removed: " + itemToRemove.itemname);
        }
    }

    private void Start()
    {
        GiveItem(0);
        GiveItem(2);
        GiveItem(1);
        GiveItem(2);
        inventoryUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
            {
                Time.timeScale = 0;
                isPause = true;
            }
            else
            {
                Time.timeScale = 1;
                isPause = false;
            }
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        }
    }
}
