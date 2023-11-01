using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class QuickSlotsUI : MonoBehaviour
    {
        // Dastlab 2 ta UI Image Uchun O'ng-Chap Weapon Icon degan O'zgaruvchilarni E'lon Qilamiz
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;


        // UpdateQuickWeaponSlotsUI Metodini Ochamiz, Parametrisga Isleft booleni va WeaponItemni yozamiz

        public void UpdateQuickWeaponSlotsUI(bool isLeft, WeaponItem weapon)
        {
            // Agar Isleft false bo'lsa
            if(isLeft == false)
            {
                // Agar weapon(WeaponItem) Ichidagi itemIcon null bo'lmasa(Ya'ni Qurol Bor Bo'lsa)
                if(weapon.itemIcon != null)
                {
                    // RightWeaponIcon ni spriti, weapon(WeaponItem) ichidagi itemIcon ga Tenglimiz
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    // RightWeaponIcon ni enable qilamiz yoqamiz true qilamiz
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    // Bo'lmasa rightWeaponIcon ni spritini null qilamiz, Rasm Yo'q
                    rightWeaponIcon.sprite = null;
                    // rightWeaponIcon ni enabledini false(O'chirman, Ya'ni Image O'chiriladi) 
                    rightWeaponIcon.enabled = false;
                }


            }
            else
            {
                // Agar weapon(WeaponItem) Ichidagi itemIcon null bo'lmasa(Ya'ni Qurol Bor Bo'lsa)
                if (weapon.itemIcon != null)
                {
                    // RightWeaponIcon ni spriti, weapon(WeaponItem) ichidagi itemIcon ga Tenglimiz
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    // LeftWeaponIcon ni enable qilamiz yoqamiz true qilamiz
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    // Bo'lmasa leftWeaponIcon ni spritini null qilamiz, Rasm Yo'q
                    leftWeaponIcon.sprite = null;
                    // leftWeaponIcon ni enabledini false(O'chirman, Ya'ni Image O'chiriladi) 
                    leftWeaponIcon.enabled = false;
                }

            
            }


        }

    }
}