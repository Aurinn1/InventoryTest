using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        private ItemPickUpPoint itemPickUpPoint;

        public List<Item> inventoryItems = new List<Item>();
        public Image[] inventoryItemImages;

        public List<ItemSlotHandler> itemSlotHandlers =  new List<ItemSlotHandler>();



        void Start()
        {
            inventoryItemImages = GetComponentsInChildren<Image>();
            ItemSlotHandlerCompiler();
            AttachItemsAndImagesToSlots();
        }


        public void ItemSlotHandlerCompiler()
        {
            foreach(ItemSlotHandler itemSlotHandler in GetComponentsInChildren<ItemSlotHandler>())
            {
                itemSlotHandlers.Add(itemSlotHandler);
            }
        }

        public void AttachItemsAndImagesToSlots()
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItemImages[i].sprite == null && inventoryItems[i] != null)
                {

                    itemSlotHandlers[i].selectedItem = inventoryItems[i];
                    itemSlotHandlers[i].itemCurrentStack = 1;
                    itemSlotHandlers[i].selectedItemImage.sprite = inventoryItems[i].GetItemIcon();
                    itemSlotHandlers[i].selectedItemImage.color = new Color(1, 1, 1, 1);
                }
            }
        }

        public void AddTheNewItemToInventory(Item newItemToAdd, int newItemToAddStack)
        {
            inventoryItems.Add(newItemToAdd);

            for (int i = 0; i < itemSlotHandlers.Count; i++)
            {
                if (itemSlotHandlers[i].selectedItem == null)
                {

                        itemSlotHandlers[i].selectedItem = newItemToAdd;
                        itemSlotHandlers[i].selectedItemImage.sprite = newItemToAdd.GetItemIcon();
                        itemSlotHandlers[i].itemCurrentStack = newItemToAddStack;

                        if(itemSlotHandlers[i].itemCurrentStack > 1)
                        {
                            itemSlotHandlers[i].itemStackText.text = newItemToAddStack.ToString();
                        }
                        else
                        {
                            itemSlotHandlers[i].itemStackText.text = null;
                        }

                        itemSlotHandlers[i].selectedItemImage.color = new Color(1, 1, 1, 1);
                        return;
                    

                }

                if(itemSlotHandlers[i].selectedItem == newItemToAdd && itemSlotHandlers[i].itemCurrentStack < newItemToAdd.GetMaxStack() )
                {
                    itemSlotHandlers[i].itemCurrentStack += 1;
                    itemSlotHandlers[i].itemStackText.text = itemSlotHandlers[i].itemCurrentStack.ToString();
                    return;
                }

                
            } 
            
        }

        public void DropTheItemToTheGround(Item itemToDrop, Vector3 dropPosition, Vector3 localForceToPush, int droppedStack)
        {
            GameObject itemToDropObject = Instantiate(itemToDrop.GetItemObject(), dropPosition, Quaternion.identity);
            inventoryItems.Remove(itemToDrop);
            Rigidbody itemRB = itemToDropObject.GetComponent<Rigidbody>();
            itemRB.AddForce(localForceToPush);

            StartCoroutine(AttachItemPickUpPointScriptRoutine(itemToDropObject, itemToDrop, droppedStack));

            itemToDropObject.name = itemToDrop + "Pick Up Point";
        }

        IEnumerator AttachItemPickUpPointScriptRoutine(GameObject localItemToDropObject, Item localItemToDrop, int localItemStackSize)
        {
            yield return new WaitForSeconds(0.5f);
            ItemPickUpPoint itemPickUpPoint = localItemToDropObject.AddComponent<ItemPickUpPoint>();
            itemPickUpPoint.itemToCollect = localItemToDrop;
            itemPickUpPoint.itemStackSize = localItemStackSize;
        }

    }
}

