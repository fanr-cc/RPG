using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour
{
    public static CursorManager _instance;

    public Texture2D mouseCursor_normal;
    public Texture2D mouseCursor_talk;
    public Texture2D mouseCursor_attack;
    public Texture2D mouseCursor_lockTarget;
    public Texture2D mouseCursor_pick;

    void Awake()
    {
        _instance = this;
    }

    //更改鼠标指针，
    public void SetNormal()
    {
        Cursor.SetCursor(mouseCursor_normal, Vector2.zero, CursorMode.Auto);
    }
    public void SetNpcTalk()
    {
        Cursor.SetCursor(mouseCursor_talk, Vector2.zero, CursorMode.Auto);
    }
    public void SetAttack()
    {
        Cursor.SetCursor(mouseCursor_attack, Vector2.zero, CursorMode.Auto);
    }
    public void SetLockTarget()
    {
        Cursor.SetCursor(mouseCursor_lockTarget, Vector2.zero, CursorMode.Auto);
    }
    public void SetPick()
    {
        Cursor.SetCursor(mouseCursor_pick, Vector2.zero, CursorMode.Auto);
    }



}
