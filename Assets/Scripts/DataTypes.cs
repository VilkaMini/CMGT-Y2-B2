using System;

public static class DataTypes
{
    [Serializable]
    public enum ControlState
    {
        View3D,
        View2D,
        ViewDraw,
        ViewCrashSelection
    }
    
    [Serializable]
    public enum RotationSide
    {
        Front,
        Right,
        Left,
        Back
    }
}
