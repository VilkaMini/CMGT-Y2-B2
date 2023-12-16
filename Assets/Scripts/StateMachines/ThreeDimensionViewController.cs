using UnityEngine;
using static DataTypes;

public class ThreeDimensionViewController : ControllerBase
{
    private Vector2 _touchBeginPosition;
    private Vector2 _touchMovedPosition;
    
    [SerializeField] private float cameraSpeedMultiplier = 0.01f;
    [SerializeField] private GameObject cameraPivot;
    private float _pivotRotationX;
    private float _pivotRotationY;
    
    void Update()
    {
        if (stateController.GameState == ControlState.View3D)
        {
            DetectInput();
            RotateCamera();
        }
    }
    
    /// <summary>
    /// Method <c>DetectInput</c> registers if only one finger is touching the screen and records the begin, stationary and moved positions.
    /// </summary>
    private void DetectInput()
    {
        if (inputManager.IsTouchApplied())
        {
            Touch currentTouch = inputManager.GetTouch();
            TouchPhase currentTouchPhase = currentTouch.phase;
            
            if (currentTouchPhase == TouchPhase.Began || currentTouchPhase == TouchPhase.Stationary) {
                _touchBeginPosition = currentTouch.position;
                _touchMovedPosition = currentTouch.position;
            }
            if (currentTouchPhase == TouchPhase.Moved) {
                _touchMovedPosition = currentTouch.position;
            }
            if (currentTouchPhase == TouchPhase.Ended)
            {
                _touchBeginPosition = Vector2.zero;
                _touchMovedPosition = Vector2.zero;
            }
        }
    }
    
    /// <summary>
    /// Method <c>RotateCamera</c> rotates camera pivot point around the target. Deadzones are implemented to safeguard from excessive rotation.
    /// </summary>
    private void RotateCamera()
    {
        Vector2 touchMoveDir = _touchBeginPosition - _touchMovedPosition;
        print(touchMoveDir);
        
        var rotCheckVal = _pivotRotationY + touchMoveDir.y * cameraSpeedMultiplier;
        if (rotCheckVal is >= -20 and <= 20)
        {
            _pivotRotationY += touchMoveDir.y * cameraSpeedMultiplier;
        }

        if (touchMoveDir.x is <= -1 or >= 1)
        {
            _pivotRotationX += touchMoveDir.x * cameraSpeedMultiplier;
        }

        cameraPivot.transform.rotation = Quaternion.Euler(_pivotRotationY, _pivotRotationX, 0);
    }
}
