using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public Sprite imageSprite;

    void Start()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        GameObject imageObject = new GameObject("Image");
        imageObject.transform.SetParent(transform, false);
        Image image = imageObject.AddComponent<Image>();
        image.sprite = imageSprite;

        imageObject.transform.position = transform.position;
    }
}
