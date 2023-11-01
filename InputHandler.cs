using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SG
{
    public class InputHandler : MonoBehaviour
    {
        #region E'lonlar


        // Vertical Va Horizontallar

        public float vertical;
        public float horizontal;

        // Mouses

        public float mouseX;
        public float mouseY;

        // Attack Uchun RB va RT Boolenlarini Yaratamiz
        public bool rt_Input;
        public bool rb_Input;

        // D_Pad lar Uchun Boolen Yaratimiz
        public bool d_pad_Up;
        public bool d_pad_Down;
        public bool d_pad_Left;
        public bool d_pad_Right;


        // Input Systemni Chaqirish

    
        // Player Controllerni E'lon Qilamiz
        PlayerController InputAction;

        // Player Attackerni E'lon Qilamiz
        PlayerAttacker playerAttacker;

        // Player Inventory ni E'lon Qilamiz
        PlayerInventory playerInventory;

        // Player Manager ni E'lon Qilamiz
        PlayerManager playermanager;

        // PlayerLocomotion ni E'lon Qilamiz
        PlayerLocomotion playerlocomotion;

        // UIManager ni E'lon Qilamiz
        UIManager uiManager;

        // CameraHandlerni Chaqiramiz
        CameraHandler cameraHandler;

        // Harakatlanish
        public float moveAmount;

        // X,Y O'qi Bo'ylab Harakatlanish
        Vector2 movementInput;

        // Camera Uchun X, Y O'qi Bo'ylab 
        Vector2 cameraInput;


        // B_Input degan Bool Yaratamiz
        public bool b_input;

        // A_Input degan Bool Yaratamiz
        public bool a_Input;

        // Jump degan Bool Yaratamiz
        public bool jump_Input;

        // RollFlag degan Bool Yaratamiz
        public bool rollFlag;

        // LockOnInput degan Bool Yaratamiz
        public bool lockOnInput;

        // LockOn Uchun right va left input boolenlarini yozamiz(1,2)
        public bool right_Stick_Right_Input;
        public bool left_Stick_Left_Input;




        // Inventory_Input degan Bool Yaratamiz
        public bool inventory_Input;

        // RolInputTimer Degan Float Yaratmiz
        public float rollInputTimer;

        // SprintFlag Degan Bool Yaratmiz
        public bool sprintFlag;

        // ComboFlag Yaratamiz
        public bool comboFlag;

        // InvetoryFlag Yaratamiz
        public bool inventoryFlag;

        // LockOnFlag Yaratmiz
        public bool lockOnFlag;
  



        #endregion


        #region Yoqish O'chirishlar

        public void OnEnable()
        {

            // Agar InputAction Bo'sh Bo'lsa
            if (InputAction == null)
            {
                // PlayerControlni Object qilib Ula!
                InputAction = new PlayerController();

                // Movement WASD ni Chaqirish
                InputAction.PlayerMovemenrt.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();

                // Camerani Chaqirish
                InputAction.PlayerMovemenrt.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

                // rb va rt Inputlarni Chaqiramiz
                InputAction.PlayerActions.RB.performed += i => rb_Input = true;
                InputAction.PlayerActions.RT.performed += i => rt_Input = true;

                // Inputdan System dan D_Pad Right va Leftni Chaqiramiz
                InputAction.PlayerQuickSlots.D_Pad_Rigth.performed += i => d_pad_Right = true;
                InputAction.PlayerQuickSlots.D_Pad_Left.performed += i => d_pad_Left = true;

                // Jump Inputni Chaqiramiz
                InputAction.PlayerActions.Jump.performed += i => jump_Input = true;

                // A inputni Chaqiramiz[F]
                InputAction.PlayerActions.Interact.performed += i => a_Input = true;

                // Inventory Inputni Chaqiramiz
                InputAction.PlayerActions.Inventory.performed += i => inventory_Input = true;
              
                // LockOn Inputni Chaqiramiz
                InputAction.PlayerActions.LockOn.performed += i => lockOnInput = true;

                // LockOn Right-Left larni Chaqiramiz
                InputAction.PlayerMovemenrt.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                InputAction.PlayerMovemenrt.LockOnTargetLeft.performed += i => left_Stick_Left_Input = true;

            
            }

            InputAction.Enable();
        }

        private void OnDisable()
        {
            InputAction.Disable();
        }


        #endregion


        #region TickInput Metodi Asosiy Kodlarni Jamlash uchun



        // TickInput Yaratamiz delta Parametri Bilan
        public void TickInput(float delta)
        {
            // Move Inputni Chaqiramiz delta Parametri Bilan
            HandleMoveInput(delta);

            // HandleRollingInput Chaqiramiz delta Parametri bilan
            HandleRollingInput(delta);

            // HandleAttackInput Chaqiramiz delta Parametri bilan
            HandleAttackInput(delta);

            // HandleQuickSlotsInput Chaqiramiz delta Kerakmas
            HandleQuickSlotsInput();

            // HandleInteractingButtonInput Chaqiramiz delta kerakmas
            HandleInteractingButtonInput();


            // HandleInventoryInput ni Chaqiramiz
            HandleInventoryInput();

            // HandleLockOnInput ni Chaqiramiz
            HandleLockOnInput();
        }


        #endregion



        #region Asosiy Kodlar

        private void Awake()
        {
            // playerAttackerni Chaqiramiz
            playerAttacker = GetComponent<PlayerAttacker>();
            // PlayerInventoryni Chaqiramiz
            playerInventory = GetComponent<PlayerInventory>();
            // PlayerManagerni Chaqiramiz
            playermanager = GetComponent<PlayerManager>();
            // UIManagerni FindAnyObjectByType orqali Chaqiramiz
            uiManager = FindObjectOfType<UIManager>();
            // CameraHandlerni FindAnyObjectByType orqali Chaqiramiz 
            cameraHandler = FindAnyObjectByType<CameraHandler>();
        }



        // Move Input Yaratamiz delta Parametri Bilan
        private void HandleMoveInput(float delta)
        {
            // Vertical Va Horizontalni O'zlashtirish
            horizontal = movementInput.x;
            vertical = movementInput.y;


            // Vertical Va Horizontalni Qiymatlarini Absaloute Qilish
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            // Camerani O'zlashtirish
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;



        }


        // Rolkling Input Yaratamiz delta Parametri Bilan;

        private void HandleRollingInput(float delta)
        {
            // b_Input ni chaqiramiz
            b_input = InputAction.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            // SprintFlagni true qilamiz
            sprintFlag = b_input;
            // Agar B_Input true Bo'lsa

            if (b_input)
            {
                // RollgFlag true Bo'lsa PlayerLocomotionda Amal Bajaramiz
                // rollFlag = true;

                // RollInputTimerga delta Time O'rnatamiz
                rollInputTimer += delta;

                
            }
            else
            {
                // Agar rollInputTimer 0 dan Kichik Va 0.5 dan Katta Bo'lsa
                if (rollInputTimer > 0 && rollInputTimer < 0.5)
                {
                    // SprintFalg fals Bo'ladi
                    sprintFlag = false;

                    // RollFlag true Bo'ladi
                    rollFlag = true;
                }

                // Agar Unde Bo'lmasa rollInputTimer 0 qilamiz
                rollInputTimer = 0;

            }


      

        }
        #endregion

        // AttackInput Yaratamiz Delta Parametri Bilan
        private void HandleAttackInput(float delta)
        {
         
            
            // Agar RB_Input true Bo'lsa
            if(rb_Input)
            {
                // PlayerManager ichidagi canDoCombo booleni true bo'lsa
                if (playermanager.canDoCombo) {
                    // ComboFlagni true qilamiz
                    comboFlag = true;
                    // PlayerAttcak ichidagi ComboAttack metodi Parametriga playerInvetori ichidagi rightWeaponni O'zlashtiramiz
                    playerAttacker.HandleComboAttack(playerInventory.rightWeapon);
                    // Combofalgni false qilamiz
                    comboFlag = false;
                }
                else
                {
                    // Agar PlayerManagerdagi isInteracting true bo'lsa return Qaytar
                    if (playermanager.isInteracting)
                        return;

                    // Agar PlayerManagerdagi canDoCombo true return Qaytar
                    if (playermanager.canDoCombo)
                        return;

                    // PlayerAttavker Ichidagi Hujum Metodi Parametriga, playerInvetori ichidagi righWeaponni O'zlashtiramiz
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }

            }

            // Agar RT_Input true Bo'lsa
            if (rt_Input)
            {
                // PlayerAttavker Ichidagi Og'ir Hujum Metodi Parametriga, playerInvetori ichidagi righWeaponni O'zlashtirami
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }



        }


        // D_Pad Input yaratamiz
        private void HandleQuickSlotsInput()
        {
           

            // Agar D_Pad Rigth True Bo'lsa 
            if (d_pad_Right)
            {
                // PlayerInventorydagi ChangerRightWeaponni Chaqiramiz
                playerInventory.ChangeRightWeapon();
            }
            else if (d_pad_Left)
            {
                // PlayerInventorydagi ChangeLeftWeaponni Chaqiramiz
                playerInventory.ChangeLeftWeapon();
            }
        }

        // HandleActionButtonInput yaratamiz
        public void HandleInteractingButtonInput()
        {
            //Debug.Log("A_input" + a_Input);
           
            if (a_Input)
            {
                playermanager.CheckForInteractableObject();
            }

        }


        // HandleInventory yaratamiz
        public void HandleInventoryInput()
        {

            // inventory_Input true Bo'lsa
            if (inventory_Input)
            {
                // Escs Bosilsa true yana bosilsa false qiladi
                inventoryFlag = !inventoryFlag;

                // Agar Inventory flag true bo'lsa
                if (inventoryFlag)
                {
                    // Select Window Ko'rsatilsin
                    uiManager.OpenSelectWindow();
                    // Upadete UI Ishlasin
                    uiManager.UpdateUI();
                    // Hud Window O'chirilsin
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    // Select Window Ko'rsatilmasin
                    uiManager.CloseSelectWindow();
                    // WeaponInventory Window Ko'rsatilaydi
                    uiManager.CloseAllInventoryWindows();

                    // Hud Window Yoqilsin
                    uiManager.hudWindow.SetActive(true);
                }

            }
        }

        // HandleLockOnInput ni Chaqiramiz
        private void HandleLockOnInput()
        {
            // Agar locOnInput Va LockOnFlag false Bo'lsa
            if(lockOnInput && lockOnFlag == false)
            {
                // cameraHandlerdagi ClearLockOnTarget ni chaqiramiz
                cameraHandler.ClearLockOnTarget();
               
                // lockOnInput true bo'ladi
                lockOnInput = false;


                // CameraHandler Ichidagi HandleLockOn Metodini Chaqiramiz
                cameraHandler.HandleLockOn();

                if(cameraHandler.nearestLockOnTarget != null)
                {
                    // Agar CameraHandler Ichidagi LockOnTarget nearestLockOnTarget ga teng bo'lsa LockOn Ishlidi
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;

                    // lockOnFlag esa true
                    lockOnFlag = true;
                }

            }
            // Agar lockOnInput va lockOnFlag true Bo'lsa
            else if (lockOnInput && lockOnFlag)
            {
               
                // lockOnInput false bo'ladi
                lockOnInput = false;

                // lockOnFlag esa false
                lockOnFlag = false;

                // cameraHandlerdagi ClearLockOnTarget ni chaqiramiz
                cameraHandler.ClearLockOnTarget();

            }



            // lockOnFlag Va left_Stick_Left_Input true bo'lsa
            if (lockOnFlag && left_Stick_Left_Input)
            {
                // left_Stick_Left_Input o'zgaruvchisini false ga o'zgartiramiz
                left_Stick_Left_Input = false;

                // cameraHandler obyektining HandleLockOn() metodini chaqiramiz
                cameraHandler.HandleLockOn();

                // cameraHandler obyektining leftLockOnTarget o'zgaruvchisi null emasligini tekshiramiz
                if (cameraHandler.leftLockOnTarget != null)
                {
                    // cameraHandler obyektining currentLockOnTarget o'zgaruvchisini leftLockOnTarget ga tenglashtiramiz
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockOnTarget;
                }

                Debug.Log("LockOn: " + lockOnFlag + " left_Stick_Left_Input: " + left_Stick_Left_Input);
            }


            // lockOnFlag Va right_Stick_Right_Input true bo'lsa
            if (lockOnFlag && right_Stick_Right_Input)
            {
                // right_Stick_Right_Input o'zgaruvchisini false ga o'zgartiramiz
                right_Stick_Right_Input = false;

                // cameraHandler obyektining HandleLockOn() metodini chaqiramiz
                cameraHandler.HandleLockOn();

                // cameraHandler obyektining rightLockOnTarget o'zgaruvchisi null emasligini tekshiramiz
                if (cameraHandler.rightLockOnTarget != null)
                {
                    // cameraHandler obyektining currentLockOnTarget o'zgaruvchisini rightLockOnTarget ga tenglashtiramiz
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockOnTarget;
                }

                Debug.Log("LockOn: " + lockOnFlag + " right_Stick_Right_Input: " + right_Stick_Right_Input);
            }

            // SetCameraHeight metodini chaqiramiz
            cameraHandler.SetCameraHeight();
          
        }
    }
}
