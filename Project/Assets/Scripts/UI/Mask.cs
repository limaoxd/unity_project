using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    private Item selectedItem;

    private void Update()
    {
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>().item;
        if (selectedItem != null)
            GetComponent<Canvas>().enabled = false;
        else
            GetComponent<Canvas>().enabled = true;
    }
}
