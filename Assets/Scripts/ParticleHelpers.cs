using UnityEngine;
using UnityEngine.UI;

public class ParticleHelpers
{ 
    public static string GetVariableStringByEnum(CloudVariableType VarType)
    {
        switch (VarType)
        {
            case CloudVariableType.Channel1:
                return "Channel1";
            case CloudVariableType.Channel2:
                return "Channel2";
            case CloudVariableType.Intensity:
                return "Intensity";
            case CloudVariableType.Speed:
                return "Speed";
            case CloudVariableType.Animation:
                return "Animation";
            case CloudVariableType.Power:
                return "Power";
        }

        return "";
    }

    public static string GetFunctionNameByEnum(CloudVariableType VarType)
    {
        switch (VarType)
        {
            case CloudVariableType.Channel1:
                return "SetChannel1";
            case CloudVariableType.Channel2:
                return "SetChannel2";
            case CloudVariableType.Intensity:
                return "SetIntensity";
            case CloudVariableType.Speed:
                return "SetSpeed";
            case CloudVariableType.Animation:
                return "SetAnimation";
            case CloudVariableType.Power:
                return "Power";
        }

        return "";
    }

    public static void SetSliderObject(object obj, float val)
    {
        SliderBinding slider = (SliderBinding)obj;
        slider._slider.SetValueWithoutNotify(Mathf.Clamp(val, 0.0f, 1.0f));

        int TextVal = 1 + Mathf.Clamp(Mathf.RoundToInt(val * 99.0f), 0, 99);

        slider._text.SetText(TextVal.ToString() + "%");
    }

    public static void SetPowerColor(object obj, bool isOn)
    {
        Image img = obj as Image;
        
        if (img)
            img.color = isOn ? Color.green : Color.red;
    }
    public static void SetChannelColor(CloudVariableType ch, object obj, Color inColor)
    {
        ColorController controller = obj as ColorController;
        
        if (controller)
            controller.UpdateColorForChannel(ch, inColor);
    }

    public static void SetButtonIndex(object obj, int index)
    {
        ButtonSelectorController controller = obj as ButtonSelectorController;
        
        if (controller)
            controller.ForceSelectBtn(index);
    }
}
