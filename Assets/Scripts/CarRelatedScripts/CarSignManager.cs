using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarSignManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> signPrefabs;

    public GameObject PlaceSignAtLocation(Vector3 location, int carId, int signId)
    {
        GameObject signObject = Instantiate(signPrefabs[signId], location, Quaternion.identity, transform);
        signObject.GetComponent<SignLogic>().carId = carId;
        signObject.GetComponent<NetworkObject>().Spawn();
        return signObject;
    }
}
