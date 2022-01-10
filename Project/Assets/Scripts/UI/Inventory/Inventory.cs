using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;
    public bool isPause = false;
    private ThirdPersonController player;

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
                    //Debug.Log("Increased item amount: " + itemToAdd.itemname);
                    break;
                }
            }
        }
        else
        {
            characterItems.Add(itemToAdd);
            inventoryUI.AddNewItem(itemToAdd);
            //Debug.Log("Added item: " + itemToAdd.itemname);
        }
    }
    //add the item
    public void GiveItem(string itemName)
    {
        Item itemToAdd = itemDatabase.GetItem(itemName);
        int id = itemToAdd.id;
        if (CheckItemInCharacterInventory(itemToAdd) && itemToAdd.stackable)
        {
            for (int i = 0; i < characterItems.Count; i++)
            {
                if (characterItems[i].id == id)
                {
                    UIItem data = inventoryUI.uIItems[i].transform.GetComponent<UIItem>();
                    data.item.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.item.amount.ToString();
                    //Debug.Log("Increased item amount: " + itemToAdd.itemname);
                    break;
                }
            }
        }
        else
        {
            characterItems.Add(itemToAdd);
            inventoryUI.AddNewItem(itemToAdd);
            //Debug.Log("Added item: " + itemToAdd.itemname);
        }
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
            //Debug.Log("Item removed: " + itemToRemove.itemname);
        }
    }

    private void Start()
    {
        GiveItem(4);
        GiveItem(6);
        GiveItem(12);
        inventoryUI.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPause)
        { 
            Cursor.visible = true;
            isPause = true;
            player.menuing = true;
            inventoryUI.gameObject.SetActive(true);
        }
        else if((Input.GetKeyDown(KeyCode.Escape) && isPause) || player.hurtTime > 0)
        {
            Cursor.visible = false;
            player.menuing = false;
            isPause = false;
            inventoryUI.gameObject.SetActive(false);
        }
    }
}