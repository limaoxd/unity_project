using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


//from article
public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    private JsonData itemdata;

    private void Awake()
    {
        itemdata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        BuildDatabase();
    }

    void BuildDatabase()
    {
        for(int i=0;i<itemdata.Count;i++)
        {
            items.Add(new Item((int)itemdata[i]["id"], itemdata[i]["itemname"].ToString(),
                      (bool)itemdata[i]["stackable"], itemdata[i]["slug"].ToString(),
                      itemdata[i]["description"].ToString()));
        }
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
