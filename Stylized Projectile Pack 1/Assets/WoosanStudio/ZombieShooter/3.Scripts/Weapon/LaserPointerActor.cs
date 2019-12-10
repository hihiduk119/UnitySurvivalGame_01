using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// 무기에서 레이저 포인터 컨트롤
    /// </summary>
    public class LaserPointerActor : MonoBehaviour
    {
        [Header("[총기의 레이저 포인터 리스트[총기 많아 졌을때 무기 교체시 무기마다 레이저포인터 세팅되게 해야함]]")]
        public List<Transform> laserPointers = new List<Transform>();
        [Header("[라인 랜더 세팅]")]
        public LineRenderer lineRenderer;
        [Header ("[보여주기 토글]")]
        public bool isVisible = true;
        //캐슁
        Vector3 tmpPos;

        private void Update()
        {
            if (!isVisible) return;

            lineRenderer.SetPosition(0, laserPointers[0].position);
            tmpPos = laserPointers[0].TransformPoint(new Vector3(0, 0, 10f));
            lineRenderer.SetPosition(1, tmpPos);
        }
    }
}
