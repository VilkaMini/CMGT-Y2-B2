using UnityEngine;

[RequireComponent(typeof(GameStateController))]
public class ControllerBase : MonoBehaviour
{
    public GameStateController stateController;
    void Awake()
    {
        stateController = GetComponent<GameStateController>();
    }
}
