using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SG
{
    public class HealthBar : MonoBehaviour
    {
        // Slider E'lon qilamiz
        public Slider slider;

       
        
        
        
        // Max Health Metodi

        public void SetMaxHealth(int maxValue)
        {
            // Slider maxValue sini maxValue Parametriga tenglimiz
            slider.maxValue = maxValue;

            // Slider value sini maxValuega Parametriga tenglimiz
            slider.value = maxValue;
        }


        // Ayni Damdagi Health Metodi

        public void SetCurrentHealth(int currentHealth)
        {
            // Slider value sini healthga tenglimiz
            slider.value = currentHealth;
        }





        
    }
}