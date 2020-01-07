using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UnityStandardAssets.Characters.ThirdPerson;

namespace WoosanStudio.ZombieShooter
{
    public class PlayerMoveActor : MonoBehaviour
    {
        //에니메이션 컨트롤
        //public Animator animator;

        //네비메쉬 [실제 이동 담당]
        public UnityEngine.AI.NavMeshAgent navMeshAgent;

        //서드퍼슨 컨트롤 [ 애니메이션만 담당 , 회전도 담 당]
        public ThirdPersonCharacter thirdPersonCharacter;

        //조이스틱 기준 오브젝트
        public GameObject joystickPivot;

        //사격 컨트롤러
        //public FireActor fireActor;

        float horizon;
        float vertical;
        Vector3 desiredVelocity;

        //캠의 방향으로 조이스틱 조정하기 위해 사용
        private Transform cam;
        private Vector3 camForward;
        //움직임 관련
        public bool aimed = false;

        //[Header("[Look At할 타겟들]")]
        private List<Transform> targets = new List<Transform>();
        //[Header("[Look At 타겟]")]
        private Transform fireTarget = null;

        //코루틴 람다식 형태
        IEnumerator WaitAndDo(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        private void Start()
        {
            //네비게이션 세팅
            navMeshAgent.updateRotation = false;

            if (Camera.main != null)
            {
                cam = Camera.main.transform;
            }

            //NavMeshAgent를 사용하여 실제 조이스틱 값을 사용해서 움직이는 부분
            StartCoroutine(CorNavMove());
        }

        /// <summary>
        /// NavMeshAgent를 사용하여 실제 조이스틱 값을 사용해서 움직이는 부분
        /// NavMeshAgent 는 이동에만 사용.
        /// </summary>
        /// <returns></returns>
        IEnumerator CorNavMove()
        {
            //값으 높을수록 좋다. 퍼포먼스 생각하면 값 세팅
            WaitForSeconds WFS = new WaitForSeconds(0.1f);
            while (true)
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

                //네비메쉬에이전트에 값 집어넣어서 실제 움직임
                navMeshAgent.destination = transform.position + desiredVelocity;
                yield return WFS;
            }
        }

        /// <summary>
        /// FixedUpdate 사용시 부드럽지 않아서 기본 Update사용
        /// </summary>
        private void Update()
        {
            //앞으로 걸을지 뒷걸을 칠지 결정
            MoveStateControl();
        }

        /// <summary>
        /// 앞으로 걷을지 뒷걸을 칠지 결정
        /// 회전도 결정함. 회전은 서드퍼슨캐릭터 사용
        /// </summary>
        private void MoveStateControl()
        {

            //실제 이동을 담당
            //navMeshAgent.destination = transform.position + desiredVelocity;

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

            //1.번 방식 말 머리 앞에 당근 놓아 쫒아가게 하는 방식
            //플레이어의 좌표와 왜곡 보정계산된 방향 가속도를 현재 좌표에 적용.
            //tmpPos = this.transform.localPosition;
            //tmpPos.z += desiredVelocity.z * distance;
            //tmpPos.x += desiredVelocity.x * distance;
            //joystickPivot.transform.localPosition = tmpPos;
            //해당 지점으로 이동시키는 코드 => 조이스틱에 의 움직인 오브젝트임
            //navMeshAgent.SetDestination(joystickPivot.transform.position);

            //정면을 볼지 타겟을 볼지 결정
            //aimed = AimTarget();

            //조준 됐다면 캐릭터의 포지션을 타겟을 바라보게 함.
            //aimed 값은 PlayerActor 의 이벤트에 의해서 값 설정됨
            if (aimed)
            {
                //Vector3 look = zombies[0].position - transform.position;

                if (fireTarget == null) {
                    Debug.Log("[" +this.name+ "] fireTarget 널!!");
                    return;
                }
                

                Vector3 look = fireTarget.position - transform.position;
                look = look.normalized;

                //비활성화라면 활성화
                //if (!aimMaker.gameObject.activeSelf)
                //{
                //    aimMaker.gameObject.SetActive(true);
                //}
                ////조준된 좀비에 에임 활성화
                //aimMaker.SetValue(zombies[0], ZombieKinds.WeakZombie);

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
                }
            }//비조준 상태시 전방 주시
            else
            {
                thirdPersonCharacter.Move(desiredVelocity, false, false);
            }
        }

        /// <summary>
        /// 정면을 볼지 타겟을 볼지 결정
        /// </summary>
        /*bool AimTarget()
        {
            //Debug.Log("["+this.name+"] 타겟 수 = " + targets.Count);

            //쳐다볼 타겟이 존재한다
            if (targets.Count > 0)
            {
                //에니메이션 상태를 조준상태로 변경
                animator.SetBool("Aimed", true);
                aimed = true;

                //쳐다볼 타겟 세팅하기 => 가장 가까운 타겟 가져오기
                fireTarget = WoosanStudio.Common.TargetUtililty.GetNearestTarget(targets, transform);
            }
            else
            {
                //에니메이션 상태를 비조준상태로 변경
                animator.SetBool("Aimed", false);
                aimed = false;
            }

            return aimed;
        }*/



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



        //========================[PlayerActor 받은 Aim 상태]========================


        public void AimTarget(bool value)
        {
            this.aimed = value;

            //쳐다볼 타겟 세팅하기 => 가장 가까운 타겟 가져오기
            fireTarget = WoosanStudio.Common.TargetUtililty.GetNearestTarget(targets, transform);
        }

        public void AimRelease(bool value)
        {
            this.aimed = value;
        }
    }
}
