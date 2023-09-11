using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIObjectView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image bgImage;

    public void InitializeUIObject(UIObject uiObject)
    {
        nameText.text = uiObject.name;
        bgImage.color = uiObject.color;
        RectTransform myRect = GetComponent<RectTransform>();
        myRect.localPosition = uiObject.position;
        myRect.localRotation = Quaternion.Euler(uiObject.rotation);
    }
}
