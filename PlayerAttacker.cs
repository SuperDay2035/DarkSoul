using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SG { 
    public class PlayerAttacker : MonoBehaviour
    {
        #region E'lonlar
        
        // AnimationHandlerni E'lon Qilamiz
        AnimationHandler animatorHandler;

        // InputHadlerni E'lon Qilamiz
        InputHandler inputHandler;

        // WeaponSlotManagerni E'lon Qilamiz
        WaponSlotManager weaponSlotManager;

        // Last Attack String E'lon qilamiz
        public string lastAttack;




        #endregion


        #region Awake
        private void Awake()
        {
            // Animator Handlerni Chaqiramiz
            animatorHandler = GetComponentInChildren<AnimationHandler>();

            // WeaponSlotManagerni Chaqiramiz
            weaponSlotManager = GetComponentInChildren<WaponSlotManager>();
            // InputHandlerni Chaqiramiz
            inputHandler = GetComponent<InputHandler>();
        }
        #endregion

        #region Hujum Metodlari
        // Hujum Qilish Metodi
        public void HandleLightAttack(WeaponItem weaponsItem)
        {
            // Agar WeaponsItem null Bo'lmasa
            if(weaponsItem != null)
            {
                // AnimatorHandlerga OH_Light_attackni Ulimiz PlayTargetAnimation Orqali
                animatorHandler.PlayTargetAnimation(weaponsItem.OH_Light_Attack_1, true);

                // WeaponItem ichidagi OH_Light_Attack_1 ni lastAttackga O'zlashtgiramiz
                lastAttack = weaponsItem.OH_Light_Attack_1;
              
                //WeaponSlot Manager Ichidagi attackingWeaponni tenglimizi weaponItemga
                weaponSlotManager.attackingWeapon = weaponsItem;
            
            }
        }

        // Og'ir Uslubda Hujum Qilish Metodi

        public void HandleHeavyAttack(WeaponItem weaponsItem)
        {
            // Agar WeaponsItem null Bo'lmasa
            if (weaponsItem != null)
            {
                // AnimatorHandlerga OH_Heavy_Attack_2 Ulimiz PlayTargetAnimation Orqali
                animatorHandler.PlayTargetAnimation(weaponsItem.OH_Heavy_Attack_2, true);
               
                // WeaponItem ichidagi OH_Light_Attack_1 ni lastAttackga O'zlashtgiramiz
                lastAttack = weaponsItem.OH_Light_Attack_1;
 
                //WeaponSlot Manager Ichidagi attackingWeaponni tenglimizi weaponItemga
                weaponSlotManager.attackingWeapon = weaponsItem;
            }
        }

        // Combo Hujum Metodi
        public void HandleComboAttack(WeaponItem weapon)
        {

            // Agar InputHandlerdagi ComboFlag true bo'lsa
            if (inputHandler.comboFlag)
            {
                // Animatorhandlerdagi anim dagi canDoCombo Boolenini false qilamiz
                animatorHandler.anim.SetBool("canDoCombo", false);

                // Agar last Attack  WeaponItem ichidagi Hujumga teng Bo'lsa
                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    // Og'ir Hujum Qilish Animatsiyasini Qo'llaymiz
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }

            }

                

        }

        #endregion

    }
}