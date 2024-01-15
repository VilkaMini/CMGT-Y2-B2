using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CarSignManager : MonoBehaviour
{
    [SerializeField] private GameObject signPrefab;
    [SerializeField] private List<Texture> signTextures;
    [SerializeField] private Material baseMat;

    public GameObject PlaceSignAtLocation(Vector3 location, int carId)
    {
        GameObject signObject = Instantiate(signPrefab, location, Quaternion.Euler(-90, 0, 0), transform);
        signObject.GetComponent<SignLogic>().carId = carId;
        signObject.GetComponent<MeshRenderer>().material = CraftMaterial(0);
        signObject.GetComponent<NetworkObject>().Spawn();
        return signObject;
    }

    private Material CraftMaterial(int matId)
    {
        Material signMat = baseMat;
        signMat.mainTexture = signTextures[matId];
        return signMat;
    }
}
