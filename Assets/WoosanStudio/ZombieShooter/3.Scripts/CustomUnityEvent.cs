using UnityEngine;
using UnityEngine.Events;
using System;

//좀비의 어택 이벤트
namespace WoosanStudio.ZombieShooter
{
    [Serializable]
    public class AttackEvent : UnityEvent<ZombieKinds> { }
    [Serializable]
    public class AimTargetEvent : UnityEvent<bool> { }
    //에임 사거리 용 이벤트
    [Serializable]
    public class TransformUnityEvent : UnityEvent<Transform> { }
}
