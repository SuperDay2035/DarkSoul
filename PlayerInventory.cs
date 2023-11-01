using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SG
{
    public class PlayerInventory : MonoBehaviour
    {
        // WeaponSlot Managerni Chaqiramiz
        WaponSlotManager weaponSlotManager;
        
        // O'ng Va Chap Qurollarni WeaponItemga O'zlashtirib E'lon qilamiz
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        // Qurollanmagan O'zgaruvchini WeaponItemga O'zlashtirib E'lon qilamiz
        public WeaponItem unarmedWeapon;


        // O'ng Va Chap Qurollar Uchun Weapon Itemdan Massiv E'lon qilamiz
        public WeaponItem[] weaponsInRightHandsSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandsSlots = new WeaponItem[1];

        // O'ng va Chap Qurollarni Aynida Damdagi Qurollarni E'lon Qilib Indexsini 0 qilamiz
        public int currentRightWeaponIndex = -1;
        public int currentLeftWeaponIndex = -1;


        // Qurol Uchun List Yaratamiz WeaponItemni olib!
        public List<WeaponItem> weaponsInvetory;

        private void Awake()
        {
            // WeaponSlotManagerni Chaqiramiz
            weaponSlotManager = GetComponentInChildren<WaponSlotManager>();
           
        }

        private void Start()
        {
            //    // O'ng va Chap Qurollarni WeaponItemga, WeaponItem Massiv O'zgaruvchilarini indexsiga current O'ng Va Chap int O'zgaruvchilarini belgilimiz
            //    rightWeapon = weaponsInRightHandsSlots[currentRightWeaponIndex];
            //    leftWeapon = weaponsInLeftHandsSlots[currentLeftWeaponIndex];


            //    // Weapon Slot Manager Ichidagi LoadWeaponni Chaqiramiz Parametriga O'ng Qo'li Uchun False Beramiz
            //    weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            //    // Weapon Slot Manager Ichidagi LoadWeaponni Chaqiramiz Parametriga Chap Qo'li Uchun True Beramiz
            //    weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);

            // O'yin Boshida 2 kala qo'lda ham Qurollar Bo'lmaydi
            rightWeapon = unarmedWeapon;
            leftWeapon = unarmedWeapon;

            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }


        #region Playerda Qurollarni Almashtirish uchun ChangeWeapon Metodi
        
        // O'ng Qo'l uchun
        public void ChangeRightWeapon()
        {
            // current O'ng Qo'ldagi indexni 1 ga tenglimiz
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
            
            // Curren O'ng Qo'ldagi Index 0 Bo'lsa va Weaponright handIndex si 0 bo'lsa va null bo'lmasa
            if(currentRightWeaponIndex == 0 && weaponsInRightHandsSlots[0] != null)
            {
                // WeaponInRightSlots Indeksini currentRightWeaponIndex qil va right Weaponga O'zlashtir
                rightWeapon = weaponsInRightHandsSlots[currentRightWeaponIndex];

                // Weapon Slot Manager Ichidagi LoadWeaponOnSlot Parametriga weaponsInRightHandsSlots yozamiz va indeksiga currentRightWeaponIndex
                // False beramiz Chuni IsLeft Booleni Right Bo'lib Turibti Left emas
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandsSlots[currentRightWeaponIndex], false);

            }
            // Agar null bo'lsa
            else if(currentRightWeaponIndex == 0 && weaponsInRightHandsSlots[0] == null)
            {
                // CurrentRightWeaponIndexni 1 qilamiz Agar Index 0 Null bo'ls 1 chi indeksga o'tadi
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }


            // Curren O'ng Qo'ldagi Index 1 Bo'lsa va Weaponright handIndex si 1 bo'lsa va null bo'lmasa
            else if(currentRightWeaponIndex == 1 && weaponsInRightHandsSlots[1] != null)
            {
                // WeaponInRightSlots Indeksini currentRightWeaponIndex qil va right Weaponga O'zlashtir
                rightWeapon = weaponsInRightHandsSlots[currentRightWeaponIndex];

                // Weapon Slot Manager Ichidagi LoadWeaponOnSlot Parametriga weaponsInRightHandsSlots yozamiz va indeksiga currentRightWeaponIndex
                // False beramiz Chuni IsLeft Booleni Right Bo'lib Turibti Left emas
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandsSlots[currentRightWeaponIndex], false);

            }
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandsSlots[1] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

              
            // Agar CurrentRightWeaponIndex katta bo'lsa weaponsIndRightHandsni Lengthidan va ayirilsa 1 ga
            if(currentRightWeaponIndex > weaponsInRightHandsSlots.Length - 1)
            {
                // CRWI ni -1 qilamiz 
                currentRightWeaponIndex = -1;
                
                // UnarmedWeaponni rigth Weaponga O'zlashtiramiz
                rightWeapon = unarmedWeapon;

                // WeaponSloManager ichidagi LoadWeaponSlot Parametriga unarmedWeapon beramiz,
                // va Buleniga false beramiz chunki Right ishlatyapmiz
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false); 
            
            }
        }

        public void ChangeLeftWeapon()
        {
            // current O'ng Qo'ldagi indexni 1 ga tenglimiz
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            // Curren O'ng Qo'ldagi Index 0 Bo'lsa va Weaponright handIndex si 0 bo'lsa va null bo'lmasa
            if (currentLeftWeaponIndex == 0 && weaponsInLeftHandsSlots[0] != null)
            {
                // WeaponInRightSlots Indeksini currentRightWeaponIndex qil va right Weaponga O'zlashtir
                leftWeapon = weaponsInLeftHandsSlots[currentLeftWeaponIndex];

                // Weapon Slot Manager Ichidagi LoadWeaponOnSlot Parametriga weaponsInRightHandsSlots yozamiz va indeksiga currentRightWeaponIndex
                // False beramiz Chuni IsLeft Booleni Right Bo'lib Turibti Left emas
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandsSlots[currentLeftWeaponIndex], true);

            }
            // Agar null bo'lsa
            else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandsSlots[0] == null)
            {
                // CurrentRightWeaponIndexni 1 qilamiz Agar Index 0 Null bo'ls 1 chi indeksga o'tadi
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }


            // Curren O'ng Qo'ldagi Index 1 Bo'lsa va Weaponright handIndex si 1 bo'lsa va null bo'lmasa
            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandsSlots[1] != null)
            {
                // WeaponInRightSlots Indeksini currentRightWeaponIndex qil va right Weaponga O'zlashtir
                leftWeapon = weaponsInLeftHandsSlots[currentLeftWeaponIndex];

                // Weapon Slot Manager Ichidagi LoadWeaponOnSlot Parametriga weaponsInRightHandsSlots yozamiz va indeksiga currentRightWeaponIndex
                // False beramiz Chuni IsLeft Booleni Right Bo'lib Turibti Left emas
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandsSlots[currentLeftWeaponIndex], true);

            }
            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandsSlots[1] == null)
            {
                // currentLeftWeaponIndexni 1 qilamiz Agar Index 0 Null bo'lsa 1 chi indeksga o'tadi
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }


            // Agar CurrentRightWeaponIndex katta bo'lsa weaponsIndRightHandsni Lengthidan va ayirilsa 1 ga
            if (currentLeftWeaponIndex > weaponsInLeftHandsSlots.Length - 1)
            {
                // CRWI ni -1 qilamiz 
                currentLeftWeaponIndex = -1;

                // UnarmedWeaponni rigth Weaponga O'zlashtiramiz
                leftWeapon = unarmedWeapon;

                // WeaponSloManager ichidagi LoadWeaponSlot Parametriga unarmedWeapon beramiz,
                // va Buleniga false beramiz chunki Right ishlatyapmiz
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);

            }
        }

        #endregion

    }
}