using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SG
{

    // Interactabledan meros olamiz
    public class WeaponPickUp : Interactable
    {
        
        // WeaponItemni E'lon qilamiz
        public WeaponItem weapon;


        // Interact Metodini chaqiramiz
        public override void Interact(PlayerManager playerManager)
        {
            // Base Orqali Interact Metodia Murojat qilamiz
            base.Interact(playerManager);

            // PickUpItem Metodini Chaqiramiz player Parametri Bilan
            PickUpItem(playerManager);

            Debug.Log("Interact Ishlayapti");
        }


        // Objectni Olish uchun PickUp Item Yaratamiz va Parametriga player managerni yozamiz
        public void PickUpItem(PlayerManager playerManager)
        {
            // PlayerInventory va playerLocomotionni chaqiramiz
            PlayerInventory playerInventory;
            PlayerLocomotion playerlocomotion;
             
            // AnimationHandlerni Chaqiramiz
            AnimationHandler animatorHandler;

            // PlayerManager PlayerInventory va playerLocomotionni Componentiga Ega Bo'ladi 
            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerlocomotion = playerManager.GetComponent<PlayerLocomotion>();

          

            // AnimationHandlerni Chaqiramiz, Children qilamiz Chunki AnimatorHandler Hierarchyda Playerni ichida Joylashgan 
            animatorHandler = playerManager.GetComponentInChildren<AnimationHandler>();

            // Objectni PickUp qilyotgan paytda Player harakatlanmasligi kerak
            playerlocomotion.Rigidbody.velocity = Vector3.zero;

            // Player Objectni tanlash Animatsiyani
            animatorHandler.PlayTargetAnimation("PickUpItem", true);

            if (weapon != null)
            {

                // PlayerInventory Ichidagi weaponsInventory Listiga Qo'shami weaponni
                playerInventory.weaponsInvetory.Add(weapon);

                // Player Managerdagi ItemInteractableGameObject Chaqiramiz
                // Text Componentini Childreniga Beramiz va textiga!
                // weapon(Weapon Item) dagi Item Meros Bo'lgan Skriptni ichigai itemName ni Beramiz
                playerManager.ItemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;

                // Player Managerdagi ItemInteractableGameObject Chaqiramiz
                // RawImage Componentini Childreniga Bearmiz va texture ga!
                // weapon(Weapon Item) dagi Item Meros Bo'lgan Skriptni ichigai ItemIcon ni texture ni olamiz
                playerManager.ItemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
                // PlayerManger ichidagi ItemInteractableGameObject Gameobjectnini Enabl Qilasan  Active
                playerManager.ItemInteractableGameObject.SetActive(true);
            }






            // Item Tanlanganda Itemni  Destroy qilamiz
            Destroy(gameObject);

        }


    }
}
