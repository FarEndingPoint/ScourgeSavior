using System.Collections;
using UnityEngine;
using QFramework;

public class ActionSystem : AbstractSystem
{
    class Mono : MonoBehaviour
    {
    }
    GameObject go;
    Mono mono;

    float curActionTime;//当前动作剩余的时间
    float curPreInputWindow;
    bool isCurActionEnd;
    bool canPreInput;
    bool canEnterAction;//保证上一个动作结束后再开始下一个动作

    public float CurActionTime { get { return curActionTime; } }

    protected override void OnInit()
    {
        
    }

    public void Init()
    {
        go = new GameObject("ActionSystem");
        mono = go.AddComponent<Mono>();

        isCurActionEnd = true;
        canEnterAction = true;

        ClearCurAction();
    }

    public IEnumerator WaitCurAction()
    {
        while (!isCurActionEnd) yield return null;
        yield return null;
    }

    public bool IsInAction()
    {
        return curActionTime > 0;
    }

    public bool IsThisActionPreInput()//看这个动作是不是预输入进来的
    {
        return curActionTime > 0 && curActionTime <= curPreInputWindow;
    }

    public void ActEveryFrame()
    {
        curActionTime -= Time.deltaTime;
    }

    public bool CanEnterAction()
    {
        //下一个动作的输入要在当前动作的预输入窗口期内才允许进行
        //如果当前动作已接受预输入 则不能再接受预输入
        if (!canEnterAction || curActionTime > curPreInputWindow) return false;
        else if (curActionTime > 0)
            if (canPreInput)
            {
                canPreInput = false;
                return true;
            }
            else return false;
        else return true;
    }

    public void StartAction(float actionTime, float preInputWindow)//固定时间的动作
    {
        isCurActionEnd = false;
        curActionTime = actionTime;
        curPreInputWindow = preInputWindow;
    }

    public void EndAction(bool canPreInput = true)
    {
        isCurActionEnd = true;
        this.canPreInput = canPreInput;//一个动作结束时 都允许接下来的动作预输入
    }

    public void ClearCurAction()//强行退出当前动作
    {
        mono.StopCoroutine(WaitCurAction());
        curActionTime = curPreInputWindow = 0;
        canPreInput = true;
    }

    public void SetCanEnterAction(bool canEnterAction)//如果禁止下个动作进入 强行设置false
    {
        this.canEnterAction = canEnterAction;
    }
}
