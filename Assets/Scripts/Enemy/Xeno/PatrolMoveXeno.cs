using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PatrolMoveXeno
{
    float patrolRadius { get; }
    float patrolSpeed { get; }
    float patrolTime { get; }
    Vector3 patrolCenter { get; set; }
    Vector3 patrolPoint { get; set; }
    float curPatrolTime { get; set; }
}
