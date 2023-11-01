using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace SG
{
    public class PlayerManager : CharacterManager
    {

        #region E'lonlar

        // InputHandler Chaqiramiz
        InputHandler inputHandler;

        // Animator anim ni Chaqiramiz
        Animator anim;

        // CameraHandlerni Chaqirish
        CameraHandler cameraHandler;

        // PlayerLocomotion Chaqirish
        PlayerLocomotion playerLocomotion;

        // InteractableUI ni Chaqirish
        InteractableUI interactableUI;

        // IsInteracting degan Bool Yaratamiz
        public bool isInteracting;

        [Header("Player Flags")]
        // Sprinting Bo'ldi Bool E'lon qilamiz
        public bool isSprinting;
        // isInAir Bo'ldi Bool E'lon qilamiz
        public bool isInAir;
        // isGrounded Bo'ldi Bool E'lon qilamiz
        public bool isGrounded;
        // CanDoCombo Bo'ldi Bool E'lon qilamiz
        public bool canDoCombo;

        // InteractableUIGameObject degan GameObject E'lon qilamiz
        public GameObject InteractableUIGameObject;

        // ItemInteractableGameObject degan GameObject E'lon qilamiz
        public GameObject ItemInteractableGameObject;


        #endregion

        void Start()
        {
            // InputHandlerni Chaqiramiz
            inputHandler = GetComponent<InputHandler>();

            // anim ni Chaqiramiz
            anim = GetComponentInChildren<Animator>();

            // Player Locomotionni Chaqiramiz
            playerLocomotion = GetComponent<PlayerLocomotion>();

            // InteractableUI ni Chaqirish
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        private void Awake()
        {
            // FindObjectOfType orqali CameraHandler Clasini Topamiz Va cameraHandler O'zgaruvchisga Yuklimiz
            cameraHandler = FindObjectOfType<CameraHandler>();

        }

        // Update is called once per frame
        void Update()
        {

            // Kod 1
            // InputHandlerdagi Isinteracting Booleniga animdagi IsInteractingni Parametriga O'zlashtiramiz
            isInteracting = anim.GetBool("IsInteracting");

            // InputHandlerdagi canDoCombo Booleniga animdagi canDoComb Parametriga O'zlashtiramiz
            canDoCombo = anim.GetBool("canDoCombo");

            // Kod-2

            // Bizga Delta time kerak
            float delta = Time.deltaTime;

            // InputHandle dagi TickInputni Chaqiramiz
            // deltani Paratmetr Sifatida Joylaymiz
            // Bu MoveInputni Chaqiradi!
            inputHandler.TickInput(delta);

            // HandleRollingAndSprintni Chaqiramiz
            playerLocomotion.HandleRollingAndSprint(delta);

            // HandleJumping ni Chaqiramiz
            playerLocomotion.HandleJumping();

           

       


         

            // CheckForInteractableObject ni chaqiramiz
            CheckForInteractableObject();



        }

        // CheckForInteractableObject Metodini Ochamiz

        public void CheckForInteractableObject(){

            // RaycastHit ochamiz
            RaycastHit hit;

           
            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {

                // Agar Interactable tag ga urilsa
                if (hit.collider.tag == "Interactable")
                {
                   // Debug.Log("CheckForInteract ishlayapti, Ray >>" + hit);
                    // Interable dan interactableObject O'zgaruvchisiga hit.collider objectini ichiga Intractble Componentini yozamiz
                    Interactable InteractableObject = hit.collider.GetComponent<Interactable>();

                    // Agar InteractableObject Null bo'lmasa
                    if (InteractableObject != null)
                    {
                       // Debug.Log("InteractableObject Ishlayapti");
                        // interactablText string O'zgaruvchiga, Interactableobject ichidagi InteractableTextni o'zlashtiramiz
                        string interactableText = InteractableObject.InteractableText;

                        // interactableUI ni Ichidagi InteractableTextni texti teng bo'ladi interactableTextga
                        interactableUI.interactableText.text = interactableText;

                        // InteractableUIGameObject ni Active Qilamiz
                        InteractableUIGameObject.SetActive(true);


                        //Debug.Log("Input: " + inputHandler.a_Input);

                        if(inputHandler.a_Input)
                        {
                           // Debug.Log("B_Input Ishladi");
                            //  hit.collider objectini ichiga Intractble Componentini yozamiz va Interactable ichidagi Interact
                            // metodini Chaqiramiz Parametriga this yozsak Interactni parametriga Metoddi O'zi Chaqirilyapti!
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }


                    }



                }


            }
            else
            {
                // Agar InteractableUIGameObject null bo'lmasa
                if (InteractableUIGameObject != null)
                {
                    // InteractableUIGameObject ni disable false qilamiz
                    InteractableUIGameObject.SetActive(false);
                }

                // Agar ItemInteractableGameObject null Bo'lmasa
                if(ItemInteractableGameObject != null && inputHandler.a_Input)
                {
                    ItemInteractableGameObject.SetActive(false);
                }
            }

        }

        // Fizika Uchun FixedUpdate Ishlatamiz

        private void FixedUpdate()
        {  
            
            // Delta Time Fizikaviy Vaqt Bilan Moslashadi Fizikaviy Harakatni 
            // To'g'ri Taqsimlaydi
            float delta = Time.fixedDeltaTime;


            // HandleMovementni Chaqiramiz
            playerLocomotion.HandleMovement(delta);

            // HandleFallingni Chaqirmazi
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        // Late Update ni Chaqiramiz

        private void LateUpdate()
        {

            // InputHandlerdagi RollFlag Reset Bo'lishi Uchun Uni False Qilamiz
            inputHandler.rollFlag = false;


            // InputHandlerdagi rb_Input Reset Bo'lishi Uchun Uni False Qilamiz
            inputHandler.rb_Input = false;

            // InputHandlerdagi rt_Input Reset Bo'lishi Uchun Uni False Qilamiz
            inputHandler.rt_Input = false;


            // InputHandlerdagi D_Padlarni Reset Bo'lishi Uchun Uni False Qilamiz
            inputHandler.d_pad_Up = false;
            inputHandler.d_pad_Down = false;
            inputHandler.d_pad_Left = false;
            inputHandler.d_pad_Right = false;

            // InputHandlerdagi A_Input Reset Bo'lishi Uchun Uni Fals Qilamiz
            inputHandler.a_Input = false;

            // InputHandlerdagi jump_Input Reset Bo'lishi Uchun Uni False Qilamiz
            inputHandler.jump_Input = false;

            // InputHandlerdagi inventory_Input Reset Bo'lishi Uchun Uni False Qilamiz
            inputHandler.inventory_Input = false;

            // Kod 3

            // delta Timeni e'lon qilamiz delta o'zgaruvchisiga olib
            float delta = Time.deltaTime;
            // cameraHandler Null Bo'lmasa
            if (cameraHandler != null)
            {

                // CameraHandlerdagi FollowCamerani Chaqiramiz
                cameraHandler.FollowTarget(delta);
                // CameraHandlerdagi Rotatsiyani Chaqiramiz
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }



            // Agar Player Havoda Bo'lsa
            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }


    }
}