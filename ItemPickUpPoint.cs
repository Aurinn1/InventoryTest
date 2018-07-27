using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ItemPickUpPoint : MonoBehaviour
    {

        public Item itemToCollect;

        public int itemStackSize = 1;



        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {

            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            Inventory inventory = FindObjectOfType<Inventory>();

            if (player != null && inventory != null)
            {
                inventory.AddTheNewItemToInventory(itemToCollect, itemStackSize);
                Destroy(gameObject);
            }

            if (other.gameObject.GetComponent<TerrainCollider>() != null)
            {
                Destroy(GetComponent<Rigidbody>());
            }

        }




    }
}

