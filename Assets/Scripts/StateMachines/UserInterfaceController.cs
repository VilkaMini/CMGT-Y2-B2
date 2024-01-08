using TMPro;
using static DataTypes;
using UnityEngine;

public class UserInterfaceController : MonoBehaviour
{
    // Initialize image display
    [SerializeField] private GameObject listOfSchematicsObject;
    [SerializeField] private UnityEngine.UI.Image[] listOfImages;
    [SerializeField] private int activeSchematic = 0;

    [SerializeField] private GameObject schematics2DGroup;
    [SerializeField] private GameObject schematics3DGroup;
    [SerializeField] private GameObject crashSelectScreen;
    [SerializeField] private GameObject drawViewGroup;
    [SerializeField] private GameObject setupGroup;
    [SerializeField] private GameObject setupNumberGroup;
    [SerializeField] private TMP_InputField numberPlateInputField;
    [SerializeField] private GameObject setupSeatsGroup;

    [SerializeField] private Camera viewCam;

    private bool _setupComplete = false;
    
    void Start()
    {
        // Form array of Images and set all to invisible apart from first one
        listOfImages = listOfSchematicsObject.GetComponentsInChildren<UnityEngine.UI.Image>();
        for (int i = 0; i < listOfImages.Length; i++)
        {
            if (i == activeSchematic)
            {
                listOfImages[i].gameObject.SetActive(true);
            }
            else
            {
                listOfImages[i].gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Method <c>DisplaySchematic</c> iterates through list of <c>GameObjects</c> and activates the object of schematicIndex.
    /// <param name="schematicIndex">Index of which GameObject should be activated.</param>
    /// </summary>
    private void DisplaySchematic(int schematicIndex)
    {
        for (int i = 0; i < listOfImages.Length; i++)
        {
            if (i == schematicIndex)
            {
                listOfImages[i].gameObject.SetActive(true);
            }
            else
            {
                listOfImages[i].gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Method <c>PressSchematicControl</c> is used by Buttons on the Canvas.
    /// <param name="previousOrNext">boolean that indicates if button <c>Next</c> or <c>Previous</c> was clicked.</param>
    /// </summary>
    public void PressSchematicControl(bool previousOrNext)
    {
        // If 1st schematic is displayed and previous clicked -> jump to last
        if (activeSchematic == 0 && !previousOrNext)
        {
            activeSchematic = listOfImages.Length-1;
        }
        // If last schematic is displayed and next clicked -> jump to first
        else if (activeSchematic == listOfImages.Length-1 && previousOrNext)
        {
            activeSchematic = 0;
        }
        // If next clicked
        else if (previousOrNext)
        {
            activeSchematic++;
        }
        // If previous clicked
        else
        {
            activeSchematic--;
        }
        DisplaySchematic(activeSchematic);
    }
    
    /// <summary>
    /// Method <c>ChangeUI</c> is used by Buttons on the Canvas to react to state change.
    /// <param name="gameState">ControlState that indicates which state the game is in.</param>
    /// </summary>
    public void ChangeUI(ControlState gameState)
    {
        viewCam.orthographic = false;
        schematics2DGroup.SetActive(false);
        schematics3DGroup.SetActive(false);
        crashSelectScreen.SetActive(false);
        drawViewGroup.SetActive(false);
        setupGroup.SetActive(false);
        switch (gameState)
        {
            case ControlState.View3D:
                schematics3DGroup.SetActive(true);
                break;
            case ControlState.View2D:
                schematics2DGroup.SetActive(true);
                break;
            case ControlState.ViewCrashSelection:
                crashSelectScreen.SetActive(true);
                break;
            case ControlState.ViewDraw:
                viewCam.orthographic = true;
                drawViewGroup.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Method <c>SetupControl</c> is used to complete the initial setup for a car.
    /// <param name="step">integer that indicates which step of the setup is done.</param>
    /// </summary>
    public void SetupControl(int step)
    {
        if (_setupComplete)
        {
            GetComponent<GameStateController>().ChangeGameState(0);
            return;
        }

        setupGroup.SetActive(true);
        if (step == 0)
        {
            if (numberPlateInputField.text.Length == 6)
            {
                setupNumberGroup.SetActive(false);
                setupSeatsGroup.SetActive(true);
                _setupComplete = true;
            }
        }
    }
}
