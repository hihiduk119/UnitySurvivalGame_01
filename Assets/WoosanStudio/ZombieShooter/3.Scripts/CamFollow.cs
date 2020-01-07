using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//화면에서 목적 Pivot 에 따라 다니게 하기위해 만든 녀석

/// <summary>
/// Target을 계속 따라 다니는 스크립트
/// ex) 카메라를 달아두면 카메라를 계속 따라 다님.
/// </summary>
public class CamFollow : MonoBehaviour
{
    [Header ("[따라다닐 타겟 설정]")]
    public Transform target;
    Vector3 gab;

    private void Start()
    {
        gab = target.position - transform.position;
    }

    void Update()
    {
        transform.localPosition = target.position - gab;    
    }
}
