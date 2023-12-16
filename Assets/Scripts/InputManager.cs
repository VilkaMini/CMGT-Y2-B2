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

    public Touch GetTouch()
    {
        return Input.GetTouch(0);
    }

    public Touch[] GetDoubleTouch()
    {
        return new Touch[]
        {
            Input.GetTouch(0), 
            Input.GetTouch(1)
        };
    }
}
