using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static DataTypes;

public class NetworkManagerController : NetworkBehaviour
{
    private int _activeCar = 0;
    [SerializeField] private Transform carModelsPrefabs;
    [SerializeField] private List<Transform> carObjectTransforms;
    [SerializeField] private Transform car3DModelPrefab;
    private Transform spawnedObjectTransform;
    
    /// <summary>
    /// Method <c>SpawnModel</c> spawns model on the network.
    /// </summary>
    public void SpawnModel()
    {
        carObjectTransforms[_activeCar] = Instantiate(car3DModelPrefab);
        carObjectTransforms[_activeCar].GetComponent<NetworkObject>().Spawn(true);
    }

    /// <summary>
    /// Method <c>ActOnStateChange</c> changes the networked objects based on state of game on client.
    /// <param name="gameState">ControlState used to proceed with logic.</param>
    /// </summary>
    public void ActOnStateChange(ControlState gameState, int carId)
    {
        if (gameState == ControlState.ManagerSetup || gameState == ControlState.MemberSetup) return;
        if (gameState == ControlState.ViewCrashSelection) TurnOffCarServerRpc(carId);
        if (gameState == ControlState.View3D && carObjectTransforms[_activeCar]) TurnOnCarServerRpc(carId);
        
        if (carObjectTransforms[_activeCar]) return;
        if (IsServer) SpawnModel();
    }

    /// <summary>
    /// Method <c>TurnOnCarServerRpc</c> executes only on server when client requests car to display.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void TurnOnCarServerRpc(int carId ,ServerRpcParams serverRpcParams = default)
    {
        TurnOnCarClientRpc(carId ,serverRpcParams.Receive.SenderClientId);
    }

    /// <summary>
    /// Method <c>TurnOnCarClientRpc</c> executes only on client with specific client Id to show the car.
    /// </summary>
    [ClientRpc]
    private void TurnOnCarClientRpc(int carId, ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            carObjectTransforms[carId].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Method <c>TurnOffCarServerRpc</c> executes only on server when client requests car to stop display.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void TurnOffCarServerRpc(int carId, ServerRpcParams serverRpcParams = default)
    {
        TurnOffCarClientRpc(carId, serverRpcParams.Receive.SenderClientId);
    }

    /// <summary>
    /// Method <c>TurnOffCarClientRpc</c> executes only on client with specific client Id to hide the car.
    /// </summary>
    [ClientRpc]
    private void TurnOffCarClientRpc(int carId, ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            carObjectTransforms[carId].gameObject.SetActive(false);
        }
    }
}
