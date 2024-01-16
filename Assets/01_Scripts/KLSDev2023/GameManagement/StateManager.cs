using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private static StateManager instance;
    public static StateManager Instance()
    {
        return instance;
    }

    public int EngineMaxVolume = 5;      // 엔진 최대 수용 가능한 기계 개수
    public int EngineCurrentVolume = 3;  // 현재 엔진이 수용한 기계 개수

    public int voltNum = 0;       // 현재 보유 볼트(재화) 개수
    public int accumulateWood = 0;    // 누적 Wood 개수 
    public int accumulateSteel = 0;   // 누적 Steel 개수
    public int accumulateTrack = 0;    // 누적 Rail 사용 개수 
    public float accumulateElapsedTime = 0;  // 누적 경과 시간

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
    }

    private void Update()
    {      
        // 라운드 시작 하면 시간 누적 시작 하기 & 라운드 끝나면 누적 시간 초기화
        //accumulateElapsedTime += Time.deltaTime;
        //CheckAccumulateTime();
    }

    // 플레이어가 현재 들고 있는 오브젝트 정보 매개변수로 전달 받기(해당 오브젝트의 이름)
    // → 재료는 저장소에 내려 놓기 직전에 해당 함수 실행하기
    // → 트랙은 트랙 옆에 내려 놓기 직전에 해당 함수 실행하기
    public void CheckAccumulateCount(string _name, int _amount)
    {
        int currentNum = (int)GetType().GetField($"accumulate{_name}").GetValue(instance);
        GetType().GetField($"accumulate{_name}").SetValue(instance, currentNum + _amount);

        // Quest 진행도 업그레이드
        QuestManager.Instance().UpdateProgress(_name, _amount);
    }

    // 현재 라운드의 누적 시간
    public void CheckAccumulateTime()
    {
        // Quest 진행도 업그레이드
        QuestManager.Instance().UpdateProgress(accumulateElapsedTime);
    }
}