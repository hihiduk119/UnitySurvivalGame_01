using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Woosan.SurvivalGame
{
    public abstract class Enemy : MonoBehaviour
    {
        public bool IsAlive { get; set; } = false;
    }
}
