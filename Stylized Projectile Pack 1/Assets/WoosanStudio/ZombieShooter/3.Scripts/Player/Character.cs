using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

using System;

namespace WoosanStudio.ZombieShooter
{
    public class Character : MonoBehaviour
    {
        public static Character instance;
        //좀비의 공격
        public UnityAction<ZombieKinds> attackAction;

        //사거리에 들어온 좀비 리스트
        List<Transform> zombies = new List<Transform>();
        //사거리 관련 
        public Range range;
        //에니메이션 컨트롤
        public Animator animator;

        //네비메쉬 [실제 이동 담당]
        public NavMeshAgent navMeshAgent;

        //서드퍼슨 컨트롤 [ 애니메이션만 담당 , 회전도 담 당]
        public ThirdPersonCharacter thirdPersonCharacter;

        //조이스틱 기준 오브젝트
        public GameObject joystickPivot;

        //사격 컨트롤러
        public FireController fireController;

        float horizon;
        float vertical;
        Vector3 desiredVelocity;
        //캠의 방향으로 조이스틱 조정하기 위해 사용
        private Transform cam;                  
        private Vector3 camForward;
        //움직임 관련
        bool aimed = false;
        //조준이 되는 에임 마커
        public AimMaker aimMaker;

        //발사체 관련
        public projectileActor m_projectileActor;
        //현재 사격중인지 아닌지 여부
        //bool firing = false;
        bool isReloading = false;
        //Coroutine corFire;

        //현재 설정된 최대 총알수
        //int bulletMagazineMaxCount = 30;
        //현재 남은 총알수
        //int bulletMagazineCount = 0;

        //사격시 발생하는 BlobLight
        public GameObject objGunFireBlobLights;
        //임시 사용
        Vector3 tmpPos;
        float distance = 0.25f;

        //코루틴 람다식 형태
        IEnumerator WaitAndDo(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //네비게이션 세팅
            navMeshAgent.updateRotation = false;

            //좀비의 액션에 대한 콜백메서드 세팅
            attackAction = new UnityAction<ZombieKinds>(BeAttackedCallback);

            if (Camera.main != null)
            {
                cam = Camera.main.transform;
            }

            //재시작용 초기화
            Reset();

            //StartCoroutine(CorMoveJoystickPivot());
        }

        public void Reset()
        {
            //시작시 에임 마커 디스에이블
            aimMaker.gameObject.SetActive(false);
            //사격중지
            //m_projectileActor.Stop();
        }

        /// <summary>
        /// 모스터에게 공격받았을때 호출
        /// </summary>
        /// <param name="zombieKinds"></param>
        public void BeAttackedCallback(ZombieKinds zombieKinds)
        {
            Debug.Log("몬스터에게 공격받음");
        }

        private void FixedUpdate()
        {
            //정면을 볼지 타겟을 볼지 결정
            LookAtTarget();
            //이동
            Move();

            //Test code [Test 1]
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        /// <summary>
        /// 재장전 애니메이션 실행
        /// </summary>
        void Reload()
        {
            animator.SetTrigger("Reload");
            AudioManager.instance.OneShot(SoundOneshot.RifleOne_Reload_00);
            isReloading = true;
        }

        //재장전 애니메이션 완료시 호출
        public void ReloadEndCallback() {
            isReloading = false;
        }

        /// <summary>
        /// 이동 부분
        /// </summary>
        private void Move()
        {
            //실제 조이스틱 값 가져오는 부분
            horizon = UltimateJoystick.GetHorizontalAxis("Move");
            vertical = UltimateJoystick.GetVerticalAxis("Move");

            //Debug.Log("h = " + vertical + " v = " + vertical);

            if (cam != null)
            {
                //카메라 기준으로 조이스틱 방향성 바꿔줌
                camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
                desiredVelocity = vertical * camForward + horizon * cam.right;
            }
            else
            {
                //카메라가 없다면 기본 방향
                desiredVelocity = vertical * Vector3.forward + horizon * Vector3.right;
            }
            //실제 이동을 담당
            //navMeshAgent.destination = transform.position + desiredVelocity;


            //플레이어의 좌표와 왜곡 보정계산된 방향 가속도를 현재 좌표에 적용.
            tmpPos = this.transform.localPosition;
            tmpPos.z += desiredVelocity.z * distance;
            tmpPos.x += desiredVelocity.x * distance;

            //러프를 걸 타겟
            joystickPivot.transform.localPosition = tmpPos;

            //해당 지점으로 이동시키는 코드 => 조이스틱에 의 움직인 오브젝트임
            //navMeshAgent.SetDestination(joystickPivot.transform.position);
            navMeshAgent.destination = transform.position + desiredVelocity;
            //navMeshAgent.Move(joystickPivot.transform.position);
            //navMeshAgent.SetDestination(joystickPivot.transform.position);


            //조준 됐다면 타겟을 바라봐야함
            if (aimed)
            {
                Vector3 look = zombies[0].position - transform.position;
                look = look.normalized;

                //비활성화라면 활성화
                if (!aimMaker.gameObject.activeSelf)
                {
                    aimMaker.gameObject.SetActive(true);
                }
                //조준된 좀비에 에임 활성화
                aimMaker.SetValue(zombies[0], ZombieKinds.WeakZombie);
                //Debug.Log("look!");
                //가상패드 인식이 없을때 그냥 서서 총쏘는 애니메이션
                if (horizon == 0 && vertical == 0)
                {
                    thirdPersonCharacter.OnlyTurn(look, false, false);
                    //Debug.Log("정지");
                    return;
                }
                else
                {//가상패드 인식이 있을때는 걸어다니며 슈팅a
                    //애니메이션 움직임만 담당 [회전 포함]
                    thirdPersonCharacter.Move(look, false, false);
                    //Debug.Log("x = " + look.x +"  z = " + look.z);
                    //navMeshAgent.speed = 4;
                }
            }
            else
            {
                thirdPersonCharacter.Move(desiredVelocity, false, false);
            }
        }

        /// <summary>
        /// 정면을 볼지 타겟을 볼지 결정
        /// </summary>
        void LookAtTarget() 
        {
            //Debug.Log(zombies.Count);
            //좀비가 있다면
            if(zombies.Count > 0) {
                animator.SetBool("Aimed", true);
                aimed = true;
            } else {
                animator.SetBool("Aimed", false);
                aimed = false;

                //에임 비활성화 시키고 나에게로 가져오기
                aimMaker.SetValue(transform, ZombieKinds.WeakZombie);
                aimMaker.gameObject.SetActive(false);
            }
        }


        void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 150), "사격"))
            {
                m_projectileActor.Fire();
                //m_projectileActor.Switch(1);
            }

            //if (GUI.Button(new Rect(0, 150, 200, 150), "중지"))
            //{

            //}
        }
    }
}
