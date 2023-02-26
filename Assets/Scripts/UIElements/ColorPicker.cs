using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public UnityEvent<Color> ColorPickerEvent;
    
    [SerializeField] RawImage _img;
    [SerializeField] RectTransform cursor;
    [SerializeField] Image cursorColor;

    Texture2D colorChart;
    RectTransform rt;
    

    private void Start()
    {
        colorChart = _img.texture as Texture2D;
        rt = _img.gameObject.transform as RectTransform;
        
    }

    public void PickColor(BaseEventData data)
    {
        PointerEventData pointer = data as PointerEventData;

        Vector3[] v = new Vector3[4];
        rt.GetWorldCorners(v);

        float xPercent = Mathf.Clamp((pointer.position.x - v[1].x) / (v[2].x - v[1].x), 0.0f, 1.0f);
        float yPercent = Mathf.Clamp((pointer.position.y - v[0].y) / (v[1].y - v[0].y), 0.0f, 1.0f);

        Color pickedColor = colorChart.GetPixel((int)(xPercent * colorChart.width), (int)(yPercent * colorChart.height));
        
        //cursorColor.color = pickedColor;
        
        ColorPickerEvent?.Invoke(pickedColor);
    }
}
