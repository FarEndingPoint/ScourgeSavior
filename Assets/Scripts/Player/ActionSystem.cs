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

    float curActionTime;//��ǰ����ʣ���ʱ��
    float curPreInputWindow;
    bool isCurActionEnd;
    bool canPreInput;
    bool canEnterAction;//��֤��һ�������������ٿ�ʼ��һ������

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

    public bool IsThisActionPreInput()//����������ǲ���Ԥ���������
    {
        return curActionTime > 0 && curActionTime <= curPreInputWindow;
    }

    public void ActEveryFrame()
    {
        curActionTime -= Time.deltaTime;
    }

    public bool CanEnterAction()
    {
        //��һ������������Ҫ�ڵ�ǰ������Ԥ���봰�����ڲ��������
        //�����ǰ�����ѽ���Ԥ���� �����ٽ���Ԥ����
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

    public void StartAction(float actionTime, float preInputWindow)//�̶�ʱ��Ķ���
    {
        isCurActionEnd = false;
        curActionTime = actionTime;
        curPreInputWindow = preInputWindow;
    }

    public void EndAction(bool canPreInput = true)
    {
        isCurActionEnd = true;
        this.canPreInput = canPreInput;//һ����������ʱ ������������Ķ���Ԥ����
    }

    public void ClearCurAction()//ǿ���˳���ǰ����
    {
        mono.StopCoroutine(WaitCurAction());
        curActionTime = curPreInputWindow = 0;
        canPreInput = true;
    }

    public void SetCanEnterAction(bool canEnterAction)//�����ֹ�¸��������� ǿ������false
    {
        this.canEnterAction = canEnterAction;
    }
}
