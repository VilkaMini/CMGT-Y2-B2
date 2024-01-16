using UnityEngine;

public class SignLogic : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _floatSpeed;

    private Camera _cam;

    public int carId;

    private Vector3 _initialPosition;
    private Vector3 _initialRotation = new Vector3(-90, 0, 0);

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (!_cam)
        {
            FindCamera();
        }
        transform.rotation = Quaternion.Euler(Quaternion.LookRotation((_cam.transform.position - transform.position).normalized).eulerAngles  - new Vector3(90, 0, 0));
    }

    private void FindCamera()
    {
        var tempCam = FindObjectOfType<Camera>();
        if (tempCam)
        {
            _cam = tempCam;
        }
    }

    // /// <summary>
    // /// Method <c>RotateAround</c> animates the rotation of the sign around itself.
    // /// </summary>
    // private void RotateAround()
    // {
    //     transform.rotation = Quaternion.Euler(_initialRotation.x ,transform.rotation.eulerAngles.y + 1.0f * _rotationSpeed, _initialRotation.z);
    // }
    //
    // /// <summary>
    // /// Method <c>FloatAnimation</c> animates sign to achieve up and down smooth motion.
    // /// </summary>
    // private void FloatAnimation()
    // {
    //     transform.position = new Vector3(_initialPosition.x, _initialPosition.y + Mathf.Sin(Time.time) * _floatSpeed, _initialPosition.z);
    // }
}
