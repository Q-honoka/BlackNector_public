using System;
using System.Diagnostics;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public static InputControl Instance;

    public enum PlayerActions
    {
        NONE_PRESS_KEY = -1,
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,
        ACTION_PECK,
        ACTION_CALL,
        MAX
    };


    private void Awake()
    {
        //シングルトン化
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //playerの操作キーの状態を確認
        CheckPlayerMoveKey();
    }

    public int CheckPlayerMoveKey()
    {
        if (Input.GetKey(KeyCode.W)) { return (int)PlayerActions.MOVE_UP; }
        else if (Input.GetKey(KeyCode.A)) { return (int)PlayerActions.MOVE_LEFT; }
        else if (Input.GetKey(KeyCode.S)) { return (int)PlayerActions.MOVE_DOWN; }
        else if (Input.GetKey(KeyCode.D)) { return (int)PlayerActions.MOVE_RIGHT; }
        else if (Input.GetKey(KeyCode.E)) { return (int)PlayerActions.ACTION_PECK; }
        else if (Input.GetKey(KeyCode.C)) { return (int)PlayerActions.ACTION_CALL; }
        return (int)PlayerActions.NONE_PRESS_KEY;
    }
}

