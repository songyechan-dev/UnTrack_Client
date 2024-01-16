using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance()
    {
        return instance;
    }

    public string dataPath;
    private string content;
    private string questType;
    private string progressType;
    private int progressGoal;
    private int reward;

    public int progress = 0;
    public bool isCompleted = false;

    public Text questText;

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

    private void Start()
    {

    }

    // **** UI 확인용 버튼 ****
    public void OnExampleButton()
    {
        InitQuest();
        QuestTextChange(content, progress, progressGoal);
    }

    // 수집형 퀘스트 진행도 업그레이드 → StateManager.cs에서 
    public void UpdateProgress(string _progressType, int _amount)
    {
        if (!isCompleted && progressType.Equals(_progressType))
        {
            progress += _amount;
            QuestTextChange(content, progress, progressGoal);

            if (progress >= progressGoal)
            {
                Debug.Log(":::: Quest Completed ::::");
                isCompleted = true;
            }
        }
    }

    // 시간형 퀘스트 진행도 업그레이드 → StateManager.cs에서 
    public void UpdateProgress(float _amount)
    {
        if (!isCompleted && questType.Equals("Time"))
        {
            progress = (int)_amount;
            Debug.Log(progress);

            if (progress >= progressGoal)
            {
                Debug.Log(":::: Quest Completed ::::");
                isCompleted = true;
            }
        }
    }

    // 퀘스트 성공 시 보상 지급 → 라운드 종료 후 실행 
    public void CheckCompletion()
    {
        if(isCompleted)
        {
            StateManager.Instance().voltNum += reward;
        }
    }

    // 퀘스트 새로 받아오기 및 초기화 → 새 라운드 시작할 때 실행 
    public void InitQuest()
    {
        isCompleted = false;
        progress = 0;

        int randomNum = Random.Range(0, 15);
        QuestJsonLoad(dataPath, randomNum);
    }

    // TODO : 이유정 2024.01.16 QuestManager.cs QuestTextChange(string _content, int _progress, int _progressGaol)
    public void QuestTextChange(string _content, int _progress, int _progressGaol)
    {
        if (questType.Equals("Time"))
        {
            questText.text = _content;
        }
        else
        {
            questText.text = _content + $" ( {_progress} / {_progressGaol} )";
        }
    }

    // TODO : 이유정 2024.01.15 QuestManager.cs QuestJsonLoad(string path)
    void QuestJsonLoad(string _path, int _n)
    {
        TextAsset json = (TextAsset)Resources.Load(_path);
        string jsonStr = json.text;

        var jsonData = JSON.Parse(jsonStr);

        content = jsonData[_n]["CONTENT"];
        questType = jsonData[_n]["QUEST_TYPE"];
        progressType = jsonData[_n]["PROGRESS_TYPE"];
        progressGoal = jsonData[_n]["PROGRESS_GOAL"];
        reward = jsonData[_n]["REWARD_VOLT"];
    }
}
