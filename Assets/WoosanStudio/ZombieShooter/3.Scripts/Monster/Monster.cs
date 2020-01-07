using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.AI;

namespace WoosanStudio.ZombieShooter
{
    public class Monster : MonoBehaviour
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void ParameterReset()
        {
            animator.SetBool("Idle",    false);
            animator.SetBool("Walk",    false);
            animator.SetBool("Chase",   false);
        }

        public enum AttackType
        {
            Leap,                       //해당 영역 도약
            Dash,                       //해당 영역 돌진
            UnstoppableDash,            //직선 영역 돌진
            UnBurrow,                   //해당 영역 애어본
            ProjectileLaunch,           //투사체를 발사한다.
            ProjectileDrop,             //하늘에서 떨구는 방식
            LaneAttack,                 //해당 라인 전체에 데미지
            ContinuousAngleArea,        //화염방사기
            ContinuousCircleArea,
        }

        //캐릭터 상태
        //속박, 에어본 , 슬로우 , 스턴

        public enum MoveType
        {
            Hold,       //고정 상태
            Walk,       //걷기
            Run,        //달리기
            Burrow,     //땅굴서 이동
            Teleport,   //순간 이동
            Jump,       //점프 이동
            HighJump,   //높이 뛰기 이동
        }

        public enum MonsterState
        {
            Patrol,
            Investigate,
            Battle,
            None,
        }

        public MonsterState _state;

        public MonsterState State
        {
            get => _state;
            set
            {
                ExitState(_state);
                _state = value;
                EnterState(_state);
            }
        }

        //몬스터 행동 시퀀스
        //랜덤으로 로밍 다님
        //플레이어가 일정 사거리 내 접근하면 일정시간 쳐다봄
        //쳐다봄 끝난후 공격시작

        private NavMeshAgent navAgent;

        public Transform player;

        private float patrolSpeed = 10.5f;

        Job roam;
        Job see;
        Job attack;

        public UnityEvent roamingEvent;
        public UnityEvent seeingEvent;
        public UnityEvent attackingEvent;



        void Initialize()
        {
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.updatePosition = true;
            navAgent.updateRotation = true;
        }

        private void Start()
        {
            Initialize();

            StartCoroutine(Roaming());
            //StartCoroutine(Seeing());

            //State = MonsterState.Patrol;

        }

        /// <summary>
        /// 몬스터 스테이터를 세팅할때마다 자동 호출
        /// </summary>
        /// <param name="state"></param>
        void EnterState(MonsterState state)
        {
            switch (state)
            {
                case MonsterState.Patrol:
                    roam = new Job(Roaming());
                    break;
                case MonsterState.Investigate:

                    break;
                case MonsterState.Battle:

                    break;
            }
        }

        /// <summary>
        /// 몬스터 스테이터를 세팅할때마다 자동 호출
        /// </summary>
        /// <param name="state"></param>
        void ExitState(MonsterState state)
        {
            switch (state)
            {
                case MonsterState.Patrol:
                    if (roam != null) roam.kill();
                    break;
                case MonsterState.Investigate:

                    break;
                case MonsterState.Battle:

                    break;
            }
        }


        IEnumerator Roaming()
        {
            Vector3 pos;
            navAgent.updatePosition = true;
            while (true)
            {
                navAgent.speed = patrolSpeed;

                pos = (UnityEngine.Random.insideUnitSphere * 3) + transform.position;
                pos.y = 1f;
                navAgent.SetDestination(pos);

                


                yield return new WaitForSeconds(3f);

            }
        }

        //쳐다보기
        IEnumerator Seeing()
        {
            navAgent.updatePosition = false;
            navAgent.updateRotation = false;
            Vector3 direction;
            Quaternion lookRotation;

            while (true)
            {
                direction = (player.position - transform.position).normalized;
                lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                yield return new WaitForEndOfFrame();
            }
        }


        /*public bool VisionCheck(Transform target, float distance)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, target.position - transform.position, out hit, distance))
            {
                if (hit.transform == target) return true;
                else return false;
            }
            else return false;
        }*/


        IEnumerator Test()
        {
            while (true)
            {
                Debug.Log("Gie");

                //yield return null;

                yield return new WaitForSeconds(1f);
            }
        }


        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 150), "걷기"))
            {
                ParameterReset();
                animator.SetBool("Walk", true);
            }

            if (GUI.Button(new Rect(0, 150, 200, 150), "공격"))
            {
                ParameterReset();
                animator.SetTrigger("Attack");
            }

            if (GUI.Button(new Rect(0, 300, 200, 150), "Idle"))
            {
                ParameterReset();
                animator.SetBool("Idle", true);
            }

            if (GUI.Button(new Rect(0, 450, 200, 150), "추적"))
            {
                ParameterReset();
                animator.SetBool("Chase", true);
            }
        }
    }
}
