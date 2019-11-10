using UnityEngine;
using UnityEngine.Events;

using DG.Tweening;

namespace Woosan.SurvivalGame
{
    public class Range : MonoBehaviour
    {
        //enter 이벤트
        public TransformUnityEvent triggerEnterEvent = new TransformUnityEvent();
        //exit 이벤트
        public TransformUnityEvent triggerExitEvent = new TransformUnityEvent();
        //
        //public UnityAction detectedAction;

        //주변에 인식 부분
        private SphereCollider coll;
        //사거리 표시 부분
        private Transform view;

        Material material;

        private void Awake()
        {
            //콜라이더 세팅
            this.coll = this.transform.GetComponent<SphereCollider>();
            //실제 사거리 스케일
            this.view = this.transform.GetChild(0);

            material = view.GetComponent<MeshRenderer>().sharedMaterial;
            //깜빡임 트윈 시작
            Blink();
        }

        void Blink() 
        {
            material.DOFade(0.05f, "_TintColor", 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
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
            if (other.gameObject.layer == LayerMask.NameToLayer("zombie"))
            {
                //Debug.Log("OnTriggerEnter");
                this.triggerEnterEvent.Invoke(other.transform);
            }
        }

        //Exit 이벤트 발생
        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("OnTriggerExit");
            if (other.gameObject.layer == LayerMask.NameToLayer("zombie"))
            {
                this.triggerExitEvent.Invoke(other.transform);
            }
        }
    }
}
