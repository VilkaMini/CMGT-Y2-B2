using System.Collections.Generic;
using static DataTypes;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField] private GameObject schematics3DGroup;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject drawViewGroup;
    [SerializeField] private GameObject drawToggle;
    [SerializeField] private GameObject infoManualGroup;
    [SerializeField] private GameObject overviewGroup;
    
    [SerializeField] private GameObject managerScreen;
    [SerializeField] private GameObject memberScreen;

    [SerializeField] private GameObject view2DGroup;

    [SerializeField] private Camera viewCam;
    [SerializeField] private Image shaderControl;
    [SerializeField] private List<Sprite> shaderControlSprites;

    [SerializeField] private GameObject newCarPrefab;
    private List<CarUIInformation> carUIInforList = new List<CarUIInformation>(){};

    private GameStateController _gameStateController;
    [SerializeField] private NetworkManagerController _networkManagerController;

    private int currentWireframeState = 1;
    
    void Start()
    {
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
        managerScreen.SetActive(false);
        memberScreen.SetActive(false);
        view2DGroup.SetActive(false);
        switch (gameState)
        {
            case ControlState.View3D:
                schematics3DGroup.SetActive(true);
                break;
            case ControlState.View2D:
                viewCam.orthographic = true;
                view2DGroup.SetActive(true);
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
    /// Method <c>ChangeNonOperationalUIInfo</c> is used by Buttons to open info manual group.
    /// <param name="open">ControlState that indicates which state the ui should be activated.</param>
    /// </summary>
    public void ChangeNonOperationalUIInfo(bool open)
    {
        infoManualGroup.SetActive(open);
    }
    
    /// <summary>
    /// Method <c>ChangeNonOperationalUIInfo</c> is used by Buttons to open info manual group.
    /// <param name="open">ControlState that indicates which state the ui should be activated.</param>
    /// </summary>
    public void ChangeOverviewUIInfo(bool open)
    {
        overviewGroup.SetActive(open);
    }
    
    /// <summary>
    /// Method <c>GoBackToSetupFirstPage</c> is used by Buttons to open start screen group.
    /// <param name="open">ControlState that indicates which state the ui should be activated.</param>
    /// </summary>
    public void GoBackToSetupFirstPage(bool open)
    {
        startScreen.SetActive(open);
    }

    /// <summary>
    /// Method <c>StartControl</c> is used to start the server or get back to the 3D view.
    /// <param name="message">string contains either Host or Client keywords.</param>
    /// </summary>
    public void StartControl(string message)
    {
        if (message == "Host")
        {
            if (!_networkManagerController.IsHost)
            {
                NetworkManager.Singleton.StartHost();
            }
            _gameStateController.UserType = UserType.Manager;
            ChangeUI(ControlState.ManagerSetup);
        }
        else if (message == "Client")
        {
            if (!_networkManagerController.IsClient)
            {
                NetworkManager.Singleton.StartClient();
            }
            _gameStateController.UserType = UserType.Member;
            drawToggle.SetActive(false);
            ChangeUI(ControlState.MemberSetup);
        }
    }

    /// <summary>
    /// Method <c>AddCar</c> is used to add a car UI to the host and client screens.
    /// </summary>
    public void AddCar()
    {
        var newCarUI = Instantiate(newCarPrefab, managerScreen.transform);
        newCarUI.transform.position = new Vector3(newCarUI.transform.position.x, newCarUI.transform.position.y - carUIInforList.Count * 200, newCarUI.transform.position.z);
        var script = newCarUI.GetComponent<CarUIInformation>();
        script.carId = carUIInforList.Count;
        carUIInforList.Add(script);
        _networkManagerController.SpawnCarUIClientRpc();
    }

    /// <summary>
    /// Method <c>AddMemberCar</c> is used to add a car UI to the client side.
    /// </summary>
    public void AddMemberCar()
    {
        var newCarUI = Instantiate(newCarPrefab, memberScreen.transform);
        newCarUI.transform.position = new Vector3(newCarUI.transform.position.x, newCarUI.transform.position.y - carUIInforList.Count * 200, newCarUI.transform.position.z);
        var script = newCarUI.GetComponent<CarUIInformation>();
        script.carId = carUIInforList.Count;
        carUIInforList.Add(script);
    }

    /// <summary>
    /// Method <c>ChangeSelected</c> is used to change a selected car id.
    /// <param name="carId">int indicates selected car id.</param>
    /// </summary>
    public void ChangeSelected(int carId)
    {
        _gameStateController.ActiveCarId = carId;
        for (int i = 0; i < carUIInforList.Count; i++)
        {
            if (i != carId)
            {   
                carUIInforList[i].Deselect();
            }
        }
    }

    public void ChangeCarShaderVisibility()
    {
        if (currentWireframeState == 1) { currentWireframeState = 0; }
        else { currentWireframeState = 1; }
        shaderControl.sprite = shaderControlSprites[currentWireframeState];
        CarSignManager car = FindObjectOfType<CarSignManager>();
        car.ChangeShaderMaterial(currentWireframeState);
    }
}
