using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SmashedStar : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Shake());
    }

    private void OnDisable()
    {
        StopCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        while (true) 
        {
            transform.DOShakePosition(0.3f, 0.1f);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
