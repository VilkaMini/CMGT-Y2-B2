using UnityEngine;

public class SignLogic : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _floatSpeed;

    public int carId;

    private Vector3 _initialPosition;
    private Vector3 _initialRotation = new Vector3(-90, 0, 0);

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        RotateAround();
        FloatAnimation();
    }

    /// <summary>
    /// Method <c>RotateAround</c> animates the rotation of the sign around itself.
    /// </summary>
    private void RotateAround()
    {
        transform.rotation = Quaternion.Euler(_initialRotation.x ,transform.rotation.eulerAngles.y + 1.0f * _rotationSpeed, _initialRotation.z);
    }

    /// <summary>
    /// Method <c>FloatAnimation</c> animates sign to achieve up and down smooth motion.
    /// </summary>
    private void FloatAnimation()
    {
        transform.position = new Vector3(_initialPosition.x, _initialPosition.y + Mathf.Sin(Time.time) * _floatSpeed, _initialPosition.z);
    }
}
