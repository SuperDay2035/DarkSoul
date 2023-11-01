using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{

    [CreateAssetMenu(menuName = "Item/Weapon Item")]


    public class WeaponItem : Item
    {
        
        // Model Uchun GameObject E'lon Qilamiz
        public GameObject modelPrefabs;

        // Qurolsizmi Bool E'lon Qilamiz
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;

        // Hujum Animatsiyalari
        [Header("Attack Animations")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_2;

        // Stamina Animatsiyalari
        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier; 

    
    }
}