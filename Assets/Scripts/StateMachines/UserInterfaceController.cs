using TMPro;
using static DataTypes;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField] private GameObject schematics3DGroup;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject drawViewGroup;
    [SerializeField] private GameObject setupGroup;
    
    [SerializeField] private GameObject managerScreen;
    [SerializeField] private GameObject memberScreen;
    
    [SerializeField] private GameObject setupNumberGroup;
    [SerializeField] private TMP_InputField numberPlateInputField;

    [SerializeField] private Camera viewCam;

    private GameStateController _gameStateController;
    private bool _setupComplete = false;
    
    void Start()
    {
        // Form array of Images and set all to invisible apart from first one
        _gameStateController = GetComponent<GameStateController>();
    }
    
    /// <summary>
    /// Method <c>ChangeUI</c> is used by Buttons on the Canvas to react to state change.
    /// <param name="gameState">ControlState that indicates which state the game is in.</param>
    /// </summary>
    public void ChangeUI(ControlState gameState)
    {
        viewCam.orthographic = false;
        schematics3DGroup.SetActive(false);
        startScreen.SetActive(false);
        drawViewGroup.SetActive(false);
        setupGroup.SetActive(false);
        switch (gameState)
        {
            case ControlState.View3D:
                schematics3DGroup.SetActive(true);
                break;
            case ControlState.View2D:
                viewCam.orthographic = true;
                break;
            case ControlState.ViewCrashSelection:
                startScreen.SetActive(true);
                break;
            case ControlState.ViewDraw:
                viewCam.orthographic = true;
                drawViewGroup.SetActive(true);
                break;
            case ControlState.ManagerSetup:
                managerScreen.SetActive(true);
                break;
            case ControlState.MemberSetup:
                memberScreen.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Method <c>SetupControl</c> is used to complete the initial setup for a car.
    /// <param name="step">integer that indicates which step of the setup is done.</param>
    /// </summary>
    public void SetupControl(int step)
    {
        if (_setupComplete)
        {
            _gameStateController.ChangeGameState(0);
            return;
        }

        setupGroup.SetActive(true);
        if (step == 0)
        {
            if (numberPlateInputField.text.Length == 6)
            {
                setupNumberGroup.SetActive(false);
                _setupComplete = true;
            }
        }
    }

    public void StartControl(string message)
    {
        if (message == "Host")
        {
            NetworkManager.Singleton.StartHost();
            _gameStateController.UserType = UserType.Manager;
            ChangeUI(ControlState.ManagerSetup);
        }
        else if (message == "Client")
        {
            NetworkManager.Singleton.StartClient();
            _gameStateController.UserType = UserType.Member;
            ChangeUI(ControlState.MemberSetup);
        }
    }
}
