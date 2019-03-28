using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMaker : MonoBehaviour
{
    //기본 되는 표적 포지션
    readonly private Vector3 defaultPosition = new Vector3(0f, 0.07f, 0f);
    readonly private Vector3 defaultRotation = new Vector3(90f, 0, 0);
    //3가지 종류 표적 사이즈 [ZombieKinds와 인덱스가 같다]
    readonly private List<Vector3> scales = new List<Vector3>() { new Vector3(1.2f, 1.2f, 1.2f), new Vector3(1.5f, 1.5f, 1.5f), new Vector3(2f, 2f, 2f)};

    public void SetValue(Transform target,ZombieKinds kinds) 
    {
        //이미 타겟 되어있다면 종료
        if (transform.parent.Equals(target)) return;

        transform.parent = target;
        transform.localPosition = defaultPosition;
        transform.localRotation = Quaternion.Euler(defaultRotation);
        //ZombieKinds의 순서와 스캐일은 index가 같아야 한다
        transform.localScale = scales[(int)kinds];
    }
}
