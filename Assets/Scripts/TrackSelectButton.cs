using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackSelectButton : MonoBehaviour
{

    [SerializeField]
    private string trackSceneName;
    [SerializeField]
    private Image trackImage;
    [SerializeField]
    private int raceLaps = 4;

    public void SelectTrack()
    {
        RaceInfoManager.Instance.SetTrackToLoad(trackSceneName);
        RaceInfoManager.Instance.SetNoOfLaps(raceLaps);
        RaceInfoManager.Instance.SetTrackSprite(trackImage.sprite);

        MainMenu.Instance.SetTrackSelectImage(trackImage);

        MainMenu.Instance.CloseTrackSelect();
    }
}
