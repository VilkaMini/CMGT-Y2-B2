using System;
using UnityEngine;
using static DataTypes;

public class GameStateController : MonoBehaviour
{
    private InputManager _inputManager;
    private ControlState _gameState;

    public ControlState GameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    private void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _gameState = ControlState.View3D;
    }

    private void Update()
    {
        switch (_gameState)
        {
            case ControlState.View3D:
                print("3DView");
                break;
            case ControlState.View2D:
                print("2DView");
                break;
            case ControlState.ViewDraw:
                print("DrawView");
                break;
        }
    }

    public void ChangeGameState(int state)
    {
        GameState = (ControlState)state;
    }
}
