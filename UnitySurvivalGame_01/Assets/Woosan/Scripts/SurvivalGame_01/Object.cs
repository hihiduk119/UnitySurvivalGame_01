using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.Common {
    public class Object : MonoBehaviour {
        [Header("해당 매니저의 타겟은 여기에 : ")]
        public Transform tfBaseTarget;
        [HideInInspector] public Vector3 initPos;
        [HideInInspector] public Vector3 initScale;
        [HideInInspector] public Quaternion initRot;

        public void Init () {
            if(this.tfBaseTarget != null) {
                this.initPos = this.tfBaseTarget.localPosition;
                this.initScale = this.tfBaseTarget.localScale;
                this.initRot = this.tfBaseTarget.localRotation;
            }
        }
    }    
}

