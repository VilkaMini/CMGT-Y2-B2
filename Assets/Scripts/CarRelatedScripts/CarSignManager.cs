using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSignManager : MonoBehaviour
{
    [SerializeField] private GameObject signPrefab;
    [SerializeField] private List<Texture> signTextures;
    [SerializeField] private Material baseMat;

    public void PlaceSignAtLocation(Vector3 location)
    {
        GameObject signObject = Instantiate(signPrefab, location, Quaternion.Euler(-90, 0, 0), transform);
        signObject.GetComponent<MeshRenderer>().material = CraftMaterial(0);
    }

    private Material CraftMaterial(int matId)
    {
        Material signMat = baseMat;
        signMat.mainTexture = signTextures[matId];
        return signMat;
    }
}
