using System;
using UnityEngine;
using static DataTypes;

[RequireComponent(typeof(UserInterfaceController))]
public class GameStateController : MonoBehaviour
{
    private ControlState _gameState;
    private UserInterfaceController _userInterface;
    [SerializeField] private NetworkManagerController _networkManager;

    /// <summary>
    /// Enum <c>ControlState</c> is used to indicate the state the game is in.
    /// </summary>
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
    
    /// <summary>
    /// Method <c>ChangeGameState</c> is used by to change the state of which the game is in.
    /// <param name="state">integer that tells which state the game should switch to based on ControlState</param>
    /// </summary>
    public void ChangeGameState(int state)
    {
        GameState = (ControlState)state;
        _userInterface.ChangeUI(GameState);
        _networkManager.ActOnStateChange(GameState);
    }
}
