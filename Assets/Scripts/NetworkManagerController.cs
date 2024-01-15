using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static DataTypes;

public class NetworkManagerController : NetworkBehaviour
{
    private int _activeCar = 0;
    [SerializeField] private List<Transform> carModelsPrefabs;
    [SerializeField] private List<Transform> carObjectTransforms = new List<Transform>(){}; 
    [SerializeField] public List<GameObject> allSigns = new List<GameObject>(){};
    private Transform spawnedObjectTransform;

    [SerializeField] private UserInterfaceController _userInterfaceController;
    [SerializeField] private GameStateController _gameStateController;
    
    /// <summary>
    /// Method <c>SpawnModel</c> spawns model on the network.
    /// </summary>
    public void SpawnModel()
    {
        print("Spawning the car");
        carObjectTransforms.Add(Instantiate(carModelsPrefabs[_activeCar]));
        carObjectTransforms[_activeCar].GetComponent<NetworkObject>().Spawn(true);
    }

    /// <summary>
    /// Method <c>ActOnStateChange</c> changes the networked objects based on state of game on client.
    /// <param name="gameState">ControlState used to proceed with logic.</param>
    /// </summary>
    public void ActOnStateChange(ControlState gameState, int carId)
    {
        _activeCar = carId;
        if (gameState == ControlState.ManagerSetup || gameState == ControlState.MemberSetup) TurnOffCarServerRpc(carId);
        if (gameState == ControlState.ViewCrashSelection) TurnOffCarServerRpc(carId);
        if (IsServer && carObjectTransforms.Count == carId)
        {
            SpawnModel();
        }

        if (IsClient)
        {
            if (gameState == ControlState.View3D) TurnOnCarServerRpc(carId);
        }
    }

    [ClientRpc]
    public void SpawnCarUIClientRpc()
    {
        if (IsHost) return;
        _userInterfaceController.AddMemberCar();
    }

    /// <summary>
    /// Method <c>TurnOnCarServerRpc</c> executes only on server when client requests car to display.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void TurnOnCarServerRpc(int carId ,ServerRpcParams serverRpcParams = default)
    {
        if (IsServer)
        {
            print("Turn on car host for id:" + serverRpcParams.Receive.SenderClientId);
            carObjectTransforms[carId].gameObject.SetActive(true);
        }
        else if (IsClient)
        {
            print("Turn on car client for id:" + serverRpcParams.Receive.SenderClientId);
            carObjectTransforms[carId].gameObject.GetComponent<NetworkObject>().NetworkShow(serverRpcParams.Receive.SenderClientId);
        }
    }

    /// <summary>
    /// Method <c>TurnOnCarClientRpc</c> executes only on client with specific client Id to show the car.
    /// </summary>
    private void TurnOnCar(int carId, ulong clientID)
    {
        //carObjectTransforms[carId].gameObject.GetComponent<NetworkObject>().NetworkHide(serverRpcParams.Receive.SenderClientId);
        //if (clientID == NetworkManager.Singleton.LocalClientId)
        //{
            carObjectTransforms[carId].gameObject.GetComponent<NetworkObject>().NetworkShow(clientID);
            //carObjectTransforms[carId].gameObject.SetActive(true);
            // for (int i = 0; i < allSigns.Count; i++)
            // {
            //     if (allSigns[i].GetComponent<SignLogic>().carId == _gameStateController.ActiveCarId)
            //     {
            //         allSigns[i].SetActive(true);
            //     }
            // }
        //}
    }

    /// <summary>
    /// Method <c>TurnOffCarServerRpc</c> executes only on server when client requests car to stop display.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void TurnOffCarServerRpc(int carId, ServerRpcParams serverRpcParams = default)
    {
        if (IsServer)
        {
            print("Turn off car host for id:" + serverRpcParams.Receive.SenderClientId);
            carObjectTransforms[carId].gameObject.SetActive(false);
        }
        else if (IsClient)
        {
            print("Turn off car client for id:" + serverRpcParams.Receive.SenderClientId);
            carObjectTransforms[carId].gameObject.GetComponent<NetworkObject>().NetworkHide(serverRpcParams.Receive.SenderClientId);
        }

        
        //if (serverRpcParams.Receive.SenderClientId == NetworkManager.Singleton.LocalClientId)
        //{
        
        
            //carObjectTransforms[carId].gameObject.SetActive(false);
            //for (int i = 0; i < allSigns.Count; i++)
            //{
            //     if (allSigns[i].GetComponent<SignLogic>().carId == _gameStateController.ActiveCarId)
            //     {
            //         allSigns[i].SetActive(false);
            //     }
            // }
        //}
        //TurnOffCarClientRpc(carId, serverRpcParams.Receive.SenderClientId);
    }

    /// <summary>
    /// Method <c>TurnOffCarClientRpc</c> executes only on client with specific client Id to hide the car.
    /// </summary>
    private void TurnOffCar(int carId, ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            carObjectTransforms[carId].gameObject.GetComponent<NetworkObject>().NetworkHide(clientID);
            //carObjectTransforms[carId].gameObject.SetActive(false);
            //for (int i = 0; i < allSigns.Count; i++)
            //{
            //     if (allSigns[i].GetComponent<SignLogic>().carId == _gameStateController.ActiveCarId)
            //     {
            //         allSigns[i].SetActive(false);
            //     }
            // }
        }
    }
}
