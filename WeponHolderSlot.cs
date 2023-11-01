using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class WeponHolderSlot : MonoBehaviour
    {
        public Transform ParentOverride;
        // Qurol Chap Qo'ldamiz Bool E'lon Qilamiz
        public bool isLeftHandSlot;
        // Qurol O'ng Qo'ldamiz Bool E'lon Qilamiz
        public bool isRightHandSlot;

        // Ayni Damdagi Qurol Modeli GameObjecti E'lon qilamiz
        public GameObject currentWeaponModel;


        // Weaponni Tushirish
        public void UndloadWeapon()
        {
            // Agar Ayni damdagi Wepon Null Bo'lmasa
            if (currentWeaponModel != null)
            {
                // CurrentWeponModelni O'chirib Qo'yamiz
                currentWeaponModel.SetActive(false);
            }
        }


        // Qurolni tushirish va yo'q qilish
        public void UnloadWeaponAndDestroy()
        {
            // Agara CurrentWeapon Null Bo'lmasa!
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }


        // Buyerga Parametr Yozamiz, WeponItem Scriptni weaponItem qilib Parametrs Sifatida O'zlashtiramiz
        public void LoadWeaponModel(WeaponItem weaponItem)
        {

            UnloadWeaponAndDestroy();

            // Agar weaponItem null bo'lsa
            if (weaponItem == null)
            {
                 // CurrentWeaponni O'chirib Qo'yadi
                 UndloadWeapon();
            }

            // Model Game Objectga weaponItem ichidagi modelPrefabsdan Nusxa olamiz 
            GameObject model = Instantiate(weaponItem.modelPrefabs);


            // Agar Model Null ga Teng Bo'lmasa
            if(model != null)
            {
                // Agar Model Null ga Teng Bo'lmasa
                if (ParentOverride != null)
                {
                    model.transform.parent = ParentOverride;
                }
                {
                    model.transform.parent = transform;
                }

                // Modalni Local Positionini Vector(0,0,0) Qilamiz
                model.transform.localPosition = Vector3.zero;
                // Modalni localRotationni Rotatsiya Bo'lmaydigon Qilamiz
                model.transform.localRotation = Quaternion.identity;
                // Modelni LocalScale ni Vector3.one ga tenglimiz
                model.transform.localScale = Vector3.one;

            }

            currentWeaponModel = model;

        }
    
    }
}