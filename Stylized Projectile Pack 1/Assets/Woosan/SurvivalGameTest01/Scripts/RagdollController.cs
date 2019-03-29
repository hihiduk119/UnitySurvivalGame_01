using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Woosan.SurvivalGame
{
    public class RagdollController : MonoBehaviour
    {
        List<Rigidbody> rigidbodies;

        public Animator animator;
        public CapsuleCollider capsuleCollider;
        public Transform root;
        public Transform bullet;
        public NavMeshAgent navMeshAgent ;

        Vector3 initPos;

        private void Awake()
        {
            //최초 시작 포지션 세팅
            initPos = root.transform.position;

            rigidbodies = new List<Rigidbody>(this.transform.GetComponentsInChildren<Rigidbody>());

            //해당 리지드바지 가져오기
            //Rigidbody rd = rigidbodies[rigidbodies.FindIndex(value => value.gameObject.name.Equals("aa"))];

            Debug.Log("count = " + rigidbodies.Count);

            DisableRagdoll();
        }

        /// <summary>
        /// 레그돌 활성화
        /// </summary>
        public void EnableRagdoll()
        {
            animator.enabled = false;
            capsuleCollider.enabled = false;
            if(navMeshAgent != null){ navMeshAgent.enabled = false; }

            rigidbodies.ForEach(value => {
                value.useGravity = true;
                value.isKinematic = false;
                value.detectCollisions = true;
            });
        }

        /// <summary>
        /// 레그돌 비활성화
        /// </summary>
        public void DisableRagdoll()
        {
            rigidbodies.ForEach(value => {
                value.isKinematic = true;
                value.detectCollisions = false;
            });

            animator.enabled = true;
            capsuleCollider.enabled = true;
            if (navMeshAgent != null) { navMeshAgent.enabled = true; }

            //Recall에 의해 꼬인 자체 포지션 및 로테이션 초기화
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            //루트 포지션 초기화 ["SetDestination"에러가 나기 때문에 이걸로 초기화 해야함.]
            navMeshAgent.Warp(initPos);
        }

        public void Die()
        {
            Vector3 forceDir = (transform.position - bullet.position).normalized;
            float power = Random.Range(100,350);
            forceDir = forceDir * power;
            forceDir.y = Random.Range(400,800);

            rigidbodies.ForEach(value => {
                value.AddForce(forceDir, ForceMode.Force);
            });
        }

        public void Recall()
        {
            rigidbodies.ForEach(value => {
                value.useGravity = false;
                value.AddForce(new Vector3(0, Random.Range(200,400), 0), ForceMode.Acceleration);
            });
        }

        /*void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 150), "레그돌 활성화"))
            {
                EnableRagdoll();
            }

            
            if (GUI.Button(new Rect(0, 150, 200, 150), "레그돌 비활성화"))
            {
                DisableRagdoll();
            }

            if (GUI.Button(new Rect(0, 300, 200, 150), "죽음"))
            { 
                EnableRagdoll();
                Die();
            }

            if (GUI.Button(new Rect(0, 450, 200, 150), "리콜"))
            {
                Recall();
            }
        }*/
    }
}
