using UnityEngine;
using UnityEngine.Events;

namespace WoosanStudio.Turret
{
    public class Range : MonoBehaviour
    {
        //enter 이벤트
        public TransformUnityEvent triggerEnterEvent;
        //exit 이벤트
        public UnityEvent triggerExitEvent;
        //
        //public UnityAction detectedAction;

        //주변에 인식 부분
        private SphereCollider coll;
        //사거리 표시 부분
        private Transform view;

        private void Awake()
        {
            //콜라이더 세팅
            this.coll = this.transform.GetComponent<SphereCollider>();
            //실제 사거리 스케일
            this.view = this.transform.GetChild(0);
            //이벤트 메모리 할당
            this.triggerEnterEvent = new TransformUnityEvent();
            //이벤트 메모리 할당
            this.triggerExitEvent = new UnityEvent();
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
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //Debug.Log("OnTriggerEnter");
                this.triggerEnterEvent.Invoke(other.transform);
            }
        }

        //Exit 이벤트 발생
        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("OnTriggerExit");
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                this.triggerExitEvent.Invoke();
            }
        }
    }
}
