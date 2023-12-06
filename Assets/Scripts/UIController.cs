using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UIController : MonoBehaviour
{
    // Initialize image display
    [SerializeField] private GameObject listOfSchematicsObject;
    [SerializeField] private Image[] listOfImages;
    [SerializeField] private int activeSchematic = 0;

    [SerializeField] private GameObject schematics2DGroup;
    [SerializeField] private GameObject schematics3DGroup;
    
    void Start()
    {
        // Form array of Images and set all to invisible apart from first one
        listOfImages = listOfSchematicsObject.GetComponentsInChildren<Image>();
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

    public void SwitchSchematicMode()
    {
        schematics2DGroup.SetActive(!schematics2DGroup.activeSelf);
        schematics3DGroup.SetActive(!schematics3DGroup.activeSelf);
    }
}
