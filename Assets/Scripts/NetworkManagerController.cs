using Unity.Netcode;
using UnityEngine;
using static DataTypes;

public class NetworkManagerController : NetworkBehaviour
{
    [SerializeField] private Transform car3DModelPrefab;
    private Transform spawnedObjectTransform;
    
    public void SpawnModel()
    {
        spawnedObjectTransform = Instantiate(car3DModelPrefab);
        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
    }
    
    public void DespawnModel()
    {
        spawnedObjectTransform.GetComponent<NetworkObject>().Despawn();
    }

    public void ActOnStateChange(ControlState gameState)
    {
        if (gameState == ControlState.ViewCrashSelection) TurnOffCarServerRpc();
        if (spawnedObjectTransform) return;

        if (IsServer)
        {
            SpawnModel();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TurnOffCarServerRpc(ServerRpcParams serverRpcParams = default)
    {
        TurnOffCarClientRpc(serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TurnOffCarClientRpc(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            spawnedObjectTransform.gameObject.SetActive(false);
        }
    }
}
