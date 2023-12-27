using System;
using UnityEngine;
using static DataTypes;

[RequireComponent(typeof(UserInterfaceController))]
public class GameStateController : MonoBehaviour
{
    private ControlState _gameState;
    private UserInterfaceController _userInterface;
    [SerializeField] private NetworkManagerController _networkManager;

    public ControlState GameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    private void Start()
    {
        _gameState = ControlState.ViewCrashSelection;
        _userInterface = GetComponent<UserInterfaceController>();
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
            case ControlState.ViewCrashSelection:
                break;
        }
    }

    public void ChangeGameState(int state)
    {
        GameState = (ControlState)state;
        _userInterface.ChangeUI(GameState);
        _networkManager.ActOnStateChange(GameState);
    }
}
