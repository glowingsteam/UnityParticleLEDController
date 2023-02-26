using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonState : byte
{ 
    Deselected = 0,
    Selected = 1
}

public class SelectableButton : MonoBehaviour
{
    public Button _btn;
    public Image _bgImage;

    public delegate void OnClicked(SelectableButton btn);
    public OnClicked onClicked;

    private ButtonState _state = ButtonState.Deselected;

    [SerializeField] Color _deselectedColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    [SerializeField] Color _selectedColor = Color.white;

    public void SetStyle(ButtonState NewState)
    {
        _state = NewState;
        _bgImage.color = _state == ButtonState.Deselected ? _deselectedColor : _selectedColor;
    }

    public void ClickBtn()
    {
        onClicked?.Invoke(this);
    }
}
