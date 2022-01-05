using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//created from article
public class UIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    private Image spriteImage;
    private UIItem selectedItem;
    private Tooltip tooltip;

    private void Awake() //get the image of the slot
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null); //default at least one empty slot for testing purposes in the SlotPanel
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
    }

    public void UpdateItem(Item item) //check if the slot need to show the item
    {
        this.item = item;
        if (this.item != null) //show the icon
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = this.item.icon;
        }
        else //hide the icon
        {
            spriteImage.color = Color.clear;
        }
    }

    //drag and drop the item
    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.item != null)
        {
            if(selectedItem.item != null)
            {
                Item clone = new Item(selectedItem.item);
                //grab item and put it inside
                selectedItem.UpdateItem(this.item);
                UpdateItem(clone);
            }
            else
            {//no previous selectedItem we need to grab the clicked one and clear the slot it was on
                selectedItem.UpdateItem(this.item);
                UpdateItem(null);
            }
        }
        else if(selectedItem.item != null)
        {//where there is no item on the inventory and we have had an item selected,
         //we would like to drop it inside the inventory again
            UpdateItem(selectedItem.item);
            selectedItem.UpdateItem(null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.item != null)
        {
            tooltip.GenerateTooltip(this.item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
