using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    [SerializeField]
    private float curTime;
    private float prevTime;

    public List<float> roundClearTimeList;
    public float finalTime;

    private Coroutine updateTimeCoroutine;

    // �б� ���� ������Ƽ�� curTime�� ����
    public float CurTime
    {
        get { return curTime; }
        private set { curTime = value; }
    }

    public float PrevTime
    {
        get { return prevTime; }
        set { prevTime = value; }
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
        ResetCurTime();
        if (scene.buildIndex != 3)
        {
            if (updateTimeCoroutine != null)
            {
                StopCoroutine(updateTimeCoroutine);
                updateTimeCoroutine = null;
            }
        }
        else
        {
            if (updateTimeCoroutine != null)
            {
                StopCoroutine(updateTimeCoroutine);
            }
            updateTimeCoroutine = StartCoroutine(UpdateCurTime());
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
