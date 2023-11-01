using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{

    #region E'lonlar

    // PlayerInventory ni Chaqieamiz
    PlayerInventory playerInventory;

    // WeaponSlotManager ni Chaqiramiz
    WaponSlotManager weaponSlotManager;


    // UI Manager ni Chaqiramiz
    UIManager uiManager;

    

    // Image turida icon e'lon qilamiz
    public Image Icon;

    // Weapon Item ni chaqiramiz
    WeaponItem item;



    #endregion

    #region Start, Awake

    private void Awake()
    {
        // PlayerInventoryni FindObjectOfType orqali Chaqiraman
        playerInventory = FindObjectOfType<PlayerInventory>();

        // WeaponSlotManager FindObjectOfType orqali Chaqiraman
        weaponSlotManager = FindObjectOfType<WaponSlotManager>();  

        // UI Manager FindObjectOfType orqali Chaqiramiz
        uiManager = FindObjectOfType<UIManager>();
    }

    #endregion


    #region Asosiy Kodlar

    // Qurol Qo'shish Uchun AddItem Metodini yozamiz newitem(WeaponItemdan) Parametri bilan


    public void AddItem(WeaponItem newitem)
    {
        // item teng bo'ladi newitemga
        item = newitem;
        // icon.sprite orqali itemIconni chaqiramiz
        Icon.sprite = item.itemIcon;
        // iconni yoqamiz
        Icon.enabled = true;
        // gameobject Componentini active qilamiz
        gameObject.SetActive(true);




    }

    // Qurol O'chirish Uchun ClearItem Metodini yozamiz
    public void ClearInventorySlot()
    {
        // item teng bo'ladi nullga
        item = null;
        // icon.sprite null bo'ladi
        Icon.sprite = null;
        // iconni o'chiramiz
        Icon.enabled = false;
        // gameobject Componentini deactive qilamiz
        gameObject.SetActive(false);


    }


    // EquipmentThisSlot Metodni(Tanlangan Qurolni Olish uchun) Ochamiz
    public void EquipThisitem()
    {
        // Agar UiManager ichidagi rightHandSlot01Selected true Bo'lsa
        if (uiManager.rightHandSlot01Selected)
        {
            // playerInventory ichidagi WeponInventoryga, playerInventory ichidagi weaponsInRightHandsSlots(O'ng Qo'ldagi Qurol)
            // massivi ichidagi 0 chi qurolni qo'shamiz
            playerInventory.weaponsInvetory.Add(playerInventory.weaponsInRightHandsSlots[0]);

            // playerInventory ichidagi weaponsInRightHandsSlots 0 chisini itemga teng Bo'ladi
            playerInventory.weaponsInRightHandsSlots[0] = item;

            // playerInventory ichidagi weaponsInvetory ni itemini(Qurolni) O'chirib Tashlaymiz
            playerInventory.weaponsInvetory.Remove(item);
        }
        // Agar UiManager ichidagi rightHandSlot02Selected true Bo'lsa
        else if (uiManager.rightHandSlot02Selected)
        {
            // playerInventory ichidagi WeponInventoryga, playerInventory ichidagi weaponsInRightHandsSlots(O'ng Qo'ldagi Qurol)
            // massivi ichidagi 1 chi qurolni qo'shamiz
            playerInventory.weaponsInvetory.Add(playerInventory.weaponsInRightHandsSlots[1]);

            // playerInventory ichidagi weaponsInRightHandsSlots 1 chisini itemga teng Bo'ladi
            playerInventory.weaponsInRightHandsSlots[1] = item;

            // playerInventory ichidagi weaponsInvetory ni itemini(Qurolni) O'chirib Tashlaymiz
            playerInventory.weaponsInvetory.Remove(item);
        }
        // Agar UiManager ichidagi leftHandSlot01Selected true Bo'lsa
        else if (uiManager.leftHandSlot01Selected)
        {
            // playerInventory ichidagi WeponInventoryga, playerInventory ichidagi weaponsInLeftHandsSlots(Chap Qo'ldagi Qurol)
            // massivi ichidagi 0 chi qurolni qo'shamiz
            playerInventory.weaponsInvetory.Add(playerInventory.weaponsInLeftHandsSlots[0]);

            // playerInventory ichidagi weaponsInLeftHandsSlots 0 chisini itemga teng Bo'ladi
            playerInventory.weaponsInLeftHandsSlots[0] = item;

            // playerInventory ichidagi weaponsInvetory ni itemini(Qurolni) O'chirib Tashlaymiz
            playerInventory.weaponsInvetory.Remove(item);

        }
        // Agar UiManager ichidagi leftHandSlot02Selected true Bo'lsa
        else if (uiManager.leftHandSlot02Selected)
        {
            // playerInventory ichidagi WeponInventoryga, playerInventory ichidagi weaponsInLeftHandsSlots(Chap Qo'ldagi Qurol)
            // massivi ichidagi 1 chi qurolni qo'shamiz
            playerInventory.weaponsInvetory.Add(playerInventory.weaponsInLeftHandsSlots[1]);

            // playerInventory ichidagi weaponsInLeftHandsSlots 1 chisini itemga teng Bo'ladi
            playerInventory.weaponsInLeftHandsSlots[1] = item;

            // playerInventory ichidagi weaponsInvetory ni itemini(Qurolni) O'chirib Tashlaymiz
            playerInventory.weaponsInvetory.Remove(item);


        }
        else
        {
            // Return Qilamiz
            return;
        }



        // playerInventory ichidagi rightWeapon teng Bo'
        // playerInventory ichidagi weaponsInRightHandsSlots(O'ng Qo'l) ni playerInventory.currentRightWeaponIndex siga
        playerInventory.rightWeapon = playerInventory.weaponsInRightHandsSlots[playerInventory.currentRightWeaponIndex];


        // playerInventory ichidagi rightWeapon teng Bo'
        // playerInventory ichidagi weaponsInRightHandsSlots(Chap Qo'l) ni playerInventory.currentLeftWeaponIndex siga
        playerInventory.leftWeapon = playerInventory.weaponsInRightHandsSlots[playerInventory.currentLeftWeaponIndex];


        // Qurollonishlar
        weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

        uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        uiManager.ResetAllSelectedSlots();

    }


    #endregion


}
