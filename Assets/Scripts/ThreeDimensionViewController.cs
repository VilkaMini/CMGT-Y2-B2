using static DataTypes;

public class ThreeDimensionViewController : ControllerBase
{
    void Update()
    {
        if (stateController.GameState == ControlState.View3D)
        {
            print("Using 3D View Script");
        }
    }
}
