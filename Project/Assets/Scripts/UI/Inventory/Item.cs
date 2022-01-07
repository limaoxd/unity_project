using UnityEngine;
using System.Collections.Generic;

//create from article
public class Item
{
    public int id { get; set; }
    public string itemname { get; set; }
    public bool stackable { get; set; }
    public string slug { get; set; }
    public string description { get; set; }
    public Sprite icon;
    public int amount = 1;

    public Item(int id, string itemname, bool stackable, string slug, string description)
    {
        this.id = id;
        this.itemname = itemname;
        this.stackable = stackable;
        this.slug = slug;
        this.description = description;
        this.icon = Resources.Load<Sprite>("Items/" + itemname);
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.itemname = item.itemname;
        this.stackable = item.stackable;
        this.slug = item.slug;
        this.description = item.description;
        this.amount = item.amount;
        this.icon = Resources.Load<Sprite>("Items/" + item.itemname);
    }

    public Item()
    {
        this.id = -1;
    }
      
}
