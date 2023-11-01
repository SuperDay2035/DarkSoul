using UnityEngine;

namespace SG
{
    public class UIManager : MonoBehaviour
    {

        #region E'lonlar


        // PlayerInventory ni chaqiramiz
        public PlayerInventory playerInventory;

        // EquipmentWindowUi ni Chaqiramiz
        public EquipmentWindowUI equipmentWindowUI;


        [Header("UI Windows")]
        // hudWindow degan gameobject ochamiz
        public GameObject hudWindow;
        // selectWindow Degan GameObject Ochamiz
        public GameObject selectWindow;

        // eqipmentScreenWindow degan GameObject E'lon qilamiz
        public GameObject equipmentScreenWindow;

        // weaponInventoryWindow Degan GameObject Ochaman
        public GameObject weaponInventoryWindow;



        [Header("Equipment Window oynasidagi Qurollar")]

        // Right-Left Qurollar Tanlanganda!
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;



        [Header("Weapon Inventory")]
        // weaponIntentorySlotPrefabs degan gameObject E'lon qilamiz
        public GameObject weaponInventorySlotPrefabs;


        // weaponIntentorySlotParent degan Transform E'lon qilamiz 
        public Transform weaponInventorySlotParent;
    
        // weaponInventorySlot classini arrayga olamiz 
        WeaponInventorySlot[] weaponInventorySlot;

        #endregion



        #region Asosiy Kodlar


        #region Awake, Start

        private void Awake()
        {
           
        }

        public void Start()
        {
            // weaponInventorySlot teng bo'ladi weaponInventorySlotParent ga weaponInventorySlot compnentini chaqirish 
            weaponInventorySlot = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();

            // equipmentWindowUI ichigai LoadWeaponsOnEquipmentScreen Metodini Chaqiramiz, playerInventory Parametri Bilan
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }



        #endregion

        // UpdateUI metodini ochamiz

        public void UpdateUI ()
        {
            #region Qurol Inventori

            // WeaponInventorySlot arrayini for orqali aylanib chiqamiz
            for (int i = 0; i < weaponInventorySlot.Length; i++)
            {
                // Agar i PlayerInventorydagi weaponsInventorydan (weaponsInventorydagi barcha qurollarning soni) kichik bo'lsa
                if (i < playerInventory.weaponsInvetory.Count)
                {
                    // Agar weaponInventorySlot.Length PlayerInventorydagi weaponsInventorydan (weaponsInventorydagi barcha qurollarning soni) kichik bo'lsa
                    if (weaponInventorySlot.Length < playerInventory.weaponsInvetory.Count)
                    {
                        // Instantiate orqali weaponInventorySlotPrefabs GameObjectning nusxasini olamiz
                        // weaponInventorySlotParent (Transform) ga joylashish kerak bo'lgan pozitsiyani kiritamiz
                        Instantiate(weaponInventorySlotPrefabs, weaponInventorySlotParent);

                        // weaponInventorySlot ni weaponInventorySlotParent dan olingan barcha elementlarga tenglashtiramiz
                        weaponInventorySlot = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    // weaponInventorySlot ning i-indexiga AddItem metodini chaqiramiz va parametr sifatida
                    // playerInventorydagi weaponsInventoryning i-indexidagi elementini beramiz
                    weaponInventorySlot[i].AddItem(playerInventory.weaponsInvetory[i]);

                    Debug.Log(playerInventory.weaponsInvetory.Count);
                }
                else
                {
                    // weaponInventorySlot ning i-indexini tozalash uchun ClearInventorySlot metodini chaqiramiz
                    weaponInventorySlot[i].ClearInventorySlot();
                }
            }



            #endregion


        }



        // OpenSelectWindow Degan Metod Ochamiz
        public void OpenSelectWindow()
        {
            // selectWindow GameObjetni Active qilamiz truega, Bu Inventoryni ochadi!
            selectWindow.SetActive(true);
        }

        // CloseSelectWindow Degan Metod Ochamiz
        public void CloseSelectWindow()
        {
            // selectWindow GameObjetni Active qilamiz falsega, Bu Inventoryni yopadi
            selectWindow.SetActive(false);
        }

        // CloseAllInventoryWindows(Barcha Oynalar Yopiladi) degan Metod ochaman

        public void CloseAllInventoryWindows()
        {
            // Equipment Slotdagi Barcha Qurollarni Reset Qilamiz
            ResetAllSelectedSlots();

            // Weapon Inventory Windowni O'chiraman
            weaponInventoryWindow.SetActive(false);

            // equipmentScreenWindow O'chiramiz
            equipmentScreenWindow.SetActive(false);
        }

        #endregion


        // Reset Slot(Qurollarni Reset Qilish Uchun) Metodini ochamiz
        public void ResetAllSelectedSlots()
        {
            // Barcha Right-Left Slotlarni Reset Bo'lishi Uchun False Qilamiz
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }


        ///
    }
}
