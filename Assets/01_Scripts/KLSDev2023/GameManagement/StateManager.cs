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
    public int accumulateWood;    // 누적 Wood 개수 
    public int accumulateSteel;   // 누적 Steel 개수
    public int accumulateRail;    // 누적 Rail 사용 개수 
    public int accumulateElapsedTime;   // 누적 경과 시간

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

    // State가 변경될 시 Quest Progress 상태 확인 함수 호출하기

}
