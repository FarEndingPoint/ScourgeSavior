using ns;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlotEnd : MonoBehaviour
{
    void Start()
    {
        UIKit.OpenPanel<PlotEndPanel>();
        ActionKit.Delay(5, () =>
        {
            Application.Quit();
        }).Start(this);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) Application.Quit();
    }
}
