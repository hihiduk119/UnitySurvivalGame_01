using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoosanStudio.ZombieShooter
{
    public abstract class Enemy : MonoBehaviour
    {
        public bool IsAlive { get; set; } = false;
    }
}
