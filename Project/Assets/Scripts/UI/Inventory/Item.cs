using UnityEngine;
using System.Collections.Generic;


//create from article
public class Item
{
    public int id;
    public string itemname;
    public Sprite icon;

    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public Item(int id, string itemname, Dictionary<string, int> stats)
    {
        this.id = id;
        this.itemname = itemname;
        this.icon = Resources.Load<Sprite>("Items/" + itemname);
        this.stats = stats;
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.itemname = item.itemname;
        this.icon = Resources.Load<Sprite>("Items/" + item.itemname);
        this.stats = item.stats;
    }
}
