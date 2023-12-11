using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool IsTouchApplied()
    {
        return Input.touchCount == 1;
    }

    public bool IsDoubleTouchApplied()
    {
        return Input.touchCount == 2;
    }

    public Vector2 GetTouch()
    {
        return Input.GetTouch(0).position;
    }

    public Vector2[] GetDoubleTouch()
    {
        return new Vector2[]
        {
            Input.GetTouch(0).position, 
            Input.GetTouch(1).position
        };
    }
}
