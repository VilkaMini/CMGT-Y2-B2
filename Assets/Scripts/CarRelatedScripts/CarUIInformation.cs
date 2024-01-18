using System.Collections.Generic;
using UnityEngine;

public class CarUIInformation : MonoBehaviour
{
    public bool selected = false;
    public int carId;

    private UserInterfaceController _userInterfaceController;
    
    [SerializeField] private UnityEngine.UI.Image image;
    [SerializeField] private List<Sprite> spriteList;

    private void Start()
    {
        _userInterfaceController = FindObjectOfType<UserInterfaceController>();
    }

    /// <summary>
    /// Method <c>SelectDeselect</c> flips the UI image of button.
    /// </summary>
    public void SelectDeselect()
    {
        selected = !selected;
        image.sprite = spriteList[selected == false ? 0 : 1];
        _userInterfaceController.ChangeSelected(carId);
    }

    /// <summary>
    /// Method <c>Deselect</c> deselects the UI image of button (specific case).
    /// </summary>
    public void Deselect()
    {
        selected = false;
        image.sprite = spriteList[0];
    }
}
