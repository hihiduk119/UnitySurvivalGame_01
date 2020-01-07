using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.AI;
using System;

using UnityStandardAssets.Characters.ThirdPerson;
using WoosanStudio.Common;

namespace WoosanStudio.ZombieShooter
{
    public class PlayerActor : MonoSingleton<PlayerActor>
    {
        public PlayerMoveActor playerMoveActor;

        //타겟 조준 및 릴리즈 타겟 이벤트
        public AimTargetEvent aimTargetEvent;
        public AimTargetEvent aimReleaseEvent;

        //재장전 이벤트
        public UnityEvent reloadEvent;

        //사격 시작 및 정지 이벤트
        public UnityEvent startFireEvent;
        public UnityEvent stopFireEvent;

        /// <summary>
        /// 움직임 상태
        /// </summary>
        enum MoveState
        {
            JoystickDirection,      //조이스틱 방향 주시
            LookAtTarget,           //타겟 주시
        }

        /// <summary>
        /// 가지고 있는 무기 가동 상태
        /// </summary>
        enum PlayerActionState
        {
            Release,    //기본
            Aimed,      //조준중
            //재장전 상태가 없는 이유는 조준 중에도 재장전이 가능하기때문

        }

        //좀비의 공격
        public UnityAction<ZombieKinds> attackAction;
        //사거리에 들어온 좀비 리스트
        List<Transform> zombies = new List<Transform>();
        
        //조준 됨
        bool aimed = false;
        //현재 리로딩 중
        bool isReloading = false;

        //조준중
        bool aimDone = false;

        //[Header("[Look At할 타겟들]")]
        private List<Transform> targets = new List<Transform>();
        //[Header("[Look At 타겟]")]
        private Transform fireTarget = null;
        private Transform preFireTarget = null;

        private PlayerActionState playerActionState;

        //캐쉬용 [CheckAimDone()]에서 사용
        private RaycastHit hit;
        private int layerMask = 1 << 8;

        //코루틴 람다식 형태
        IEnumerator WaitAndDo(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        private void Start()
        {
            //좀비의 액션에 대한 콜백메서드 세팅
            attackAction = new UnityAction<ZombieKinds>(BeAttackedCallback);
        }
        
        /// <summary>
        /// 모스터에게 공격받았을때 호출
        /// </summary>
        /// <param name="zombieKinds"></param>
        public void BeAttackedCallback(ZombieKinds zombieKinds)
        {
            Debug.Log("몬스터에게 공격받음");
        }

        /// <summary>
        /// FixedUpdate 사용시 부드럽지 않아서 기본 Update사용
        /// </summary>
        private void Update()
        {
            //Test code [Test 1]
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            //레이를 사용하여 타겟 조준이 완전히 완료 할때를 체크
            CheckAimDone();
            //조준할 타겟이 있는지 없는지 확인.
            CheckAimTarget();
        }

        /// <summary>
        ///레이를 사용하여 조준이 완전히 완료 할때를 체크
        ///현재 미완성
        /// </summary>
        void CheckAimDone()
        {
            layerMask = ~layerMask;

            if (Physics.Raycast(transform.position + new Vector3(0,0.75f,0), transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position + new Vector3(0, 0.75f, 0), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                aimDone = true;
            }
            else
            {
                Debug.DrawRay(transform.position + new Vector3(0, 0.75f, 0), transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                //Debug.Log("Did not Hit");
                aimDone = false;
            }
        }

        /// <summary>
        /// 재장전 애니메이션 실행
        /// </summary>
        void Reload()
        {
            //animator.SetTrigger("Reload");
            //사격 중지 이벤트 발생
            stopFireEvent.Invoke();

            reloadEvent.Invoke();
            AudioManager.Instance.OneShot(SoundOneshot.RifleOne_Reload_00);
            isReloading = true;
        }

        /// <summary>
        /// Reload 에니메이션이 끝나는 이벤트
        /// </summary>
        public void ReloadEnd()
        {
            //0.2초 대기후 리로딩 해제
            StartCoroutine(WaitAndDo(0.2f, () => {
                isReloading = false;
            }));
        }        

        //========================[Range에서 받은 타겟 컨트롤]========================



        /// <summary>
        /// 타겟 추가
        /// </summary>
        /// <param name="target">추가할 타겟</param>
        public void AddTarget(Transform target)
        {
            //리스트에서 기존에 있는지 없는지 확인[없다]
            if (!targets.Find(value => value.Equals(target.name)))
            {
                //없다면 추가
                targets.Add(target);
            }
        }

        /// <summary>
        /// 타겟 제거
        /// </summary>
        /// <param name="target">제거할 타겟</param>
        public void RemoveTarget(Transform target)
        {
            //리스트에서 기존에 있는지 없는지 확인
            if (targets.Find(value => value.name.Equals(target.name)))
            {
                //있다면 제거
                targets.RemoveAt(targets.FindIndex(value => value.name.Equals(target.name)));
            }
        }


        /// <summary>
        /// 조준할 타겟이 있는지 없는지 확인.
        /// </summary>
        void CheckAimTarget()
        {
            //쳐다볼 타겟이 존재한다
            if (targets.Count > 0)
            {
                aimed = true;
                //쳐다볼 타겟 세팅하기 => 가장 가까운 타겟 가져오기
                fireTarget = WoosanStudio.Common.TargetUtililty.GetNearestTarget(targets, transform);

                //이전에 타겟이 지금것과 같다면 타겟 조준 안함
                if (preFireTarget == fireTarget) return;

                //타겟 조준 이벤트 발생
                aimTargetEvent.Invoke(aimed);

                //리로딩 중이 아닐때
                if (!isReloading)
                {
                    //사격 이벤트 발생
                    startFireEvent.Invoke();
                }

                preFireTarget = fireTarget;

                Debug.Log("조준");
            }
            else
            {
                //이미 한번 해제 이벤트가 있어 났다면
                if (!aimed) return;

                aimed = false;
                //조준 해제 이벤트 발생
                aimReleaseEvent.Invoke(aimed); 

                //사격 중지 이벤트 발생
                stopFireEvent.Invoke();

                Debug.Log("조준해제");
            }

            //리로딩 상태
            //삽탄 상태
            ///조준 상태
            ////완전한 조준이 된상태
            ///비조준 상태
        }

        //void OnGUI()
        //{
        //    if (GUI.Button(new Rect(0, 0, 200, 150), "사격"))
        //    {
        //    }

        //    if (GUI.Button(new Rect(0, 150, 200, 150), "중지"))
        //    {

        //    }
        //}
    }
}
