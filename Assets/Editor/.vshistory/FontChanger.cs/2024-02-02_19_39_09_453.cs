using UnityEditor;
using UnityEngine;
using TMPro;

public class TMPFontChanger : EditorWindow
{
    private TMP_FontAsset targetFont;

    [MenuItem("Tools /Font Changer")]
    private static void OpenWindow()
    {
        TMPFontChanger window = GetWindow<TMPFontChanger>();
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
                ChangeSelectedFonts(targetFont);
            }
            else
            {
                Debug.Log("폰트를 선택해주세요.");
            }
        }
    }

    private void ChangeSelectedFonts(TMP_FontAsset newFont)
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject selectedObject in selectedObjects)
        {
            TextMeshProUGUI textMeshPro = selectedObject.GetComponent<TextMeshProUGUI>();

            if (textMeshPro != null)
            {
                Undo.RecordObject(textMeshPro, "Change TMP Font");
                textMeshPro.font = newFont;
                EditorUtility.SetDirty(textMeshPro);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("폰트 변경됨.");
    }
}
