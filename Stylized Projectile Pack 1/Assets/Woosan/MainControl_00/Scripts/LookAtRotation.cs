using UnityEngine;

public class LookAtRotation : MonoBehaviour
{
    public Transform target;

    void Update() {

        Vector3 relativePos = target.position - transform.position;

        transform.rotation = Quaternion.LookRotation(relativePos);

    }
}
