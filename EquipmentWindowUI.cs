using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EquipmentWindowUI : MonoBehaviour
    {

        #region E'lonlar

        // Right Va Left Hand Slot Equipment lar uchun bool ochamiz

        // Right Uchun
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;

        // Left Uchun
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;


        // Player Inventoryni Chaqiramiz
        PlayerInventory playerInventory;
        // HandEquipmentSlotUI ni array ko'rinishida Chaqiramiz
        public HandEquipmentSlotUI[] handEquipmentSlotUI;

        #endregion


        #region Start

        private void Start()
        {
            // handEquipmentSlotUI Chaqiramiz
            handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>();
        }

        #endregion


        #region Asosiy Kodlar

        // Endi Barcha Boolenlar uchun alohida metodlar ochamiz hammasini true qilamiz

        // Right Hand Slot Uchun Metod Ochamiz
        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        // Left Hand Slot Uchun Metod Ochamiz


        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }


        #region Equipment Qurollarni Load Qilish
        
        // LoadWeaponsOnEquipmentSlotUI degan Metod Ochamiz
        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            // handEquipmentSlotUI ni length sigacha Aylanib Chiqamiz
            for (int i = 0; i < handEquipmentSlotUI.Length; i++)
            {
                // Agar handEquipmentSlotUI ni i chisi ichidagi rightHandSlot01 true bo'lsa
                if (handEquipmentSlotUI[i].rightHandSlot01)
                {
                    // handEquipmentSlotUI ni i chisiga AddItemni chaqir va Parametriga player Inventoridagi
                    // weaponsInRightHandsSlots ni 0 chisini Yoz
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandsSlots[0]);
                }
                // Agar handEquipmentSlotUI ni i chisi ichidagi rightHandSlot02 true bo'lsa
                else if (handEquipmentSlotUI[i].rightHandSlot02)
                {
                    // handEquipmentSlotUI ni i chisiga AddItemni chaqir va Parametriga player Inventoridagi
                    // weaponsInRightHandsSlots ni 1 chisini Yoz
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandsSlots[1]);
                }
                // Agar handEquipmentSlotUI ni i chisi ichidagi leftHandSlot01 true bo'lsa
                else if (handEquipmentSlotUI[i].leftHandSlot01)
                {
                    // handEquipmentSlotUI ni i chisiga AddItemni chaqir va Parametriga player Inventoridagi
                    // weaponsInLeftHandsSlots ni 0 chisini Yoz
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandsSlots[0]);
                }
                // Agar handEquipmentSlotUI ni i chisi ichidagi leftHandSlot02 true bo'lsa
                else if (handEquipmentSlotUI[i].leftHandSlot02)
                {
                    // handEquipmentSlotUI ni i chisiga AddItemni chaqir va Parametriga player Inventoridagi
                    // weaponsInLeftHandsSlots ni 1 chisini Yoz
                    handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandsSlots[1]);
                }
            }

        }


        #endregion


        #endregion



    }
}
