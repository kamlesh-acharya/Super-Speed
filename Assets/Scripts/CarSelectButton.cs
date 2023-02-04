using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectButton : MonoBehaviour
{
    [SerializeField]
    private CarController carToSet;
    [SerializeField]
    private Image carImage;
    
    public void SelectCar()
    {
        RaceInfoManager.Instance.SetCarToUse(carToSet);
        RaceInfoManager.Instance.SetCarSprite(carImage.sprite);
        MainMenu.Instance.SetCarSelectImage(carImage);
        MainMenu.Instance.CloseCarSelect();
    }
}
