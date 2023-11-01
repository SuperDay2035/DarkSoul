using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SG
{
    public class AnimationHandler : MonoBehaviour
    {
        // Eslatma V/H (Ma'nosi Vertical/Horizontal Shunchaki Qisqacha Yozilgan!)

        #region Animation Uchun E'lonlar

        // PlayerManagerni Chaqiramiz
        PlayerManager playerManager;

        // Animatsiyani Chaqiramiz
        public Animator anim;

        // InputHandlerni Chaqiramiz
        InputHandler inputHandler;

        // PlayerLocomotionni Chaqiramiz
        PlayerLocomotion playerLocomotion;

        // Animatsiya Uchun Vertical/Horizontal Yaratamiz

        int vertical;
        int horizontal;

        // Rotatsiya Bo'ldimi Yoki Yo'q Aniqlash uchun Bool Yaratamiz
        public bool canRotate;


        #endregion


        #region Chaqirishlar

        public void Initialize()
        {
            // PlayerManagerni Chaqiramiz
            playerManager = GetComponentInParent<PlayerManager>();

            // Animatorni Chaqiramiz
            anim = GetComponent<Animator>();

            // V/H ni Chaqiramiz Animatorni StringToHash ida
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
            inputHandler = GetComponentInParent<InputHandler>();


            // PlayerLocomotionni Chaqiramiz
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();



        }
        #endregion




        #region Animatsiyani Sozlash



        public void UpdateAnimationValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {

            #region Vertical Uchun

            // Verticalni Qiymatini 0 qilamiz
            float v = 0;

            // Vertical Movement 0 dan Katta Bo'sa Va 0.55f dan Kichik Bo'sa
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                // v 0.5f ga teng Bo'sin
                v = 0.5f;
            }
            // Vertical Movement 0.55f dan Katta Bo'sa
            else if (verticalMovement > 0.55f)
            {
                // v 1f ga teng Bo'sin
                v = 1;
            }
            // Vertical Movement 0 dan Kichik Bo'lsa Va -0.55f dan Katta Bo'lsa
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                // v 0.5f ga teng
                v = -0.5f;
            }
            // Vertical Movement -0.55 Kichik Bo'lsa 
            else if (verticalMovement < -0.55f)
            {
                // v -1 ga teng
                v = -1;
            }
            else
            {
                // Hech biri Bo'lmasa v = 0f ga
                v = 0;
            }

            #endregion

            #region Horizontal Uchun
            // Horizontal Qiymatini 0 qilamiz
            float h = 0;

            // Horizontal Movement 0 dan Katta Bo'sa Va 0.55f dan Kichik Bo'sa
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                // h 0.5f ga teng Bo'sin
                h = 0.5f;
            }
            // Horizontal Movement 0.55f dan Katta Bo'sa
            else if (horizontalMovement > 0.55f)
            {
                // h 1f ga teng Bo'sin
                h = 1;
            }
            // Horizontal Movement 0 dan Kichik Bo'lsa Va -0.55f dan Katta Bo'lsa
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                // h 0.5f ga teng
                h = -0.5f;
            }
            // Horizontal Movement -0.55 Kichik Bo'lsa 
            else if (horizontalMovement < -0.55f)
            {
                // h -1 ga teng
                h = -1;
            }
            else
            {
                // Hech biri Bo'lmasa h = 0f ga
                h = 0;
            }


            #endregion

            // Agar IsSprinting True Bo'lsa
            if (isSprinting)
            {
                // Vertical Tezlikni 2 qil
                v = 2;
                // Horirontgal O'z Qiymatida Qosin
                h = horizontalMovement;
            }


            // Animatsiyani Boshqarib Sozlash
            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);


        }


        // Burila Oladimi Funksiyasi
        public void CanRotate()
        {
            // Burila Oladimi True Qilamiz
            canRotate = true;
        }

        // Burilishni To'xtatish Funksiyasi
        public void StopRotation()
        {
            // Burila Oladimi False Qilamiz
            canRotate = false;
        }

        // Comboni Yoqish va O'chirish 
        public void EnableCombo()
        {
            // CanDoCombo Animatsiya Boolenini true qilamiz
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo() {

            // CanDoCombo Animatsiya Boolenini false qilamiz
            anim.SetBool("canDoCombo", false);
        }


        #endregion


        #region Target Uchun Animatsiya

        // Player Target Degan Metod Ochamiz, Paramatriga String Va Bool Beramiz

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            // Animatsiyada rootmotion isInteract Bo'lganda ishlidi
            anim.applyRootMotion = isInteracting;

            // IsInteracting ni SetBool orqali Parametrga Beramiz   
            anim.SetBool("IsInteracting", isInteracting);

            // Animatsiya Yumshoq O'tishi uchun CrossFadedan Foydalanmiz!
            anim.CrossFade(targetAnim, 0.2f);

        }

        // OnAnimatorMove Animatsiya Jarayonida Harakat Metodini Chaqiramiz

        private void OnAnimatorMove()
        {
            // Agar InputHandlerdagi isInteracting Booleni false Bo'lsa
            if (playerManager.isInteracting == false)
                // True ni Qaytar
                return;

            // delta time ochami delta o'zgaruvchisiga
            float delta = Time.deltaTime;

            // PlayerLocomotionda Rigidbodyni Dragini 0 qilamiz
            playerLocomotion.Rigidbody.drag = 0;

            // Animatsiya Positioni Uchun DeltaPosition Chaqiramiz
            Vector3 deltaPosition = anim.deltaPosition;

            // Animatsiya Davomida Y o'qi Bo'yicha Tepaga Chiqib ketmasligi uchun DeltaPosition Y o'qida 0 beramiz
            deltaPosition.y = 0;

            // Tezlikni Normalizatsiya Qilamiz
            Vector3 velocity = deltaPosition / delta;

            // PlayerLocomotiondagi Rigidbody Velocityni Delta Time Bilam Normalizatsiya Bo'gan Velocityga Tenglashtiramiz
            playerLocomotion.Rigidbody.velocity = velocity;

        }


        #endregion

    }
}