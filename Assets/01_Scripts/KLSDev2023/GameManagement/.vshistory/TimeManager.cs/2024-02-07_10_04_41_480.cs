using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    [SerializeField]
    private float curTime;
    private float prevTime;

    // �б� ���� ������Ƽ�� curTime�� ����
    public float CurTime
    {
        get { return curTime; }
        private set { curTime = value; }
    }

    public float PrevTime
    {
        get { return prevTime; }
    }

    public static TimeManager Instance()
    {
        return instance;
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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        prevTime = curTime;
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
        return CurTime;
    }

    private void ResetCurTime()
    {
        CurTime = 0;
    }

    private IEnumerator UpdateCurTime()
    {
        while (true)
        {
            CurTime +=Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
