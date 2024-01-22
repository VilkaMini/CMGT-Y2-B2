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
    
    /// <summary>
    /// Method <c>SpawnModel</c> spawns model on the network.
    /// </summary>
    public void SpawnModel()
    {
        carObjectTransforms.Add(Instantiate(carModelsPrefabs[_activeCar]));
        carObjectTransforms[_activeCar].GetComponent<NetworkObject>().SpawnWithObservers = false;
        carObjectTransforms[_activeCar].GetComponent<NetworkObject>().Spawn(true);
    }

    /// <summary>
    /// Method <c>ActOnStateChange</c> changes the networked objects based on state of game on client.
    /// <param name="gameState">ControlState used to proceed with logic.</param>
    /// <param name="carId">int used to indicate car.</param>
    /// </summary>
    public void ActOnStateChange(ControlState gameState, int carId)
    {
        _activeCar = carId;
        if (IsServer)
        {
            if (gameState == ControlState.View3D)
            {
                if (_activeCar > carObjectTransforms.Count-1)
                {
                    SpawnModel();
                }
                else
                {
                    TurnOnCarServerRpc(_activeCar);
                }
                return;
            }

            if (gameState == ControlState.ManagerSetup || gameState == ControlState.MemberSetup)
            {
                TurnOffCarServerRpc(_activeCar);
                return;
            }
        }
        if (IsClient)
        {
            if (gameState == ControlState.View3D)
            {
                TurnOnCarServerRpc(_activeCar);
            }
            if (gameState == ControlState.ManagerSetup || gameState == ControlState.MemberSetup)
            {
                TurnOffCarServerRpc(_activeCar);
                
            }
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
    /// <param name="carId">int used to indicate car.</param>
    /// <param name="serverRpcParams">serverRpcParams used to get client id.</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void TurnOnCarServerRpc(int carId ,ServerRpcParams serverRpcParams = default)
    {
        if (NetworkManager.Singleton.LocalClientId == serverRpcParams.Receive.SenderClientId)
        {
            carObjectTransforms[carId].gameObject.SetActive(true);
            SignVisibilityToggle(true, false, serverRpcParams.Receive.SenderClientId, carId);

        }
        else
        {
            carObjectTransforms[carId].gameObject.GetComponent<NetworkObject>().NetworkShow(serverRpcParams.Receive.SenderClientId);
            SignVisibilityToggle(true, true, serverRpcParams.Receive.SenderClientId, carId);

        }
    }

    /// <summary>
    /// Method <c>TurnOffCarServerRpc</c> executes only on server when client requests car to stop display.
    /// <param name="carId">int used to indicate car.</param>
    /// <param name="serverRpcParams">serverRpcParams used to get client id.</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void TurnOffCarServerRpc(int carId, ServerRpcParams serverRpcParams = default)
    {
        if (NetworkManager.Singleton.LocalClientId == serverRpcParams.Receive.SenderClientId)
        {
            carObjectTransforms[carId].gameObject.SetActive(false);
            SignVisibilityToggle(false, false, serverRpcParams.Receive.SenderClientId, carId);
        }
        else
        {
            carObjectTransforms[carId].gameObject.GetComponent<NetworkObject>().NetworkHide(serverRpcParams.Receive.SenderClientId);
            SignVisibilityToggle(false, true, serverRpcParams.Receive.SenderClientId, carId);
        }
    }

    /// <summary>
    /// Method <c>SignVisibilityToggle</c> executes only on server when client requests to stop display.
    /// <param name="show">bool to indicate to show or not show signs.</param>
    /// <param name="client">bool to indicate if client or host.</param>
    /// <param name="clientID">ulonh client id.</param>
    /// <param name="carId">int used to indicate car.</param>
    /// </summary>
    private void SignVisibilityToggle(bool show, bool client ,ulong clientID, int carId)
    {
        for (int i = 0; i < allSigns.Count; i++)
        {
            if (allSigns[i].GetComponent<SignLogic>().carId == carId)
            {
                if (client)
                {
                    // Hide
                    if (show == false)
                    {
                        allSigns[i].GetComponent<NetworkObject>().NetworkHide(clientID);
                    }
                    // Show
                    else
                    {
                        allSigns[i].GetComponent<NetworkObject>().NetworkShow(clientID);
                    }
                }
                else
                {
                    // Hide
                    if (show == false)
                    {
                        allSigns[i].GetComponent<MeshRenderer>().enabled = false;
                    }
                    // Show
                    else
                    {
                        allSigns[i].GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }
    }
}
