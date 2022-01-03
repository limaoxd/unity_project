using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//from article
public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        BuildDatabase();
    }

    void BuildDatabase()
    {
        items = new List<Item> {
                new Item(0, "Weapon",
                new Dictionary<string, int> { { "Attack", 15 } }),
                new Item(1, "Armor",
                new Dictionary<string, int> { { "Defence", 15 } }),
                new Item(2, "Stone",
                new Dictionary<string, int> { { "Defence", 10 } })
        };
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemname)
    {
        return items.Find(item => item.itemname == itemname);
    }
}
