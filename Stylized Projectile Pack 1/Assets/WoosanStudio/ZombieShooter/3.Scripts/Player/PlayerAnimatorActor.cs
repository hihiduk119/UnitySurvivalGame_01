using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.ZombieShooter
{
    public class PlayerAnimatorActor : MonoBehaviour
    {
        //에니메이션 컨트롤
        public Animator animator;

        //========================[PlayerActor 받은 Aim 상태]========================

        public void AimTarget(bool value)
        {
            //에니메이션 상태를 조준상태로 변경
            animator.SetBool("Aimed", value);
        }

        public void AimRelease(bool value)
        {
            //에니메이션 상태를 비조준상태로 변경
            animator.SetBool("Aimed", value);
        }

        public void Reload()
        {
            animator.SetTrigger("Reload");
        }

        
    }
}
