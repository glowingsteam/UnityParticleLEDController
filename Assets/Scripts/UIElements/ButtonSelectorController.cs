using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectorController : MonoBehaviour
{
    [SerializeField] List<SelectableButton> _btns;

    [SerializeField] int _startingIndex = 0;

    public int CurrentIndex { get { return GetIndexForBtn(_selectedButton); } }

    private SelectableButton _selectedButton;

    public delegate void OnIndexChange(int newIndex);
    public OnIndexChange onIndexChanged;

    private void Start()
    {
        if (_btns.Count == 0)
            return;

        int _currentBtn = Mathf.Clamp(_startingIndex, 0, _btns.Count - 1);
        _selectedButton = _btns[_currentBtn];

        foreach (var button in _btns)
        {
            button.onClicked += ClickedButton;
            button.SetStyle(ButtonState.Deselected);
        }

        _selectedButton.SetStyle(ButtonState.Selected);
    }

    private void ClickedButton(SelectableButton btn)
    {
        if (btn == _selectedButton)
            return;

        _selectedButton.SetStyle(ButtonState.Deselected);
        _selectedButton = btn;
        _selectedButton.SetStyle(ButtonState.Selected);

        onIndexChanged?.Invoke(GetIndexForBtn(_selectedButton));
    }

    public void ForceSelectBtn(int index)
    {
        Debug.Log("Forcing to index: " + index);

        int _currentBtn = Mathf.Clamp(index, 0, _btns.Count - 1);
        ClickedButton(_btns[_currentBtn]);
    }

    public int GetIndexForBtn(SelectableButton btn)
    {
        for (int i = 0; i < _btns.Count; i++)
        {
            if (btn == _btns[i])
                return i;
        }

        return -1;
    }
}
