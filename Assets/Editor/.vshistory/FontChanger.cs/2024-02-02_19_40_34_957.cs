using UnityEditor;
using UnityEngine;
using TMPro;

public class FontChanger : EditorWindow
{
    private TMP_FontAsset targetFont;

    [MenuItem("Window/TMP Font Changer")]
    private static void OpenWindow()
    {
        FontChanger window = GetWindow<FontChanger>();
        window.titleContent = new GUIContent("TMP Font Changer");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select the Target Font", EditorStyles.boldLabel);

        targetFont = EditorGUILayout.ObjectField("Target Font", targetFont, typeof(TMP_FontAsset), false) as TMP_FontAsset;

        if (GUILayout.Button("Change Font"))
        {
            if (targetFont != null)
            {
                ChangeCanvasFonts(targetFont);
            }
            else
            {
                Debug.LogWarning("Please select a target font.");
            }
        }
    }

    private void ChangeCanvasFonts(TMP_FontAsset newFont)
    {
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in canvases)
        {
            TextMeshProUGUI[] textMeshPros = canvas.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (TextMeshProUGUI textMeshPro in textMeshPros)
            {
                Undo.RecordObject(textMeshPro, "Change TMP Font");
                textMeshPro.font = newFont;
                EditorUtility.SetDirty(textMeshPro);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Fonts changed successfully.");
    }
}
