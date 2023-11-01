using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class PlayerStats : MonoBehaviour
    {

        // Health Level E'lon Qilamiz
        public int healthLevel = 10;

        // MaxHealth E'lon Qilamiz
        public int maxHealth;

        // Currenthealth E'lon qilamiz
        public int currentHealth;

        // Stamina Uchun StaminaLevel, Max Va Current Staminalarni E'lon qilamiz
        public int staminaLevel = 10;
        public int maxStamina;
        public int currentStamina;



        // HealthBar ni Chaqiramiz
        public HealthBar healthBar;

        // HealthBar ni Chaqiramiz
        public StaminaBar staminaBar;


        // AnimationHandlerni Chaqiramiz
        AnimationHandler animatorHandler;

        private void Awake()
        {
            // healthbarni Chaqiramiz
            healthBar = FindAnyObjectByType<HealthBar>();
            // Staminabarni Chaqiramiz
            staminaBar = FindAnyObjectByType<StaminaBar>();

            // AnimationHandlerni Chaqiramiz
            animatorHandler = GetComponentInChildren<AnimationHandler>();
        }


        void Start()
        {
            #region Health
            // Max Health SetMaxHealthFromHealthLevel Metodiga Tenglimiz
            maxHealth = SetMaxHealthFromHealthLevel();


            // O'yin Boshlanganida Jon To'la Bo'ladi
            currentHealth = maxHealth;

            // healthbar dagi SetMaxhealth Metodini Chaqiramiz va maxHelth O'zgaruvchisini beramiz
            healthBar.SetMaxHealth(maxHealth);

            // healthbar dagi SetCurrentHealth metodini chaqiramiz va currentHealthni O'zgaruvchisini beramiz
            healthBar.SetCurrentHealth(currentHealth);



            #endregion

            #region Stamina

            // Max Stamina SetMaxStaminaFromStaminaLevel Metodiga Tenglimiz
            maxStamina = SetMaxStaminaFromStaminaLevel();

            // O'yin Boshlanganida Stamina To'la Bo'ladi
            currentStamina = maxStamina;

            // staminabar dagi SetMaxStamina Metodini Chaqiramiz va maxStamina O'zgaruvchisini Beramiz
            staminaBar.SetMaxStamina(maxStamina);

            // staminaBar dagi SetCurrentStamina metodini chaqiramz va currentStamin O'zgaruvchisini Beramiz
            staminaBar.SetCurrentStamina(currentStamina);


            #endregion

        }

        // Healthni maxga belgilash Metodi
        private int SetMaxHealthFromHealthLevel()
        {
            // Max Health tenglimiz Health Level * 10 ga Natija 100 Bo'ladi
            maxHealth = healthLevel * 10;

            // int turida maxHealthni qaytaradi!
            return maxHealth;

        }

        // Staminani maxga belgilash Metodi
        private int SetMaxStaminaFromStaminaLevel()
        {
            // Max Stamina tenglimiz Stamina Level * 10 ga Natija 100 Bo'ladi
            maxStamina = staminaLevel* 10;

            // int turida maxHealthni qaytaradi!
            return maxStamina;

        }

        // TakeDamage Metodi

        public void TakeDamage(int damage)
        {
            // Ayni Damdagi Healthni damage ga ayiramiz
            currentHealth = currentHealth - damage;

            // healthBar dagi SetCurrentHealth metodini curreentHealth O'zgaruvchisiga ga tenglimzi
            healthBar.SetCurrentHealth(currentHealth);

            // Agar Player Damage Bo'lsa GetHit Animatsiyasi Qo'llaniladi
            animatorHandler.PlayTargetAnimation("GetHit", true);

            
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death", true);
            }
        }


        public void TakeStaminaDamage(int damage)
        {
            // Ayni Damdagi Staminani damage ga ayiramiz
            currentStamina = currentStamina - damage;

            staminaBar.SetCurrentStamina(currentStamina);

        }

    }
}