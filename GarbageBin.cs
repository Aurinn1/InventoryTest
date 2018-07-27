using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class GarbageBin : MonoBehaviour
    {
        [SerializeField]
        private Inventory inventory;

        public void EraseFromInventory(Item itemToRemove)
        {
            inventory.inventoryItems.Remove(itemToRemove);
        }

    }
}

