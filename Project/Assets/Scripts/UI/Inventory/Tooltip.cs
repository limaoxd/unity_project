using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Text tooltipText;

    void Start()
    {
        tooltipText = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    //check if there are stats to read inside the item
    public void GenerateTooltip(Item item)
    {
        string tooltip = string.Format("<b>{0}</b>\n{1}", item.itemname, item.description);
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }
}
