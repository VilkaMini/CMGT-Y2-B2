using static DataTypes;

public class DrawViewController : ControllerBase
{
    void Update()
    {
        if (stateController.GameState == ControlState.ViewDraw)
        {
            print("Using Draw View Script");
        }
    }
}
