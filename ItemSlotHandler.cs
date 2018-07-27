using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem
{
    public class ItemSlotHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Item selectedItem;
        public Image selectedItemImage;
        private Inventory inventory;

        [SerializeField]
        private Text itemDescriptionArea;
        [SerializeField]
        private Text itemNameArea;

        public Text itemStackText;
        public int itemCurrentStack;

        private PlayerController player;

        private Transform currentParent = null;
        private Vector2 imageLocation;
        private List<RaycastResult> uiRaycastHits = new List<RaycastResult>();



        [SerializeField]
        private bool isEmptySlot;

        private void Awake()
        {
            selectedItemImage = GetComponent<Image>();
            inventory = GetComponentInParent<Inventory>();
            player = FindObjectOfType<PlayerController>();

        }


        public void Update()
        {
            if(selectedItem == null)
            {
                isEmptySlot = true;
            }
            else
            {
                isEmptySlot = false;
                itemStackText.enabled = true;
            }

            if(isEmptySlot == true)
            {
                selectedItem = null;
                selectedItemImage.sprite = null;
                itemStackText.enabled = false;

            }

        }


        //_______---------------------MOUSE DRAG----------____________________

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(isEmptySlot == false)
            {
                currentParent = transform.parent;
                transform.SetParent(transform.root);

                imageLocation = transform.position;
                selectedItemImage.raycastTarget = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(isEmptySlot == false)
            {
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isEmptySlot == false)
            {
                EventSystem.current.RaycastAll(eventData, uiRaycastHits);
                GarbageBin garbageBin = uiRaycastHits[0].gameObject.GetComponent<GarbageBin>();

                for(int i =0; i < uiRaycastHits.Count; i++)
                {
                    print(uiRaycastHits[i].gameObject + " : " + i);
                }
      
                if (garbageBin != null)
                {
                    Debug.Log(selectedItem.name + " has been put into " + uiRaycastHits[0].gameObject.name);
                    selectedItemImage.sprite = null;
                    selectedItemImage.color = new Color(1, 1, 1, 0);

                    garbageBin.EraseFromInventory(selectedItem);
                    transform.position = imageLocation;
                    transform.SetParent(currentParent);
                    selectedItemImage.raycastTarget = true;
                    selectedItem = null;
                }

                ItemSlotHandler otherItemSlotHandler = uiRaycastHits[0].gameObject.GetComponent<ItemSlotHandler>();

                if (otherItemSlotHandler != null)
                {
                    transform.position = otherItemSlotHandler.transform.position;
                    otherItemSlotHandler.transform.position = imageLocation;

                    //----The other items scripts and parent, that is going to be replaced
                    Transform theOtherObjectsParent = otherItemSlotHandler.transform.parent;
                    int theOtherItemIndex = inventory.itemSlotHandlers.IndexOf(otherItemSlotHandler);
                    print("Other item INdex " + theOtherItemIndex);

                    int thisItemIndex = inventory.itemSlotHandlers.IndexOf(this);
                    print("This item index " + thisItemIndex);

                    inventory.itemSlotHandlers.Remove(this);
                    inventory.itemSlotHandlers.Insert(theOtherItemIndex, this);

                    inventory.itemSlotHandlers.Remove(otherItemSlotHandler);
                    inventory.itemSlotHandlers.Insert(thisItemIndex, otherItemSlotHandler);

                    transform.SetParent(theOtherObjectsParent);
                    transform.localPosition = Vector3.zero;

                    otherItemSlotHandler.transform.SetParent(currentParent);
                    otherItemSlotHandler.transform.localPosition = Vector3.zero; 

                    currentParent = null;
                    selectedItemImage.raycastTarget = true;
                }

                if(garbageBin == null && otherItemSlotHandler == null)
                {
                    if (uiRaycastHits.Count == 1)
                    {
                        transform.position = imageLocation;
                        transform.SetParent(currentParent);
                        selectedItemImage.sprite = null;
                        selectedItemImage.color = new Color(1, 1, 1, 0);

                        Vector3 itemPushVector3 = player.transform.forward * 250 + new Vector3(0, 150, 0);
                        inventory.DropTheItemToTheGround(selectedItem, player.transform.position, itemPushVector3, itemCurrentStack);

                        itemCurrentStack = 0;
                        itemStackText.text = null;
                        currentParent = null;
                        selectedItem = null;
                        selectedItemImage.raycastTarget = true;
                        return;
                    }

                    transform.position = imageLocation;
                    transform.SetParent(currentParent);
                    selectedItemImage.raycastTarget = true;
                    currentParent = null;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(selectedItem != null)
            {
                if (itemNameArea != null && itemDescriptionArea != null)
                {
                    itemNameArea.text = selectedItem.GetItemName();
                    itemDescriptionArea.text = selectedItem.GetItemDescription();          
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(selectedItem != null)
            {
                if (itemNameArea != null && itemDescriptionArea != null)
                {
                    itemNameArea.text = null;
                    itemDescriptionArea.text = null;
                }
            }
 
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on " + eventData);
        }
        //_______---------------------MOUSE DRAG----------____________________


    }
}

