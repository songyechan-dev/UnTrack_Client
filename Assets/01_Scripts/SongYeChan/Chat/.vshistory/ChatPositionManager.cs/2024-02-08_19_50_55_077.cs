using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public Sprite imageSprite; 

    void Start()
    {
        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();

        GameObject imageObject = new GameObject("Image");
        imageObject.transform.SetParent(transform, false);
        Image image = imageObject.AddComponent<Image>();
        image.sprite = imageSprite;

        RectTransform imageRectTransform = imageObject.GetComponent<RectTransform>();
        RectTransform textRectTransform = textMeshPro.GetComponent<RectTransform>();
        imageRectTransform.sizeDelta = textRectTransform.sizeDelta;

        imageRectTransform.position = textRectTransform.position;
    }
}
