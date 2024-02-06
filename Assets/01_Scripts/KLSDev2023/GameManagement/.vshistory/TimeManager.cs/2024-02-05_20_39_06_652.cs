using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    [SerializeField]
    private float curTime;

    // 읽기 전용 프로퍼티로 curTime을 정의
    public float CurTime
    {
        get { return curTime; }
        private set { curTime = value; }
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

        // Scene이 로드될 때마다 이벤트에 함수 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
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
