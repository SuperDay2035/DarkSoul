using System.Collections;
using System.Collections.Generic;
using UnityEditor.Hardware;
using UnityEngine;
using UnityEngine.UI;


namespace SG
{
    public class CameraHandler : MonoBehaviour
    {

        #region E'lonlar

        // InputHandlerni Chaqiramiz
        InputHandler inputHandler;

        // Player Managerni Chaqiramiz
        PlayerManager playerManager;



        // Target Uchun Transform yaratamiz
        public Transform targetTransform;

        // Camera Uchun Transform Yaratamiz
        public Transform cameraTransform;

        // Camera Pivot uchun Transform yaratamiz
        public Transform cameraPivotTransform;

        // My Transform Uchun Transform
        private Transform myTransform;

        // Right va Left uchun LockOntarget Tranform yaratamiz
        public Transform leftLockOnTarget;
        public Transform rightLockOnTarget;

        // Camera Positsiyasi Uchun Vector3 Yaratamiz
        private Vector3 cameraTransformPostion;

        // Bu Camera Collision uchun ishlidi
        public LayerMask ignoreLayers;
        public LayerMask enviromentLayer;


        // Camera Handlerni olamiz
        public static CameraHandler singleton;

        // CameraFollow Velocity si Ergasish Velocity si
        private Vector3 cameraFollowVelocity = Vector3.zero;

        // Qarash Tezligi
        public float lookSpeed = 0.1f;

        // Camera Ergasish Tezligi
        public float followSpeed = 0.1f;

        // Camera Pivot tezligi
        public float pivotSpeed = 0.03f;

        // Deafualt Positsiya
        private float defaultPosition;

        // Tomonga Qarash
        private float lookAngle;

        // Pivot uchun tomonga qarash
        private float PivotAngle;

        // Minimum Va Maxsimum tepaga va pastga qarash
        private float minimumPivot = -35;
        private float maximumPivot = 35;


        // Target Position float type da olamiz
        private float targetposition;

        // Camera uchun Sphera Radius E'lon qilamiz uni qiymati 0.2f bo'ladi
        public float cameraSphereRadius = 0.2f;

        // Camera Collision E'lon qilamiz
        public float cameraCollisionOffset = 0.2f;

        // Camerani Minimum Collisioni
        public float minimumCollisionoffset = 0.2f;

        // LockOn Uchun MaximumLockOnDistance float ochamiz Bu Lockon Maximum 30 metrdan boshlab
        // ishlashini anglatadi 
        public float maximumLockOnDistance = 30;

        // LPP ga 2.25f, ULPP ga esa 1,65f beramiz
        // bu 
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f; 

        // Lock On Uchun nearestLockOnTarget Degan Transform E'lon qilamiz
        public Transform nearestLockOnTarget;

        // Transform tipida currentLockOnTarget E'lon qilamiz
        public Transform currentLockOnTarget;


        // CharacterManager Uchun List yaratamiz    
        List<CharacterManager> availableTargets = new List<CharacterManager>();


        #endregion

        #region Chaqirishlar

        private void Awake()
        {
            // singletonga this orqali CameraHandlerni o'zlashtiramiz
            singleton = this;

            // default transformni myTransformga o'zlashtiramiz
            myTransform = transform;

            // CameraTransformda Local z olamiz
            defaultPosition = cameraTransform.localPosition.z;

            //  Collision uchun
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);

            // target Transform ga PlayerManagerni Transformini chaqiramiz FindObjectOfType orqali
            targetTransform = FindObjectOfType<PlayerManager>().transform;

            // InputHandlerni Chaqiramiz FindObjectOfType orqali
            inputHandler = FindObjectOfType<InputHandler>();

            // PlayerManageni Chaqiramiz FindObjectOfType orqali
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            enviromentLayer = LayerMask.NameToLayer("Enviroment");
        }

        #endregion


        #region Camera Ergashadi

        public void FollowTarget(float delta)
        {


            // Camera Targetga Ergashishi Uchu SmoothDamp dan foydalanamiz 
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);

