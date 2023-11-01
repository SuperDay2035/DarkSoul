using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class WaponSlotManager : MonoBehaviour
    {

        #region E'lonlar
        // WeaponHolderSlot uchun 2ta o'zgaruvchi ochamiz
        WeponHolderSlot leftHandSlot;
        WeponHolderSlot rightHandSlot;

        // DamageCollider uchun 2ta o'zgaruvchi ochamiz
        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        // Animator E'lon qilamiz
        Animator animator;

        // QuickSlotUI ni Chaqiramiz
        QuickSlotsUI quickSlotsUI;

        // PlayerStats ni Chaqiramiz
        PlayerStats playerStats;

        // AttackingWeapon Degan O'zgaruvchi ochami WeaponItemdan
        public WeaponItem attackingWeapon;

        #endregion

        #region Awake
        private void Awake()
        {
            // Animatorni Chaqiramiz
            animator = GetComponent<Animator>();

            // QuickSlotsUi ni Chaqiramiz FindObjectOfType bilan
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

            // PlayerStats ni Chaqiramiz Parent bilan
            playerStats = GetComponentInParent<PlayerStats>();


            // WeaponHolderSlot ni Massivga Olamiz Va Objectni Barcha Bolalariga Murojat qilamiz, Murojat Qilinglan Objectlar
            // WeaponHolderSlotga Massiv Bo'lib Yig'iladi
            WeponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeponHolderSlot>();


            // Foreach orqali WeaponHolderSlotni Massivga Olamiz
            foreach (WeponHolderSlot weaponSlot in weaponHolderSlots)
            {
                // Agar weaponSlot ni Ichidagi isLeftHandSlot true Bo'lsa
                if (weaponSlot.isLeftHandSlot)
                {
                    // weaponSlotni leftHandSlot ga O'zlashtir
                    leftHandSlot = weaponSlot;
                }
                // Agar weaponSlot ni Ichidagi isRightHandSlot true Bo'lsa
                else if (weaponSlot.isRightHandSlot)
                {
                    // weaponSlotni leftHandSlot ga O'zlashtir
                    rightHandSlot = weaponSlot;
                }
            }

        }
        #endregion


        #region O'ng Va Chap Qo'lga Qurollanish
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);

                // LoadLeftHandDamageCollider ni Chaqiramiz
                LoadLeftWeaponDamageCollider();

                // quickSlotsUI Ichidagi UpdateQuickWeaponSlotsUI metodini parametri Bilan Chaqiramiz
                // left bo'gani uchun IsLeft Boolenini true qilamiz va Weaponitemni(WeaponItemni) chaqiramiz
                quickSlotsUI.UpdateQuickWeaponSlotsUI(true, weaponItem);

                #region O'ng Qo'l Weapon Idle Animatsiyasi
                // Agar weaponItem null bo'lmasa
                if (weaponItem != null)
                {
                    // Left handIdle Animatsiyasini Yumshatamiz
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    // Left Arm Empty Animatsiyasini Yumshatib Chaqir
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }

                #endregion

            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);

                // LoadRightWeaponDamageCollider ni Chaqiramiz
                LoadRightWeaponDamageCollider();

                // quickSlotsUI Ichidagi UpdateQuickWeaponSlotsUI metodini parametri Bilan Chaqiramiz
                // right bo'gani uchun IsLeft Boolenini false qilamiz va Weaponitemni(WeaponItemni) chaqiramiz
                quickSlotsUI.UpdateQuickWeaponSlotsUI(false, weaponItem);


                #region Chap Qo'l Weapon Idle Animatsiyasi
                // Agar weaponItem null bo'lmasa
                if (weaponItem != null)
                {
                    // Right handIdle Animatsiyasini Yumshatib Chaqir
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }
                else
                {
                    // Right Arm Empty Animatsiyasini Yumshatib Chaqir
                    animator.CrossFade("Right Arm Empty", 0.2f);
                }
                #endregion
            }
        }
        #endregion


        #region O'n va Chap Weapon Damage Colliderlar Metodi
        public void LoadRightWeaponDamageCollider()
        {
            // Bu Degani leftHandDamageCollider Damage Colliderni, leftHandSlot(WeaponSlotKlassi Ichidagi) currentWeaponModeliga 
            // DamageCollider klasini ulimiz
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        // Huddi Shuni Leftga Ham Qilamiz
        public void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        #endregion


        #region Damage ColliderlarYoqish Va O'chirish

        // Damage Colliderlani Yoqish O'ng Va Chap uchun
        public void OpenRightDamageCollider()
        {
            // Biz Hozr DamageCollideni Yoqyapmiz Va O'ng Qo'lga O'zlashtiryapmiz
            rightHandDamageCollider.EnableDamageCollider();
        }
        public void OpenLeftDamageCollider()
        {
            // Biz Hozr DamageCollideni Yoqyapmiz Va O'ng Qo'lga O'zlashtiryapmiz
            leftHandDamageCollider.EnableDamageCollider();
        }

        // Damage Colliderlarni O'chirish O'ng va Chap uchun
        public void CloseRightDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void CloseLeftDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }


        #endregion


        #region Qurol uchun Stamina Drain
        // DrainStaminaLightAttack degan Metod Ochamiz

        public void DrainStaminaLightAttack()
        {
            // PlayerStats Ichidagi TakeStaminaDamageni Chaqiramiz va Parametrdiga WeaponItem ichidagi base va attackMultplierni Ko'payitamiz RoundToInt
            // orqali qiymatlarni intga aylantiramiz
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }


        // DrainStaminaHeavytAttack degan Metod Ochamiz

        public void DrainStaminaHevyAttack()
        {
            // PlayerStats Ichidagi TakeStaminaDamageni Chaqiramiz va Parametrdiga WeaponItem ichidagi base va heavyAttackMultiplierni Ko'payitamiz RoundToInt
            // orqali qiymatlarni intga aylantiramiz
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

        #endregion
    }
}
