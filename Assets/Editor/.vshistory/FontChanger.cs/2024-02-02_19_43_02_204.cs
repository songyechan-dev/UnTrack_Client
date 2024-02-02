using UnityEditor;
using UnityEngine;
using TMPro;

public class FontChanger : EditorWindow
{
    private Font targetTTF;

    [MenuItem("Window/TTF to TMP Font")]
    private static void OpenWindow()
    {
        FontChanger window = GetWindow<FontChanger>();
        window.titleContent = new GUIContent("TTF to TMP Font");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select the TTF Font", EditorStyles.boldLabel);

        targetTTF = EditorGUILayout.ObjectField("Target TTF Font", targetTTF, typeof(Font), false) as Font;

        if (GUILayout.Button("Apply to TMP Fonts"))
        {
            if (targetTTF != null)
            {
                ApplyTTFToTMPFonts(targetTTF);
            }
            else
            {
                Debug.LogWarning("Please select a target TTF font.");
            }
        }
    }

    private void ApplyTTFToTMPFonts(Font ttfFont)
    {
        string ttfPath = AssetDatabase.GetAssetPath(ttfFont);
        TMP_FontAsset tmpFont = TMP_FontAsset.GetFontAsset(ttfPath);

        if (tmpFont == null)
        {
            Debug.LogWarning("Selected TTF font is not a valid TMP Font.");
            return;
        }

        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();

        foreach (Canvas canvas in canvases)
        {
            TextMeshProUGUI[] textMeshPros = canvas.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (TextMeshProUGUI textMeshPro in textMeshPros)
            {
                Undo.RecordObject(textMeshPro, "Change TMP Font");
                textMeshPro.font = tmpFont;
                EditorUtility.SetDirty(textMeshPro);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("TTF font applied to TMP Fonts successfully.");
    }
}
