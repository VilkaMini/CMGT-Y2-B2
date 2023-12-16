using System;
using UnityEngine;
using static DataTypes;

public class GameStateController : MonoBehaviour
{
    private ControlState _gameState;
    private UserInterfaceController _userInterface;

    public ControlState GameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    private void Start()
    {
        _gameState = ControlState.View3D;
    }

    private void Update()
    {
        switch (_gameState)
        {
            case ControlState.View3D:
                break;
            case ControlState.View2D:
                break;
            case ControlState.ViewDraw:
                break;
        }
    }

    public void ChangeGameState(int state)
    {
        GameState = (ControlState)state;
        _userInterface.ChangeUI(GameState);
    }
}
