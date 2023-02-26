using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeViewController : MonoBehaviour
{
    [SerializeField] Image _powerImage;
    [SerializeField] ButtonSelectorController _animController;
    [SerializeField] SliderBinding _IntensitySlider;
    [SerializeField] SliderBinding _SpeedSlider;
    [SerializeField] ColorController _ColorController;

    string test = "";

    private void Start()
    {
        _animController.onIndexChanged += AnimIndexChanged;

        ClickRefresh();
    }

    public void ClickOn()
    {
        bool newOn = _powerImage.color == Color.red;

        HttpController.Instance.HandlePost(CloudVariableType.Power, _powerImage, newOn ? "1" : "0");

        _powerImage.color = newOn ? Color.green : Color.red;
    }

    public void ClickRefresh()
    {
        HttpController.Instance.HandleUpdate(CloudVariableType.Power, _powerImage);
        HttpController.Instance.HandleUpdate(CloudVariableType.Channel1, _ColorController);
        HttpController.Instance.HandleUpdate(CloudVariableType.Channel2, _ColorController);
        HttpController.Instance.HandleUpdate(CloudVariableType.Animation, _animController);
        HttpController.Instance.HandleUpdate(CloudVariableType.Intensity, _IntensitySlider);
        HttpController.Instance.HandleUpdate(CloudVariableType.Speed, _SpeedSlider);
    }

    private void AnimIndexChanged(int newIndex)
    {
        HttpController.Instance.HandlePost(CloudVariableType.Animation, _animController, newIndex.ToString());
    }

    private int GetSliderInt(float val)
    {
        return Mathf.Clamp(Mathf.RoundToInt(val), 1, 100);
    }

    public void FinishedDraggingIntensity()
    {
        HttpController.Instance.HandlePost(CloudVariableType.Intensity, _IntensitySlider, _IntensitySlider._slider.value.ToString());
    }

    public void FinishedDraggingSpeed()
    {
        HttpController.Instance.HandlePost(CloudVariableType.Speed, _SpeedSlider, _SpeedSlider._slider.value.ToString());
    }

    public void SendNewColor()
    {
        HttpController.Instance.HandlePost(_ColorController.SelectedChannel, _ColorController, _ColorController.PickedColor);
    }
}