            // MyTransforni positionini targetPositionga Tenglimiz
            myTransform.position = targetPosition;


            HandleCameraCollision(delta);


        }
        #endregion


        #region Camera Burilishi

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            // inputHandlerdagi lockOnFlag false Bo'sa va currentLockOnTarget Transformi null bo'sa
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {

                // Look Angle Orqali X Bo'yicha Burilishni amalga oshiramiz: Chapga Va O'nga
                lookAngle += (mouseXInput * lookSpeed) / delta;

                // pivotAngle orqali Y Bo'yicha Tepa Pastga Burilishni Hosil Qilamiz
                PivotAngle -= (mouseYInput * pivotSpeed) / delta;

                // Pivot Angleni Camerasini Tepa Pasga Cheklimiz
                PivotAngle = Mathf.Clamp(PivotAngle, minimumPivot, maximumPivot);

                // Rotatsiyani Default Holatini Yozamiz
                Vector3 rotation = Vector3.zero;

                // rotatsiyani Y O'qini Look Angle ga Moslashtiramiz
                rotation.y = lookAngle;

                // Rotatsiyani Quatrion objectini yasimiz
                Quaternion targetRotation = Quaternion.Euler(rotation);

                // Default My Tranformni Rotationi Target Rotationga Tenglimiz
                myTransform.rotation = targetRotation;

                // Yangi Vector3 Zero qilamiz Endi X O'qi Uchun
                rotation = Vector3.zero;

                // PivotAngle X O'qi Uchun Rotatsiyani Beligimiz
                rotation.x = PivotAngle;

                // TargerRotationni Yana Olib Object qilamiz X O'qi uchun
                targetRotation = Quaternion.Euler(rotation);

                // Camerani LocalRotationini Target Rotationga Tenglashtiramiz
                cameraPivotTransform.localRotation = targetRotation;


            }
            else
            {
                // float tipida velocity ochamiz va 0 beramiz
                float velocity = 0;
                // Vector3 tipida dir ochamiz va currentLockOnTarget positsiyasidan transform ni positsiyasini ayiramiz
                Vector3 dir = currentLockOnTarget.position - transform.position;

                // dir ni Normalize qilamiz
                dir.Normalize();

                // dir ni y o'qini 0 qilamiz
                dir.y = 0;

                // Quaternion tipida targetRotation yozamiz, LookRotation(tmongaga qarashga) dir ni yozamiz
                Quaternion targetRotation = Quaternion.LookRotation(dir);

                // rotatsiya targetrotationga teng
                transform.rotation = targetRotation;

                // hozrgi LockOnTargetni positionida cameraPivotTransformni positionini ayiramiz
                dir = currentLockOnTarget.position - cameraPivotTransform.position;

                // Normaliz qilamiz
                dir.Normalize();

                // Quaternion tipida targetRotation yozamiz, LookRotation(tmongaga qarashga) dir ni yozamiz
                targetRotation = Quaternion.LookRotation(dir);

                // Vector3 tipida eularAngles yaratamiz va targetRotation ni eulerAnglesni yo'zlashtiramiz
                Vector3 eularAngles = targetRotation.eulerAngles;

                // eularAngles ni y o'qi 0 ga teng bo'ladi
                eularAngles.y = 0;

                // cameraPivotTransform ni localEulerAnglesiga eralar anglesni o'zlashtiramiz
                cameraPivotTransform.localEulerAngles = eularAngles;
                
            
            }


        }


        #endregion


        #region Camera Collision

        // Uchbu Funksiya Player Devorga Yaqinlashsa Camera 
        // Devordan O'tib Ketmidi Aksincha Camera Playerga Yaqinlashadi!
        // Camera Devordan tashqariga O'tib ketmidi
        private void HandleCameraCollision(float delta)
        {
            // TargetPositionga default Positionni o'rnatamiz
            targetposition = defaultPosition;

            // Raycast hit olamiz
            RaycastHit hit;
            // Vector3 da CameraTransformni positionini va CameraPivot Transformda Ayiramiz
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;

            // directionni Normolize qilamiz
            direction.Normalize();

            // Sphere Castdan PivotCamerani Radius Positionlarni Camerani Colission Bo'lishini taminlash
            // uchun Sozlaymiz
            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetposition), ignoreLayers))
            {
                // Vector3 orqali Camera Pivotni Masofasini O'lchimiz
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);

                // Target position ga dis dan cameracollisioffsetni ayirib o'zlashtiramzi
                targetposition = -(dis - cameraCollisionOffset);

            }

            // Agar target positionni Abstrakti minimumColissiondan Kichik Bo'lsa
            if (Mathf.Abs(targetposition) < minimumCollisionoffset)
            {
                // Targetpositionga MinimumCollisionoffsetni - holatida o'zlashtiramiz
                targetposition = -minimumCollisionoffset;
            }

            // Camera Transform Vector3 ni z o'lchamiga lerp yozamiz, lerpda CameraTransformni
            // Local z Positionidan targetposition nuqtasigacha delta / 0.2f Bo'lib Interpolatsiya qilamiz
            cameraTransformPostion.z = Mathf.Lerp(cameraTransform.localPosition.z, targetposition, delta / 0.2f);

            // CameraTransformni Local Positionini Camera transformga O'zlashtiramiz
            cameraTransform.localPosition = cameraTransformPostion;



        }


        #endregion

        #region Lock On Camera

        // HandLockOn Degan Metod Yaratamiz

        public void HandleLockOn()
        {
            // ShortDistance o'zgaruvchisini ochamiz va Mathf.Infinity (chechak qiymati) beramiz
            float shortDistance = Mathf.Infinity;
            
            // ShortDistanceTarget(right-left) o'zgaruvchisilarini ochamiz va Mathf.Infinity (chechak qiymati) beramiz
            float shortDistanceOfLeftTarget = Mathf.Infinity;
            float shortDistanceOfRightTarget = Mathf.Infinity;


            // Colliderlarni saqlash uchun massiv yaratamiz va Physics.OverlapSphere orqali targetTransform pozitsiyasiga radiusi 26 bo'lgan OverlaySphere qilingan colliderlarni olishimiz
            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            // colliders massivida aylanib chiqamiz
            for (int i = 0; i < colliders.Length; i++)
            {
                // colliders[i] obyektiga CharacterManager komponentini olish uchun GetComponent<CharacterManager>() metodi ishlatiladi va natija CharacterManager tipida bo'lgan obyektni character o'zgaruvchisiga saqlanadi
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                // Agar character null bo'lmasa (yani colliders[i] obyekti CharacterManager komponentiga ega bo'lsa)
                if (character != null)
                {
                    // lockTargetDirection o'zgaruvchisiga characterning pozitsiyasidan targetTransform pozitsiyasini ayirib olamiz
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;

                    // distanceFromTarget o'zgaruvchiga targetTransform pozitsiyasidan characterning pozitsiyasiga masofani o'lchaymiz
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);

                    // lockTargetDirection va cameraTransform.forward orasidagi burchakni olish uchun Vector3.Angle ishlatiladi va natija viewableAngle o'zgaruvchisiga saqlanadi
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    // Raycast hit ochamiz
                    RaycastHit hit;



                    // Agar character.transform.root targetTransform.transform.root ga teng bo'lmasa
                    // va viewableAngle -50 dan katta, 50 dan kichik va distanceFromTarget maximumLockOnDistance dan kichik yoki teng bo'lsa
                    if (character.transform.root != targetTransform.transform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                    {

                        // playerManager va character obyektlarining lockOnTransform komponentlaridan nuqtalarni olish va ular orasidagi liniyani tekshirish
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
{
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);

                            if (hit.transform.gameObject.layer == enviromentLayer)
                            {
                                // Liniya kesib o'tgan obyektning qatlami enviromentLayer ga teng bo'lsa

                                // Lock On ishlamidi

                            }
                            else
                            {
                                // Liniya kesib o'tgan obyektning qatlami enviromentLayer ga teng bo'lmasa

                                // character obyektini availableTargets listiga qo'shamiz
                                availableTargets.Add(character);

                                // targetTransform.transform.root.name ni Debug.Log orqali chiqaramiz
                                // Debug.Log(targetTransform.transform.root.name);
                            }
                        }


                        // Debug.Log orqali targetTransform.transform.root.name ni chiqaramiz
                        // Debug.Log(targetTransform.transform.root.name);



                    }
                }
            }



            for (int k = 0; k < availableTargets.Count; k++)
            {
                // availableTargets listidagi har bir obyekt uchun qatorni takrorlaymiz
                // va k chi indexdagi obyektni distanceFromTarget o'zgaruvchisiga
                // targetTransform pozitsiyasidan availableTargets[k] obyektining transform
                // pozitsiyasiga masofani o'lchaymiz
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);

  

                // Agar distanceFromTarget shortDistance dan kichik bo'lsa
                if (distanceFromTarget < shortDistance)
                {
                    // distanceFromTarget ni shortDistance ga o'rnatamiz
                    shortDistance = distanceFromTarget;

                    // availableTargets[k] obyektining lockOnTransformini nearestLockOnTarget ga o'rnatamiz
                    nearestLockOnTarget = availableTargets[k].lockOnTransform;

                    Debug.Log(nearestLockOnTarget + " Near");
                }

                if (inputHandler.lockOnFlag)
                {
                    // Nishonga qo'yilgan obyektlar orasidagi eng yaqin obyektni aniqlash
                    // va obyektning yo'nalishini hisoblash
                 
                        // currentLockOnTarget ni lokal koordinatalarida uchraydigan availableTargets[k] obyektining global koordinatalaridagi nuqtani topish
                        Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[k].transform.position);

                        // currentLockOnTarget ni x koordinatasidan availableTargets[k] ning x koordinatasini ayirish
                        var distanceFromLefttarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;
                        var distanceFromRighttarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;

 

                    // relativeEnemyPosition.x > 0 bo'lsa va distanceFromLefttarget shortDistanceOfLeftTarget dan kichik bo'lsa
                    // shortDistanceOfLeftTarget ni distanceFromLefttarget ga tenglashtirish va leftLockOnTarget ni availableTargets[k].lockOnTransform ga tenglashtirish
                    if (relativeEnemyPosition.x > 0.00 && distanceFromLefttarget < shortDistanceOfLeftTarget)
                        {
                            shortDistanceOfLeftTarget = distanceFromLefttarget;
                            leftLockOnTarget = availableTargets[k].lockOnTransform;
                        }

                        // relativeEnemyPosition.x > 0 bo'lsa va distanceFromRighttarget shortDistanceOfRightTarget dan kichik bo'lsa
                        // shortDistanceOfRightTarget ni distanceFromRighttarget ga tenglashtirish va rightLockOnTarget ni availableTargets[k].lockOnTransform ga tenglashtirish
                        if (relativeEnemyPosition.x < 0.00 && distanceFromRighttarget < shortDistanceOfRightTarget)
                        {
                            shortDistanceOfRightTarget = distanceFromRighttarget;
                            rightLockOnTarget = availableTargets[k].lockOnTransform;
                        }
                }

            }

       
        }


        // Cleer LockOn target ochamiz

        public void ClearLockOnTarget()
        {
            // availabletarget ichidagi listlarni tozalaymiz
            availableTargets.Clear();
            // currentLockOnTarget va nearestLockOnTarget ni null qilamiz
            currentLockOnTarget = null;
            nearestLockOnTarget = null;
        }


        // SetCameraHeight Metodini ochamiz

        public void SetCameraHeight()
        {

            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPositiong = new Vector3(0, lockedPivotPosition);
            Vector3 newUnLockedPositiong = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                // cameraPivotTransform.transform.localPosition ni newLockedPosition ga qarab SmoothDamp orqali yumshatamiz
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPositiong, ref velocity, Time.deltaTime);
            }
            else
            {
                // cameraPivotTransform.transform.localPosition ni transform.position va newUnlockedPosition ga qarab SmoothDamp orqali yumshatamiz
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnLockedPositiong, ref velocity, Time.deltaTime);
            }


        }

        #endregion

    }
}