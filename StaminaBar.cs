using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class StaminaBar : MonoBehaviour
    {
        // Slider E'lon qilamiz
        public Slider slider;

        // Max Health Metodi

        public void SetMaxStamina(int maxStamina)
        {
            // Slider maxValue sini maxValue Parametriga tenglimiz
            slider.maxValue = maxStamina;

            // Slider value sini maxValuega Parametriga tenglimiz
            slider.value = maxStamina;
        }


        // Ayni Damdagi Health Metodi

        public void SetCurrentStamina(int currentStamina)
        {
            // Slider value sini healthga tenglimiz
            slider.value = currentStamina;
        }

    }
}
