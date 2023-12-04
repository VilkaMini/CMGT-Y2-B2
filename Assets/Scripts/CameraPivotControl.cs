using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class CameraPivotControl : MonoBehaviour
{
    private GameObject camera;
    private Vector2 touchBeginPosition;
    private Vector2 touchMovedPosition;

    [SerializeField] private float cameraSpeedMultiplier;
    private float pivotRotationX;
    private float pivotRotationY;

    // Update is called once per frame
    void Update()
    {
        DetectInput();
        if (touchMovedPosition != Vector2.zero)
        {
            MoveCamera();
        }
    }

    private void DetectInput()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                touchBeginPosition = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touchMovedPosition = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchBeginPosition = Vector2.zero;
                touchMovedPosition = Vector2.zero;
            }
        }
    }

    private void MoveCamera()
    {
        Vector2 touchMoveDir = touchBeginPosition - touchMovedPosition;
        
        var rotCheckVal = pivotRotationY + touchMoveDir.y * cameraSpeedMultiplier;
        if (-20 <= rotCheckVal && rotCheckVal <= 20)
        {
            pivotRotationY += touchMoveDir.y * cameraSpeedMultiplier;
        }

        if (-1 >= touchMoveDir.x || touchMoveDir.x <= 1){}
        {
            pivotRotationX += touchMoveDir.x * cameraSpeedMultiplier;
        }

        transform.rotation = Quaternion.Euler(pivotRotationY, pivotRotationX, 0);
    }
}
