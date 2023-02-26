using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{
    [SerializeField] ButtonSelectorController _btnController;

    public Image Channel1Image;
    public Image Channel2Image;

    [SerializeField] TMP_InputField _text;
    [SerializeField] Image _previewColor;

    [SerializeField] ColorPicker _colorPicker;

    public string Color { get { return ColorUtility.ToHtmlStringRGB(CurrentImage.color); } }

    public string PickedColor { get { return ColorUtility.ToHtmlStringRGB(_previewColor.color); } }

    public CloudVariableType SelectedChannel { 
        get 
        { 
            return _btnController.CurrentIndex == 0 ? 
                CloudVariableType.Channel1 : 
                CloudVariableType.Channel2; 
        } 
    }

    public Image CurrentImage
    {
        get
        {
            return _btnController.CurrentIndex == 0 ?
                Channel1Image :
                Channel2Image;
        }
    }

    private string lastColor = "";

    private void Awake()
    {
        _btnController.onIndexChanged += IndexChanged;

        _colorPicker.ColorPickerEvent.AddListener(ColorSelected);

        lastColor = Color;
    }

    private void OnDestroy()
    {
        _btnController.onIndexChanged -= IndexChanged;
        _colorPicker.ColorPickerEvent.RemoveListener(ColorSelected);
    }

    public void UpdateColorForCurrentChannel(Color newColor)
    {
        UpdateColorForChannel(SelectedChannel, newColor);
    }

    public void UpdateColorForChannel(CloudVariableType ch, Color newColor)
    {
        Image ImageToUpdate = ch == CloudVariableType.Channel1 ? Channel1Image : Channel2Image;
        ImageToUpdate.color = newColor;

        if (ch == SelectedChannel)
            UpdatePreview(newColor);
    }

    private void IndexChanged(int newIndex)
    {
        UpdatePreview(CurrentImage.color);
    }

    private void UpdatePreview(Color newColor)
    {
        _previewColor.color = newColor;
        _text.SetTextWithoutNotify(ColorUtility.ToHtmlStringRGB(newColor));

        lastColor = Color;
    }

    private void ColorSelected(Color newColor)
    {
        UpdatePreview(newColor);
    }

    public void FinishedEditText(string newColor)
    {
        Color c;

        Debug.Log(newColor);

        if (ColorUtility.TryParseHtmlString("#" + newColor, out c))
            ColorSelected(c);
        else if (ColorUtility.TryParseHtmlString("#" + lastColor, out c))
            ColorSelected(c);
    }
}
