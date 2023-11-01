using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SG
{
    public class PlayerLocomotion : MonoBehaviour
    {

        #region E'lonlar

        // CameraHandlerni Chaqiramiz
        CameraHandler cameraHandler;

        [Header("Ground & Air Detection Stats")]

        [SerializeField]
        // groundDetectionRayStartPoint E'lon Qilamiz
        float groundDetectionRayStartPoint = 0.5f;

        [SerializeField]
        // minimumDistanceNeededToBeginFall E'lon Qilamiz
        float minimumDistanceNeededToBeginFall = 1f;

        [SerializeField]
        // groundDirectionRayDistance E'lon Qilamiz
        float groundDirectionRayDistance = 0.2f;

        // ignoreForGroundCheck E'lon Qilamiz LayerMask uchun 
        LayerMask ignoreForGroundCheck;

        // isAirTimer E'lon Qilamiz
        public float inAirTimer;




        // Player Manager Chaqiramiz
        PlayerManager playerManager;

        // Target Uchun Camera Transform
        Transform Cameraobject;

        // InputHandlerni Chaqiramiz
        InputHandler inputHandler;

        // Tomonga Yurishi Uchun Vector3
        public Vector3 moveDirection;

        // My Transform Ochib Uni Inspectorda Yashiramiz
        // MyTransform Deafult Transform uchun kerak bo'ladi
        [HideInInspector]
        public Transform Mytransform;

        // Animation Handler Kodi uchun O'zgaruvchi! Boshqa kodni Chaqirish Uchun
        [HideInInspector]
        public AnimationHandler animatorHandler;

        // Rigidbody Ulimiz
        public Rigidbody Rigidbody;



        [Header("Movement Stats")]

        // Ochiq Kod Qilib Yozamiz Inspectorga Ko'rinadi SerializeField Bilan

        // Harakat Tezligi
        [SerializeField]
        float movementSpeed = 5;
        // Yugurish Tezligi
        [SerializeField]
        float sprintSpeed = 7;


        // Rotatsiya Tezligi
        [SerializeField]
        float rotationSpeed = 10;

        // Qulash Tezligi
        [SerializeField]
        float fallingSpeed = 45;


        #endregion





        private void Awake()
        {
            // CameraHandlerni Chaqiramiz FindObjectOfType orqali
            cameraHandler = FindObjectOfType<CameraHandler>();
        }
        void Start()
        {

            #region Chaqirishlash

            // PlayerManagerni Chaqiramiz
            playerManager = GetComponent<PlayerManager>();

            // Rigidbodyni Chaqiramiz
            Rigidbody = GetComponent<Rigidbody>();

            // InputHandlerni Chaqiramiz
            inputHandler = GetComponent<InputHandler>();

            // Camerani Ulimiz
            Cameraobject = Camera.main.transform;

            // My Transformni Default Transformga Ulimiz
            Mytransform = transform;

            // AnimationHandlerni InChildern Bilan Chaqiramiz
            // Nega Bunde qiyapmiz Chunki Bizi Player Player Nomli GameObject Ichida
            // InChilren Hijerarxiyani ostidagi objectlarni ham tekshiradi
            animatorHandler = GetComponentInChildren<AnimationHandler>();

            // Animation Handlerdagi E'lonlarni Chaqirish Uchun Initilizni Chaqiramiz 
            animatorHandler.Initialize();

            #endregion

            HandleRotation(rotationSpeed);



            #region Qulash Uchun Chaqirishlar

            // O'yin Boshlanganda IsGround True Bo'lsin Chunki Player Yerda Turgan Bo'ladi
            playerManager.isGrounded = true;

            // Layerlardan 8 va 11 ni olamiz
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);

            #endregion
        }


        #region Harakatlar

        // Vector3 da NormalVector E'lon qilamiz

        Vector3 normalVector;


        // Harakatlanish Metodi
        public void HandleMovement(float delta)
        {
            // Agar inputHandler dagi RollFlag True Bo'lsa Qaytar,
            if (inputHandler.rollFlag)
                return;

            // Agar playerManager dagi isInteracting True Bo'lsa Qaytar,
            if (playerManager.isInteracting)
                return;

            // Camera moveDirection ni Z O'qini Inputdagi Verticalga Ko'paytiramiz; Endi Biz Oldinga Va Orqaga Harakatlana Olamiz;
            moveDirection = Cameraobject.forward * inputHandler.vertical;

            // Camera moveDirection ni X O'qini Inputdagi Horizontalga Ko'paytiramiz; Endi Biz Chapga Va O'nga Harakatlana Olamiz;
            // Z O'lchami ishlashi uchun TargetDirni Qo'shib Yozamiz
            moveDirection += Cameraobject.right * inputHandler.horizontal;

            // moveDirection ni Normazilatsiya Qilamiz;
            moveDirection.Normalize();

            // MoveDirection Y o'qi Bo'yicha tepaga ko'tarilmasligi Uchun Y O'qini 0 qilamiz
            moveDirection.y = 0;

            // Movement Speedni speedga O'zlashtiramiz
            float speed = movementSpeed;


            // Agar sprintFlag dagi RollFlag Va MoveAmount 0.5 dan Katta Bo'lsa
            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
            {
                // sprintSpeed ni speedga Tenglimiz
                speed = sprintSpeed;
                // IsSprintingni True Qilamiz
                playerManager.isSprinting = true;

                // speedni MoveDirectionga O'zlashtiramiz
                moveDirection *= speed;
            }
            else
            {
                // Agar MoveAmount 0.5 dan Kichik Bo'lsa
                if (inputHandler.moveAmount < 0.5)
                {
                    // Bo'lmasa Faqat speedni MoveDirectionga O'zlashtiramiz
                    moveDirection *= speed;
                    // Sprintni False qilamiz
                    playerManager.isSprinting = false;
                }
                else
                {
                    // Yana Faqat speedni MoveDirectionga O'zlashtiramiz
                    moveDirection *= speed;
                    // Sprintni False qilamiz
                    playerManager.isSprinting = false;
                }

            }

            // MoveDirectionni Speedga Ko'paytiramiz
            //moveDirection = moveDirection * speed;

            // Vector3 ni NormalVector uchun tekislimiz
            Vector3 projectVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);

            // Rigidboduni Velocitysini Project Velocityga O'zlashtiramiz
            Rigidbody.velocity = projectVelocity;

            // Agar inputHandler.lockOnFlag true bo'lsa va inputHandler.sprintFlag false bo'lsa
            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                // Animatsiya Ijro Etilis Uchun UpdateAnitionValues ga VerticalMovement Parametriga InputHandlerdaga 
                // Vertical beramiz va horizontal beramiz 
                animatorHandler.UpdateAnimationValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                // Animatsiya Ijro Etilis Uchun UpdateAnitionValues ga VerticalMovement Parametriga InputHandlerdaga.moveAmoun 
                // Horizontaliga esa 0 Beramiz
                animatorHandler.UpdateAnimationValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }

          

            // Animator Handler dagi Burilish true Bo'lsa

            if (animatorHandler.canRotate)
            {
                // HandleRotation ni Chaqiramiz Parametri Bilan
                HandleRotation(delta);
            }
        }

        #endregion


        // Targerni Positsiyasi Vector3
        Vector3 targetPositon;

        #region Rotatsiya

        // Rotatsiya Yaratamiz
        private void HandleRotation(float delta)
        {

            // inputHandler.lockOnFlag true bo'lsa va inputHandler.sprintFlag false bo'lsa
            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                // inputHandler.sprintFlag yoki inputHandler.rollFlag true bo'lsa
                if (inputHandler.sprintFlag || inputHandler.rollFlag) {

                    // targetDirection ni Vector3.zero(0, 0, 0) ga tenglaymiz
                    Vector3 targetDir = Vector3.zero;

                    // Kamera transformining Z o'qini inputHandler'ning vertical qiymatiga ko'paytiramiz
                    targetDir = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                    // Kamera transformining Y o'qini inputHandler'ning horizontal qiymatiga ko'paytiramiz
                    targetDir += cameraHandler.cameraTransform.right * inputHandler.horizontal;

                    // targetDir ni normalizatsiya qilamiz
                    targetDir.Normalize();

                    // targetDir Y o'qini 0 ga tenglaymiz, chunki bizga y o'qi bo'yicha harakat kerak emas
                    targetDir.y = 0;

                    // Agar targetDir Vector3.zero ga teng bo'lsa
                    if (targetDir == Vector3.zero)
                    {
                        // Z o'qi bo'yicha oldinga va orqaga harakatlanadi
                        targetDir = transform.forward;
                    }

                    // targetDir tomonga qarab tr rotatsiyasini olish
                    Quaternion tr = Quaternion.LookRotation(targetDir);

                    // transform.rotation dan tr ga interpolatsiya qilish, rotationSpeed va Time.deltaTime orqali
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                    // targetRotation ni transform.rotation ga tenglaymiz
                    transform.rotation = targetRotation;


                }
                else
                {
                    // moveDirection vektorini rotatiobDirection vektoriga tenglaymiz
                    Vector3 rotationDirection = moveDirection;

                    // cameraHandler.currentLockOnTarget.position dan transform.position ni ayiramiz, 
                    // bu orqali rotationDirection vektoriga hedefning yo'nalishini olishimiz mumkin
                    rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;

                    // rotatiobDirection y-koordinatasini 0 ga tenglaymiz, chunki biz yo'nalishni y o'qi bo'yicha o'zgartirmaymiz
                    rotationDirection.y = 0;

                    // rotatiobDirection vektorini normalizatsiya qilamiz
                    rotationDirection.Normalize();

                    // rotatiobDirection yo'nalishiga qarab tr rotatsiyasini hisoblaymiz
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);

                    // transform.rotation dan tr ga interpolatsiya qilamiz, rotationSpeed va Time.deltaTime orqali
                    Quaternion targetrotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                    // targetrotation ni transform.rotation ga tenglaymiz
                    transform.rotation = targetrotation;
                }


            }
            else
            {
                // targetDirection ni Vector3.zero(0.0.0) ga Tenglimiz
                Vector3 targetDir = Vector3.zero;

                // InputHandledagi MoveAmountni MoveOverride ga O'zlashtiramiz
                float moveOverride = inputHandler.moveAmount;

                // Camera Transformni Z O'qini Inputdagi Verticalga Ko'paytiramiz; Endi Biz Oldinga Va Orqaga Rotatsiya Qila Olamiz;
                targetDir = Cameraobject.forward * inputHandler.vertical;

                // Camera Transformni X O'qini Inputdagi Horizontalga Ko'paytiramiz; Endi Biz Chapga Va O'nga Rotatsiya Qila Olamiz;
                // Z O'lchami ishlashi uchun TargetDirni Qo'shib Yozamiz
                targetDir += Cameraobject.right * inputHandler.horizontal;

                // TargetDir ni Normazilatsiya Qilamiz;
                targetDir.Normalize();

                // TargetDir Y O'qini 0 ga Tenglaymiz Chunki Bizga Y O'qi Bo'yicha Harakat Keremas
                targetDir.y = 0;

                // Agar TargetDir Vector3.zeroga teng Bo'lsa Targetni default transformga Z ko'rinishda Harakatlanishni tenglimiz
                // Bu Targetn rotatsiyasini 0 ga qaytib qolishini oldini oladi!

                if (targetDir == Vector3.zero)
                {
                    targetDir = Mytransform.forward;
                }


                // Quaternion orqali LookRotationda Tomonlarga Qarashni Sozlaymiz
                Quaternion tr = Quaternion.LookRotation(targetDir);

                // RotationSpeedni O'zgaruvchiga olamiz
                float rs = rotationSpeed;

                // Transformni rotatsiyasini Sozlimiz Slerp orqali
                Quaternion targetRotation = Quaternion.Slerp(Mytransform.rotation, tr, rs * delta);

                // Rotatsiyani TargetRotationga O'zlashtiramiz
                Mytransform.rotation = targetRotation;

            }
        }


  


        #endregion


        #region Yugurish Va Dumalash

        // Roll And Sprint

        public void HandleRollingAndSprint(float delta)
        {
            // Agar AnimatorHandlerdagi anim IsInteracting True Bo'lsa
            if (animatorHandler.anim.GetBool("IsInteracting"))
            {
                // Return beramiz
                return;
            }

            // Agar InputHandlerdagi rollFlag True Bo'lsa
            if (inputHandler.rollFlag)
            {
                // Agar Z O'lchami Bo'yicha Harakatlanib RollFlag Ishlatilsa O'sha Tomonga Qarab Animatsiya Ishlidi
                moveDirection = Cameraobject.forward * inputHandler.vertical;
                // Agar Y O'lchami Bo'yicha Harakatlanib RollFlag Ishlatilsa O'sha Tomonga Qarab Animatsiya Ishlidi
                moveDirection += Cameraobject.right * inputHandler.horizontal;

                // Agar Roll true Bo'lgan Vaqta InputHandlerdagi moveamount 0 dan Katta Bo'lsa Ya'ni Harakatlanyotgan Vaqtda
                if (inputHandler.moveAmount > 0)
                {
                    // Rolling Animatsiyasi Ishlasin
                    animatorHandler.PlayTargetAnimation("Roll", true);
                    // Y O'qi Bo'yicha Tepaga Chiqib ketmaydi
                    moveDirection.y = 0;

                    // Istalgan tomonga Tez Burilib Roll Animatsiyasini Ishlatamiz
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);

                    // Default Transformni Rotatsiyani rollRotationga Tenglimzi
                    Mytransform.rotation = rollRotation;

                }
                else
                {
                    // Agar Yurmayotgan Holatida Alt Bosilsa Backstep Animatsiyasi Bo'ladi
                    animatorHandler.PlayTargetAnimation("BackStep", true);
                }

            }
        }


        #endregion



        // Handle Falling
        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            // IsGroundni False Qilamiz
            playerManager.isGrounded = false;
            // Raycasr Hit Chaqiramiz
            RaycastHit hit;
            // Ratcastni turgan nuqtasini myTransformni positioniga tenglashtiramiz
            Vector3 origin = Mytransform.position;
            // Originni Y o'lchami 0.5f ga teng bo'ladi
            origin.y += groundDetectionRayStartPoint;

            // Raycastga origin(O'zini Turgan Nuqtasi), oldingda harakatlanish uchun Mytransform.forward
            // beramiz Z o'qi bo'yicha, Agar rayni uzunligi 0.4f dan oshsa
            if (Physics.Raycast(origin, Mytransform.forward, out hit, 0.4f))
            {
                // moveDirection ga Vector3.zero tenglanadi: Ya'ni(0.0.0)
                moveDirection = Vector3.zero;
            }

            // Agar IsinAir Fals Bo'lsa
            if (playerManager.isInAir)
            {
                //"Rigidbody" komponentiga yuqoriga yo'naltiruvchi kuchni 
                // (-Vector3.up) fallingSpeed miqdori bilan qo'shib qo'yadi. Bu, o'yinchi havo bo'ylab pastga tushayotgan hissini yaratadi.
                Rigidbody.AddForce(-Vector3.up * fallingSpeed);

                // "Rigidbody" komponentiga moveDirection yo'nalishining fallingSpeed/5f miqdori
                // bilan kuch qo'shib qo'yadi. Bu, o'yinchi yurish yo'nalishida pastga tushayotgan kuchni yaratadi.
                // fallingSpeed / 5f miqdori kuchni 5 ga bo'lib bo'linadi, bu esa yurish kuchini pastlab borishning osonroq bo'lishini ta'minlaydi.
                Rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            // dir degan Vector3 ochamiz va moveDirectionni O'zlashtiramiz
            Vector3 dir = moveDirection;
            // dir o'zgaruvchini normalizatsiya qiladi. Bu, yo'nalish vektorini o'lchamlarini 1 ga tenglab, uni normalizatsiya qiladi.
            dir.Normalize();
            // origin orqali tomonga harakatlanishni aniqlimiz
            origin = origin + dir * groundDirectionRayDistance;

            // TargetPositionga  Mytransform.positionni o'zlashtiramiz
            targetPositon = Mytransform.position;

            // origin nuqtadan -Vector3.up * minimumDistanceNeededToBeginFall 
            // yo'nalishiga qizil rangdagi vaqtinchalik chiziq chizadi. 
            // Bu, pastga tushish uchun minimal masofaning o'lchamini ko'rsatadi.
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);

            // origin nuqta orqali pastga raycasting qiladi.
            // Agar rayning uzunligi minimumDistanceNeededToBeginFall miqdoridan katta
            // bo'lsa va ignoreForGroundCheck qatorida keltirilgan obyektlardan tashqari obyektlarga tegsa, shart bajariladi.
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPositon.y = tp.y;

                // Agar Havoda Bo'lsa
                if (playerManager.isInAir)
                {
                    // InAirtime 0.5 dan Katta Bo'lsa
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in air for " + inAirTimer);
                        // Landing Animation ishga tushadi
                        animatorHandler.PlayTargetAnimation("Landing", true);
                        // InAirTimer 0 ga Teng Bo'ladi
                        inAirTimer = 0;
                    }
                    else
                    {
                        // Bo'lmasa Empty Animation Ishlidi
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        // InAirTimer 0 ga Teng Bo'ladi
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }
                    Vector3 vel = Rigidbody.velocity;
                    vel.Normalize();
                    Rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            // Agar Yerda Bo'lsa
            if (playerManager.isGrounded)
            {
                // Interactting Bo'lsa Va Moveamount 0 dan katta Bo'lsa
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    // MyTransform Positionnini Lerp qilamiz target position oralig'ida
                    Mytransform.position = Vector3.Lerp(Mytransform.position, targetPositon, Time.deltaTime);
                }
                else
                {
                    // MyTransform Positionini targetPositionga tenglimiz
                    Mytransform.position = targetPositon;
                }
            }

            // Agar isInteractin true bo'lsa Va MoveAmount 0 dan Katta Bo'lsa
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                // My Transform Positionnini Lerp qilamiz target position oralig'ida
                Mytransform.position = Vector3.Lerp(Mytransform.position, targetPositon, Time.deltaTime / 0.1f);
            }
            else
            {
                // MyTransform Positionini targetPositionga tenglimiz
                Mytransform.position = targetPositon;
            }

        }

        // Handle Jump
        public void HandleJumping()
        {
            // PlayerManagerdagi isInteracting true bo'lsa return qaytaramiz, Biz Sakrash Davomida Harakatlanmaymiz
            if (playerManager.isInteracting)
                return;

            // Agar Jump_Input True Bo'lsa
            if (inputHandler.jump_Input)
            {
         
                // MoveAmoun 0 dan Katta Bo'lsa
                if (inputHandler.moveAmount > 0)
                {
                    Debug.Log("Sakrash Ishlayapti");
                    // Agar Z o'lchami Bo'yicha Harakatlanib Jump Ishlatilsa O'sha Tomonga Qarab Animatsiya Ishlidi
                    moveDirection = Cameraobject.forward * inputHandler.vertical;
                    // Agar Y o'lchami Bo'yicha Harakatlanib Jump Ishlatilsa O'sha Tomonga Qarab Animatsiya Ishlidi
                    moveDirection += Cameraobject.forward * inputHandler.horizontal;

                    // Jump Animatsiyasi Ishlasin
                    animatorHandler.PlayTargetAnimation("Jump", true);
                    // Y O'qi Bo'yicha Tepaga Chiqib ketmaydi
                    moveDirection.y = 0;
                    // Istalgan tomonga Tez Burilib Jump Animatsiyasini Ishlatamiz
                    Quaternion jumprotation = Quaternion.LookRotation(moveDirection);
                    // Default Transformni Rotatsiyani jumprotationga Tenglimzi
                    Mytransform.rotation = jumprotation;
                }


            }

        }

       
    
    
    
    
    
        ///
        }
    }


