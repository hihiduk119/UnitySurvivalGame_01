using System;
using UnityEngine;

using UnityEngine.Events;
using WoosanStudio.ZombieShooter;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    //[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        UnityEngine.AI.NavMeshAgent agent;     // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        //타겟간의 거리 유지
        float distance = 10f;
        Animator animator;
        AttackEvent attackEvent;
        ZombieKinds zombieKinds = ZombieKinds.WeakZombie;

        //모든 좀비 데이터 가지고 있음
        Zombie zombie;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            //시작하자 마자 현재 가지고 있는 에니메이터 세팅
            animator = this.GetComponent<Animator>();
            attackEvent = new AttackEvent();

            //모든 좀비 데이터 가지고 있음
            zombie = GetComponent<Zombie>();
        }


        private void Update()
        {
            //좀비가 죽었다면 실행 안함
            if (zombie.model.isDead) return;

            //타겟이 있어야 하고 Nav가 활성화 되어있어야 한다.
            if (target != null && agent.enabled == true) {
                //Debug.Log("상태 = " + agent.enabled);

                agent.SetDestination(target.position);
            }  else { return; }

            //좀비 사거리 확인용
            //Debug.Log("re dis = " + agent.remainingDistance + "   stop dis = " + agent.stoppingDistance);

            //이동
            if (agent.remainingDistance > agent.stoppingDistance) {
                character.Move(agent.desiredVelocity, false, false);
                //공격 애니메이션 호출
                animator.SetBool("Attacking", false);
            } else {//캐릭터에게 공격
                character.Move(Vector3.zero, false, false);
                //공격 애니메이션 호출
                animator.SetBool("Attacking", true);
                //공격 이벤트 호출 
                attackEvent.Invoke(zombieKinds);
            }
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
            //타겟 설정시 리스너에 콜벡 넣어주기
            attackEvent.AddListener(target.GetComponent<Character>().attackAction);
        }
    }
}
