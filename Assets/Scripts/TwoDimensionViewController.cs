using UnityEngine;
using static DataTypes;

public class TwoDimensionViewController : ControllerBase
{
    void Update()
    {
        if (stateController.GameState == ControlState.View3D)
        {
            print("Using 2D View Script");
        }
    }
}
