using System.Collections.Generic;
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
    
    [SerializeField] private GameObject managerScreen;
    [SerializeField] private GameObject memberScreen;

    [SerializeField] private GameObject view2DGroup;

    [SerializeField] private Camera viewCam;

    [SerializeField] private GameObject newCarPrefab;
    private List<CarUIInformation> carUIInforList = new List<CarUIInformation>(){};

    private GameStateController _gameStateController;
    
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

    public void AddCar()
    {
        var newCarUI = Instantiate(newCarPrefab, managerScreen.transform);
        newCarUI.transform.position = new Vector3(newCarUI.transform.position.x, newCarUI.transform.position.y - carUIInforList.Count * 200, newCarUI.transform.position.z);
        var script = newCarUI.GetComponent<CarUIInformation>();
        script.carId = carUIInforList.Count;
        carUIInforList.Add(script);
    }

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
}
