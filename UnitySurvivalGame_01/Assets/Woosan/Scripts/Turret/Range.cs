using UnityEngine;
using UnityEngine.Events;

namespace WoosanStudio.Turret
{
    public class Range : MonoBehaviour
    {
        //찾았을때 이벤트
        public UnityEvent detectedEvent;
        //
        public UnityAction detectedAction;

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

            this.detectedEvent.AddListener(() => { Debug.Log("Enemy Detected!!"); });
        }

        //사거리 세팅
        public void SetRange(float radius)
        {
            this.coll.radius = radius/2;
            this.view.localScale = new Vector3(radius, radius, radius);
        }

        //이벤트 발생
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                this.detectedEvent.Invoke();
            }
        }
    }
}
