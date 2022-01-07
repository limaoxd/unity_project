using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//created from article
public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    private Image spriteImage;
    private UIItem selectedItem;
    private Tooltip tooltip;
    private Transform originalParent;

    private void Awake() //get the image of the slot
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
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
        if (this.item != null)
        {
            if (selectedItem.item != null)
            {
                Debug.Log("Swap the item");
                Item clone = new Item(selectedItem.item);
                Debug.Log("Slot: " + clone.itemname);
                Debug.Log("Clone: " + item.itemname);
                //grab item and put it inside
                if (!item.stackable)
                    transform.GetChild(0).GetComponent<Text>().text = "";
                else
                    transform.GetChild(0).GetComponent<Text>().text = item.amount.ToString();
                selectedItem.UpdateItem(this.item);
                if(clone.stackable)
                    transform.GetChild(0).GetComponent<Text>().text = clone.amount.ToString();
                else
                    transform.GetChild(0).GetComponent<Text>().text = "";
                UpdateItem(clone);
            }
            else
            {//no previous selectedItem we need to grab the clicked one and clear the slot it was on
                if (item.stackable)
                    transform.GetChild(0).GetComponent<Text>().text = "";
                selectedItem.UpdateItem(this.item);
                UpdateItem(null);
            }
        }
        else if (selectedItem.item != null)
        {//where there is no item on the inventory and we have had an item selected,
         //we would like to drop it inside the inventory again
            UpdateItem(selectedItem.item);
            if(selectedItem.item.stackable)
                transform.GetChild(0).GetComponent<Text>().text = selectedItem.item.amount.ToString();
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

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    if(item != null)
    //    {
    //        originalParent = this.transform.parent;
    //        this.transform.SetParent(this.transform.parent.parent);
    //        this.transform.position = eventData.position;
    //        GetComponent<CanvasGroup>().blocksRaycasts = false;
    //    }
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (item != null)
    //    {
    //        tooltip.gameObject.SetActive(false);
    //        this.transform.position = eventData.position;
    //    }
    //}
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    this.transform.SetParent(originalParent);
    //    this.transform.position = originalParent.transform.position;
    //    GetComponent<CanvasGroup>().blocksRaycasts = true;
    //}
}
