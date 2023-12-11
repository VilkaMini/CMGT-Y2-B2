using static DataTypes;

public class DrawViewController : ControllerBase
{
    void Update()
    {
        if (stateController.GameState == ControlState.View3D)
        {
            print("Using Draw View Script");
        }
    }
}
