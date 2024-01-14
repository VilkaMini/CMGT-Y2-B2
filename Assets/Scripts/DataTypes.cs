using System;

public static class DataTypes
{
    [Serializable]
    public enum ControlState
    {
        View3D,
        View2D,
        ViewDraw,
        ViewCrashSelection,
        ManagerSetup,
        MemberSetup
    }
    
    [Serializable]
    public enum RotationSide
    {
        Front,
        Right,
        Left,
        Back
    }

    [Serializable]
    public enum UserType
    {
        Member,
        Manager
    }
}
