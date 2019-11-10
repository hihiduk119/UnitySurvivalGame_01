using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//화면에서 목적 Pivot 에 따라 다니게 하기위해 만든 녀석a
public class CamFollow : MonoBehaviour
{
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
