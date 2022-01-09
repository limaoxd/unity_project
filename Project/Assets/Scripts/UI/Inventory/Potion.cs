using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    private Item item;
    public ItemDatabase itemDatabase;
    private Image spriteImage;
    private ThirdPersonController controller;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
        item = itemDatabase.GetItem(16);
    }

    private void Update()
    {
        transform.GetChild(0).GetComponent<Text>().text = controller.drinkLeft.ToString();
    }
}
