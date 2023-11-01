using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {

        #region E'lonlar

        // UI Managerni E'lon qilamiz
        UIManager uiManager;


        // Image o'zgaruvchisini icon uchun e'lon qilamiz
        public Image icon;

        // WeaponItemni Chaqiramiz
        WeaponItem weapon;

        // Right va Left Hand Slot Boolenlarini Chaqiramiz
        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool leftHandSlot01;
        public bool leftHandSlot02;

        #endregion


        #region Start, Awake, Update

        private void Awake()
        {
            // Ui Managerni Chaqiramiz
            uiManager = FindObjectOfType<UIManager>();
        }


        #endregion


        #region Asosiy Kodlar


        // Add Item Metodi(Qurollarni Equipmentga Qo'shadi) yozamiz WeaponItem Parametri Bilan

        public void AddItem(WeaponItem newWeapon)
        {
            // weaponni tenlimiz newWeapon Parametriga
            weapon = newWeapon;
            // Icon Ichigagi sprite ni tenglimiz weapon ichidagi itemIconga
            icon.sprite = weapon.itemIcon;
            // Icon Imageni Yoqib Qo'yamiz
            icon.enabled = true;
            // gameObjectni yoqib qo'yamiz
            gameObject.SetActive(true);
        }

        // Clear Item(Qurollarni Equipmentdan Tozalimiz) 

        public void ClearItem()
        {
            // Weaponni null qilamiz
            weapon = null;
            // Icon Sprite Ichidaginiyam!
            icon.sprite = null;
            // Icon Imageni o'chirib qo'yamiz
            icon.enabled = false;
            // gameObjectni o'chirib qo'yamiz
            gameObject.SetActive(false);
        }


        // SelectThisSlot(UI dagi Equipment Qurollar Tanlanishi) Metodini E'lon qilamiz

        public void SelectThisSlot()
        {

            // Right-Left Slotlar true bo'lsa

            if (rightHandSlot01)
            {
                // Ui Managerdagi rightHandSlot01Selected true qilamiz
                uiManager.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                // Ui Managerdagi rightHandSlot02Selected true qilamiz
                uiManager.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                // Ui Managerdagi leftHandSlot01Selected true qilamiz
                uiManager.leftHandSlot01Selected = true;
            }
            else if(leftHandSlot02)
            {
                // Ui Managerdagi leftHandSlot02Selected true qilamiz
                uiManager.leftHandSlot02Selected = true;
            }


        }


        #endregion

    }

}