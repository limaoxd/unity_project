using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class SaveGame : MonoBehaviour
{
    public Inventory CharacterInventory;
    public ThirdPersonController Player;
    FileInfo Finfo;
    StreamWriter SW;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
        CharacterInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        Finfo = new FileInfo(Application.dataPath + @"/StreamingAssets/archive.json");
        SW = Finfo.CreateText();
    }

    public void Save()
    {
        SaveItems();
        SaveStats();
        SW.Close();
    }

    public void SaveItems()
    {
        //save weapon
        JsonData ItemData;
        ItemSave item;
        List<Item> Items = CharacterInventory.characterItems;

        for (int i = 0; i < Items.Count; i++)
        {
            item = new ItemSave(Items[i]);
            ItemData = JsonMapper.ToJson(item);
            SW.WriteLine(ItemData.ToString());
        }

        //save max_potion
        PotionSave potion = new PotionSave(Player.drinkMax);
        JsonData potionData = JsonMapper.ToJson(potion);
        SW.WriteLine(potionData);
    }

    public void SaveStats()
    {
        //save level
        //StatsSave stats = new StatsSave();
        //JsonData statsData = JsonMapper.ToJson(stats);
        //SW.WriteLine(statsData);

        //save spawn point

        //save boss progress
    }
}

class BossSave
{
    public string name;
    public string slug = "Boss";
    public bool conquered;

    public BossSave(string name,bool conquered)
    {
        this.name = name;
        this.conquered = conquered;
    }
}

class SpawnPointSave
{
    public string name = "SpawnPoint";
    public string slug = "SpawnPoint";
    public int scene;
    public int fire;

    public SpawnPointSave(int scene,int fire)
    {
        this.scene = scene;
        this.fire = fire;
    }
}

class StatsSave
{
    public string name = "Level";
    public string slug = "Level";
    public int Level;

    public StatsSave(int level)
    {
        Level = level;
    }
}

class PotionSave
{
    public int id = 16;
    public string name = "Potion";
    public string slug = "Potion";
    int max_drink;

    public PotionSave(int max)
    {
        max_drink = max;
    }
}

class ItemSave
{
    public int id;
    public string name;
    public string slug = "Weapon";

    public ItemSave(Item item)
    {
        this.id = item.id;
        this.name = item.itemname;
    }
}
