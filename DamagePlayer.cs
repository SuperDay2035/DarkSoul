using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class DamagePlayer : MonoBehaviour
    {

        public int damage = 25;

        // Colliderga Player Urilsa
        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerstats = other.GetComponent<PlayerStats>();

            if (playerstats != null)
            {
                playerstats.TakeDamage(damage);
            }

        }
    }
}