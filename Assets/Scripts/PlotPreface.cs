using ns;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlotPreface : MonoBehaviour
{
    void Start()
    {
        UIKit.OpenPanel<PlotPanel>();
        ActionKit.Delay(12, () =>
        {
            UIKit.ClosePanel<PlotPanel>();
            SceneManager.LoadScene(1);
        }).Start(this);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            UIKit.ClosePanel<PlotPanel>();
            SceneManager.LoadScene(1);
        }
    }
}
