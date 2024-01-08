using Unity.Netcode;
using UnityEngine;
using static DataTypes;

public class NetworkManagerController : NetworkBehaviour
{
    [SerializeField] private Transform car3DModelPrefab;
    private Transform spawnedObjectTransform;
    
    /// <summary>
    /// Method <c>SpawnModel</c> spawns model on the network.
    /// </summary>
    public void SpawnModel()
    {
        spawnedObjectTransform = Instantiate(car3DModelPrefab);
        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
    }

    /// <summary>
    /// Method <c>ActOnStateChange</c> changes the networked objects based on state of game on client.
    /// <param name="gameState">ControlState used to proceed with logic.</param>
    /// </summary>
    public void ActOnStateChange(ControlState gameState)
    {
        if (gameState == ControlState.ViewCrashSelection) TurnOffCarServerRpc();
        if (gameState == ControlState.View3D && spawnedObjectTransform) TurnOnCarServerRpc();
        
        if (spawnedObjectTransform) return;
        if (IsServer) SpawnModel();
    }

    /// <summary>
    /// Method <c>TurnOnCarServerRpc</c> executes only on server when client requests car to display.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void TurnOnCarServerRpc(ServerRpcParams serverRpcParams = default)
    {
        TurnOnCarClientRpc(serverRpcParams.Receive.SenderClientId);
    }

    /// <summary>
    /// Method <c>TurnOnCarClientRpc</c> executes only on client with specific client Id to show the car.
    /// </summary>
    [ClientRpc]
    private void TurnOnCarClientRpc(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            spawnedObjectTransform.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Method <c>TurnOffCarServerRpc</c> executes only on server when client requests car to stop display.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void TurnOffCarServerRpc(ServerRpcParams serverRpcParams = default)
    {
        TurnOffCarClientRpc(serverRpcParams.Receive.SenderClientId);
    }

    /// <summary>
    /// Method <c>TurnOffCarClientRpc</c> executes only on client with specific client Id to hide the car.
    /// </summary>
    [ClientRpc]
    private void TurnOffCarClientRpc(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            spawnedObjectTransform.gameObject.SetActive(false);
        }
    }
}
