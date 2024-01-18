using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarSignManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> signPrefabs;

    /// <summary>
    /// Method <c>PlaceSignAtLocation</c> places a sign prefab defined in a list when called.
    /// <param name="location">Vector3 that indicates where to place sign in world position.</param>
    /// <param name="carId">int that saves id of a car that the sign belongs to.</param>
    /// <param name="signId">int setting the prefab of a sign to use.</param>
    /// </summary>
    public GameObject PlaceSignAtLocation(Vector3 location, int carId, int signId)
    {
        GameObject signObject = Instantiate(signPrefabs[signId], location, Quaternion.identity, transform);
        signObject.GetComponent<SignLogic>().carId = carId;
        signObject.GetComponent<NetworkObject>().Spawn();
        return signObject;
    }
}
