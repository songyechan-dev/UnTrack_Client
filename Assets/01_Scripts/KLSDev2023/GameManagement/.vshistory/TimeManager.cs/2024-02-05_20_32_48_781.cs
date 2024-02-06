using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    private float curTime;

    // 읽기 전용 프로퍼티로 curTime을 정의
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

        // Scene이 로드될 때마다 이벤트에 함수 추가
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 코루틴 시작
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
        return CurTime; // 프로퍼티를 사용하여 값을 반환
    }

    private void ResetCurTime()
    {
        CurTime = 0;

    }

    private IEnumerator UpdateCurTime()
    {
        while (true)
        {
            // 코루틴에서 시간 업데이트
            CurTime = Time.time + Time.deltaTime;

            // 1프레임 대기
            yield return null;
        }
    }

    private void OnDestroy()
    {
        // 해당 객체가 파괴될 때 이벤트에서 함수 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
