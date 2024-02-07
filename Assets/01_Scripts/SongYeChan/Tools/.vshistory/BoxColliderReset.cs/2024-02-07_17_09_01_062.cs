using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxColliderReset : EditorWindow
{
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
                // 박스 콜라이더 제거
                DestroyImmediate(button.GetComponent<BoxCollider>());

                // 새로운 박스 콜라이더 추가
                button.gameObject.AddComponent<BoxCollider>();
            }
        }

        Debug.Log("Box Colliders have been reset.");
    }
}
