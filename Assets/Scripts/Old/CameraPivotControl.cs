using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class CameraPivotControl : MonoBehaviour
{
    private GameObject camera;
    private Vector2 _touchBeginPosition;
    private Vector2 _touchMovedPosition;

    private Vector2 _originBeginPosition;
    private Vector2 _originMovedPosition;

    [SerializeField] private float cameraSpeedMultiplier;
    private float _pivotRotationX;
    private float _pivotRotationY;

    // Update is called once per frame
    void Update()
    {
        DetectInput();
        DetectMoveInput();
        if (_touchMovedPosition != Vector2.zero)
        {
            RotateCamera();
        }
    }

    /// <summary>
    /// Method <c>DetectInput</c> registers if only one finger is touching the screen and records the begin, stationary and moved positions.
    /// </summary>
    private void DetectInput()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                _touchBeginPosition = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                _touchMovedPosition = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                _touchBeginPosition = Vector2.zero;
                _touchMovedPosition = Vector2.zero;
            }
        }
    }
    
    /// <summary>
    /// Method <c>DetectMoveInput</c> registers if 2 fingers are touching the screen and calculates a position between them of their begin, stationary and moed states.
    /// </summary>
    private void DetectMoveInput()
    {
        if (Input.touchCount == 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                _originBeginPosition = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                _originMovedPosition = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                _originBeginPosition = Vector2.zero;
                _originMovedPosition = Vector2.zero;
            }
        }
    }

    /// <summary>
    /// Method <c>RotateCamera</c> rotates camera pivot point around the target. Deadzones are implemented to safeguard from excessive rotation.
    /// </summary>
    private void RotateCamera()
    {
        Vector2 touchMoveDir = _touchBeginPosition - _touchMovedPosition;
        
        var rotCheckVal = _pivotRotationY + touchMoveDir.y * cameraSpeedMultiplier;
        if (rotCheckVal is >= -20 and <= 20)
        {
            _pivotRotationY += touchMoveDir.y * cameraSpeedMultiplier;
        }

        if (touchMoveDir.x is <= -1 or >= 1)
        {
            _pivotRotationX += touchMoveDir.x * cameraSpeedMultiplier;
        }

        transform.rotation = Quaternion.Euler(_pivotRotationY, _pivotRotationX, 0);
    }
}
