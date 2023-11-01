using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class DamageCollider : MonoBehaviour
    {

        // Damager Collider E'lon Qilamiz
        Collider damageCollider;

        // Ayni Damdagi Damage
        public int currentWeaponDamage = 25;


        private void Awake()
        {
            // damage Colliderni chaqiramiz
            damageCollider = GetComponent<Collider>();

            // damage Collidern GameObjectni Active qilamiz
            damageCollider.gameObject.SetActive(true);

            // damage Colliderni Triggerini true Qilamiz
            damageCollider.isTrigger = true;
        
            // damage colliderni enable ni false qilamiz
            damageCollider.enabled = false;
        }


        // 2 ta Enable Va Disable Metodlarn Yaratamiz Damage Uchun
        public void EnableDamageCollider()
        {
            // DamageColliderni enabled  ni true qilamiz Yoqiladi
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            // DamageColliderni enable ni false qilamiz O'chiradi
            damageCollider.enabled = false;
        }



        // OnTrigger Ochamiz
        private void OnTriggerEnter(Collider collision)
        {

            // Agar Player Tagiga Teng Bo'lsa
            if (collision.tag == "Player")
            {
                // PlayerStatsni collision bilan ulab chaqir
                PlayerStats playerStast = collision.GetComponent<PlayerStats>();
        
                // PlayerStats Null Bo'lmasa
                if (playerStast != null)
                {
                    // PlayerStatsdagi TakeDamage Metodini Chaqiramiz Va currentWeaponDamage parametrga
                    // Joylashtiramiz
                    playerStast.TakeDamage(currentWeaponDamage);
                }
            }


            // Agar Enemy Tagiga Teng Bo'lsa
            if (collision.tag == "Enemy")
            {
                // EnemyStats collision bilan ulab chaqir
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

                // EnemyStats Null Bo'lmasa
                if (enemyStats != null)
                {
                    // EnemyStats TakeDamage Metodini Chaqiramiz Va currentWeaponDamage parametrga
                    // Joylashtiramiz
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }


        }



    }
}
