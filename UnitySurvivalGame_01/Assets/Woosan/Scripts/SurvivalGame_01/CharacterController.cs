using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace WoosanStudio.SurvivalGame00 {
    /// <summary>
    /// 캐릭터에 붙어 사용되는 컨트롤러.
    /// </summary>
    public class CharacterController : MonoBehaviour
    {
        //걸린 모든 적과 리소스를 체크하기 위해 담아두는 리스트
        private List<string> triggedNames = new List<string>();

        [Header ("[손]")]
        public GameObject objArm;

        //손을 사용 가능한 상태이냐?
        bool ableArm = true;

        private IEnumerator Start()
        {
            while(true) {
                yield return new WaitForSeconds(0.05f);

                if(this.triggedNames.Count > 0) this.ArmAction();
            }
        }

        /// <summary>
        /// 트리거에 걸린 적과 자원을 체크 및 등록
        /// </summary>
        /// <param name="other">Other.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //Debug.Log(other.name);
                this.triggedNames.Add(other.name);
                //this.ArmAction();
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Resources")) {
                //Debug.Log(other.name);
                this.triggedNames.Add(other.name);
                //this.ArmAction();
            }
        }

        /// <summary>
        /// 트리거에서 나갈때 적과 자원을 체크 및 해제
        /// </summary>
        /// <param name="other">Other.</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //Debug.Log(other.name);
                this.triggedNames.Remove(other.name);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Resources"))
            {
                this.triggedNames.Remove(other.name);
            }
        }

        /// <summary>
        /// 적이 죽었을때 리스트에서 해제
        /// </summary>
        /// <param name="name">Name.</param>
        public void EnemyDead(string name) {
            this.triggedNames.Remove(name);
        }

        /// <summary>
        /// 손 액션
        /// </summary>
        public void ArmAction()
        {
            if (!this.ableArm) return;
            //손 액션시 팔 사용 불가
            this.ableArm = false;
            this.objArm.transform.DOLocalRotate(new Vector3(0, 360, 0), 0.3f).SetRelative(true).SetEase(Ease.Linear).OnComplete(()=>this.ableArm = true);
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 200, 150), "arm action"))
            {
                this.ArmAction();
            }
        }
    }
}
