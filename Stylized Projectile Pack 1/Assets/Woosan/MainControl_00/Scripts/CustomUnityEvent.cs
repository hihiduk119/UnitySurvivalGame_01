using UnityEngine;
using UnityEngine.Events;

//좀비의 어택 이벤트
public class AttackEvent : UnityEvent<ZombieKinds> {}
//에임 사거리 용 이벤트
public class TransformUnityEvent : UnityEvent<Transform> {}
