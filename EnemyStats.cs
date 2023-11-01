using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class EnemyStats : MonoBehaviour
    {

        // Health Level E'lon Qilamiz
        public int healthLevel = 10;

        // MaxHealth E'lon Qilamiz
        public int maxHealth;

        // Currenthealth E'lon qilamiz
        public int currentHealth;

  


        // Animator Chaqiramiz
        Animator animator;

        private void Awake()
        {
            // Animator Chaqiramiz
            animator = GetComponentInChildren<Animator>();
        }


        void Start()
        {
            // Max Health SetMaxHealthFromHealthLevel Metodiga Tenglimiz
            maxHealth = SetMaxHealthFromHealthLevel();

            // O'yin Boshlanganida Jon To'la Bo'ladi
            currentHealth = maxHealth;

        }

        // Healthni maxga belgilash Metodi
        private int SetMaxHealthFromHealthLevel()
        {
            // Max Health tenglimiz Health Level * 10 ga Natija 100 Bo'ladi
            maxHealth = healthLevel * 10;

            // int turida maxHealthni qaytaradi!
            return maxHealth;

        }


        // TakeDamage Metodi

        public void TakeDamage(int damage)
        {
            // Ayni Damdagi Healthni damage ga ayiramiz
            currentHealth = currentHealth - damage;

            // Agar Player Damage Bo'lsa GetHit Animatsiyasi Qo'llaniladi
            animator.Play("GetHit");

            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death");
            }
        }




    }
}