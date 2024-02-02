using UnityEditor;
using UnityEngine;
using TMPro;

public class ApplyTTFToTMPFonts : EditorWindow
{
    private Font targetTTF;

    [MenuItem("Tools/Apply TTF to TMP Fonts")]
    private static void OpenWindow()
    {
        ApplyTTFToTMPFonts window = GetWindow<ApplyTTFToTMPFonts>();
        window.titleContent = new GUIContent("Apply TTF to TMP Fonts");
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
                ApplyTTFToAllTMPFonts(targetTTF);
            }
            else
            {
                Debug.LogWarning("Please select a target TTF font.");
            }
        }
    }

    private void ApplyTTFToAllTMPFonts(Font ttfFont)
    {
        TextMeshProUGUI[] textMeshPros = GameObject.FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI textMeshPro in textMeshPros)
        {
            Undo.RecordObject(textMeshPro, "Change TMP Font");
            textMeshPro.font = ttfFont;
            EditorUtility.SetDirty(textMeshPro);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("TTF font applied to all TMP Fonts successfully.");
    }
}
