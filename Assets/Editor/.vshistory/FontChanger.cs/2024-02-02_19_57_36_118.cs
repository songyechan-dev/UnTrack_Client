using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ApplyTTFToTexts : EditorWindow
{
    private Font targetTTF;

    [MenuItem("Tools/Font Changer")]
    private static void OpenWindow()
    {
        ApplyTTFToTexts window = GetWindow<ApplyTTFToTexts>();
        window.titleContent = new GUIContent("FontChagner");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select the TTF Font", EditorStyles.boldLabel);

        targetTTF = EditorGUILayout.ObjectField("Target TTF Font", targetTTF, typeof(Font), false) as Font;

        if (GUILayout.Button("Apply to Texts"))
        {
            if (targetTTF != null)
            {
                ApplyTTFToAllTexts(targetTTF);
            }
            else
            {
                Debug.LogWarning("Please select a target TTF font.");
            }
        }
    }

    private void ApplyTTFToAllTexts(Font ttfFont)
    {
        Text[] texts = GameObject.FindObjectsOfType<Text>();

        foreach (Text textComponent in texts)
        {
            Undo.RecordObject(textComponent, "Change Text Font");
            textComponent.font = ttfFont;
            EditorUtility.SetDirty(textComponent);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("TTF font applied to all Texts successfully.");
    }
}
