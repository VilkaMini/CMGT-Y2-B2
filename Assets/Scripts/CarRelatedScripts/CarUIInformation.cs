using System;
using System.Collections.Generic;
using UnityEngine;

public class CarUIInformation : MonoBehaviour
{
    public string carName;
    public string carNumber;
    public bool selected = false;
    public int carId;

    private UserInterfaceController _userInterfaceController;
    
    [SerializeField] private UnityEngine.UI.Image image;
    [SerializeField] private List<Sprite> spriteList;

    private void Start()
    {
        _userInterfaceController = FindObjectOfType<UserInterfaceController>();
    }

    public void SelectDeselect()
    {
        selected = !selected;
        image.sprite = spriteList[selected == false ? 0 : 1];
        _userInterfaceController.ChangeSelected(carId);
    }

    public void Deselect()
    {
        selected = false;
        image.sprite = spriteList[0];
    }
}
