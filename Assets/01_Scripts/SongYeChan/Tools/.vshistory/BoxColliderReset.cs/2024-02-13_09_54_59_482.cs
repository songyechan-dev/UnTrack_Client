using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxColliderReset : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Tools/Reset Box Colliders")]
    private static void ResetBoxColliders()
    {
        // 모든 씬의 오브젝트를 가져오기
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] rootGameObjects = scene.GetRootGameObjects();

        foreach (GameObject rootGameObject in rootGameObjects)
        {
            PlayableButtonInfo[] buttons = rootGameObject.GetComponentsInChildren<PlayableButtonInfo>(true);
            foreach (PlayableButtonInfo button in buttons)
            {
                DestroyImmediate(button.GetComponent<BoxCollider>());
                button.gameObject.AddComponent<BoxCollider>();
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Box Colliders have been reset.");
    }
#endif
}
