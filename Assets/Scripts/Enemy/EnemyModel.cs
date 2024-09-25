using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public abstract class EnemyModel : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] int hp;
    public BindableProperty<int> _hp;

    //受击相关
    static public Color common = new Color(1, 1, 1, 1);
    static public Color beHited1 = new Color(0.172549f, 0.909804f, 0.9607844f, 1);
    static public Color beHited2 = new Color(1, 0.682353f, 0.1568628f, 1);
    //static public Color beSmashed = new Color(0.18104f, 0.408805f, 0.08870292f, 1);
    public float beHitedDisplayTime = 0.2f;
    [HideInInspector] public float curBeHitedDisplayTime = 0;
    [HideInInspector] public bool isTrun = true;

    //Smash相关
    public bool canBeSmashed;
    public float beSmashedTime = 1;
    public float beCriticalSmashedTime = 1.5f;
    [HideInInspector] public float curBeSmashedTime;
    public float bePushedTime = 0.7f;
    [HideInInspector] public float curBePushedTime;
    public float bePushedSpeed = 5;
    [HideInInspector] public Vector2 bePushedDestination;
    [HideInInspector] public Vector2 bePushedOriginalPos;

    //Dash相关
    public bool canBeDraged = true;
    public float beDashedSlowScale;

    //Fury相关
    public float afterFuryTime = 1;
    [HideInInspector] public float curafterFuryTime;

    protected virtual void Awake()
    {
        _hp = new BindableProperty<int>(hp);
    }

    protected virtual void Start()
    {
        _hp.Register(e =>
        {
            if (_hp.Value <= 0) GetComponent<Enemy>().Death();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
}
