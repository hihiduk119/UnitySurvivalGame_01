using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WoosanStudio.Common;

namespace WoosanStudio.ZombieShooter
{
    
    public class Range : MonoBehaviour
    {
        //enter 이벤트
        public TransformUnityEvent triggerEnterEvent = new TransformUnityEvent();
        //exit 이벤트
        public TransformUnityEvent triggerExitEvent = new TransformUnityEvent();

        //주변에 인식 부분
        private SphereCollider coll;
        //사거리 표시 부분
        private Transform view;

        //캐쉬용
        Material material;

        private void Awake()
        {
            //콜라이더 세팅
            this.coll = this.transform.GetComponent<SphereCollider>();
            //실제 사거리 스케일
            this.view = this.transform.GetChild(0);

            material = view.GetComponent<MeshRenderer>().sharedMaterial;
            //깜빡임 트윈 시작
            //Blink();
        }

        //실제 View 에서 보이는 사거리 세팅
        public void SetRange(float radius)
        {
            this.coll.radius = radius/2;
            this.view.localScale = new Vector3(radius, radius, radius);
        }

        //Enter 이벤트 발생
        private void OnTriggerEnter(Collider other)
        {
            //레인지 스크립트 디버그 활성화
            if (GameManager.Instance.onDebug.OnDebugForRange)
            {
                Debug.Log("[OnTriggerEnter] name =  " + other.name);
            }

            //레이어가 "좀비"
            if (other.gameObject.layer == LayerMask.NameToLayer("zombie"))
            {
                //이벤트 발생
                this.triggerEnterEvent.Invoke(other.transform);
            }
        }

        //Exit 이벤트 발생
        private void OnTriggerExit(Collider other)
        {
            //레인지 스크립트 디버그 활성화
            if (GameManager.Instance.onDebug.OnDebugForRange)
            {
                Debug.Log("[nTriggerExit] name =  " + other.name);
            }

            //레이어가 "좀비"
            if (other.gameObject.layer == LayerMask.NameToLayer("zombie"))
            {
                //이벤트 발생
                this.triggerExitEvent.Invoke(other.transform);
            }
        }
    }
}
