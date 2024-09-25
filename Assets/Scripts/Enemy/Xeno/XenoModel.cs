using UnityEngine;

public class XenoModel : EnemyModel
{
    [HideInInspector] public Vector3 rightScale = new Vector3(3, 3, 1);
    [HideInInspector] public Vector3 leftScale = new Vector3(-3, 3, 1);

    [Header("Xeno")]
    //战斗相关
    public float meetTimeToFoundPlayer = 0.5f;
    [HideInInspector] public bool isFoundPlayer = false;
    public float attackTime = 1.2f;
    [HideInInspector] public float curAttackTime;
    [HideInInspector] public bool canMove = true;
    public float beforeAttackTime = 1;
    [HideInInspector] public float curBeforeAttackTime;

    //Shoot相关
    public float beShootedTime = 0.2f;
    [HideInInspector] public float curBeShootedTime = 0;
    public float beShootedSpeed = 1;
    public float beChargeShootedTime = 0.5f;
    [HideInInspector] public float curBeChargeShootedTime = 0;
    public float beChargeShootedSpeed = 30;
    [HideInInspector] public Vector2 beShootedPoint;
    [HideInInspector] public Vector2 beShootedOriginalPos;
}
