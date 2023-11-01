using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG { 
    public class Interactable : MonoBehaviour
    {

        #region E'lonlar

        // radiusni e'lon qilamiz
        public float radius = 0.6f;

        // InteractableText E'lon qilamiz 
        public string InteractableText;
        

        #endregion


        // Gizmo Metodini Yaratamiz
        public void OnDrawGizmosSelected()
        {
            
            // Gizmoni Rangini Beramiz
            Gizmos.color = Color.blue;

            // Gizmoni Markazida Yorug'ligini yaratamiz
            Gizmos.DrawWireSphere(transform.position, radius);

        }


        // Biz  Interact Degan Metodni yaratamiz virtaul bilan(Ya'ni Metodini Turli joyda qayta ishlatish uchun);
        // Parametriga PlayerManagerni Olamiz
        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("You Interacted with an object");
        }


    }
    




}
