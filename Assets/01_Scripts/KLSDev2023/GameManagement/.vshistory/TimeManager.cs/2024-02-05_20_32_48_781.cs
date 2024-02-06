using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    private float curTime;

    // �б� ���� ������Ƽ�� curTime�� ����
    public float CurTime
    {
        get { return curTime; }
        private set { curTime = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        // Scene�� �ε�� ������ �̺�Ʈ�� �Լ� �߰�
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �ڷ�ƾ ����
        StartCoroutine(UpdateCurTime());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetCurTime();
        if (scene.buildIndex != 3)
        {
            StopCoroutine(UpdateCurTime());
        }
        else
        {
            StartCoroutine(UpdateCurTime());
        }
    }

    public float GetCurTime()
    {
        return CurTime; // ������Ƽ�� ����Ͽ� ���� ��ȯ
    }

    private void ResetCurTime()
    {
        CurTime = 0;

    }

    private IEnumerator UpdateCurTime()
    {
        while (true)
        {
            // �ڷ�ƾ���� �ð� ������Ʈ
            CurTime = Time.time + Time.deltaTime;

            // 1������ ���
            yield return null;
        }
    }

    private void OnDestroy()
    {
        // �ش� ��ü�� �ı��� �� �̺�Ʈ���� �Լ� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
