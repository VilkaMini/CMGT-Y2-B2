using UnityEngine;

public class SignLogic : MonoBehaviour
{
    private Camera _cam;
    public int carId;

    private void Update()
    {
        if (!_cam) FindCamera();
        transform.rotation = Quaternion.Euler(Quaternion.LookRotation((_cam.transform.position - transform.position).normalized).eulerAngles  - new Vector3(90, 0, 0));
    }

    /// <summary>
    /// Method <c>FindCamera</c> finds a camera in the scene (used for networked objects).
    /// </summary>
    private void FindCamera()
    {
        var tempCam = FindObjectOfType<Camera>();
        if (tempCam)
        {
            _cam = tempCam;
        }
    }
}
