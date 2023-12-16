using UnityEngine;

[RequireComponent(typeof(GameStateController))]
[RequireComponent(typeof(InputManager))]
public class ControllerBase : MonoBehaviour
{
    protected GameStateController stateController;
    protected InputManager inputManager;
    void Awake()
    {
        stateController = GetComponent<GameStateController>();
        inputManager = GetComponent<InputManager>();
    }
}
