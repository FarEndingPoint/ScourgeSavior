using ns;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Texture2D cursor;

    private void Awake()
    {
        ResKit.Init();
    }

    private void Start()
    {
        UIKit.OpenPanel<MainMenuPanel>();
        Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
    }
}