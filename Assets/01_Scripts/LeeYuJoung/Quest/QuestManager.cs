using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public string dataPath;
    private string content;
    private string questType;
    private string progressType;
    private int progressGoal;
    private int reward;

    public int progress = 0;
    public bool isCompleted = false;

    public Text questText;

    private void Start()
    {
        ResetQuest();
        QuestTextChabge(content, progress, progressGoal);
    }

    // 퀘스트 진행도 업그레이드 → 
    public void UpdateProgress(int amount)
    {
        if(!isCompleted)
        {
            progress += amount;

            if(progress >= progressGoal)
            {
                isCompleted = true;
            }
        }
    }

    // 퀘스트 성공 시 보상 지급
    public void CheckCompletion()
    {

    }

    // 퀘스트 새로 받아오기 및 초기화
    public void ResetQuest()
    {
        isCompleted = false;
        progress = 0;

        int randomNum = Random.Range(0, 15);
        QuestJsonLoad(dataPath, randomNum);
    }

    // UIManager로 변경할 부분!!!!!
    public void QuestTextChabge(string _content, int _progress, int _progressGaol)
    {
        questText.text = _content + $"( {_progress} / {_progressGaol} )";
    }

    //TODO : 이유정 2024.01.15 QuestManager.cs QuestJsonLoad(string path)
    void QuestJsonLoad(string path, int n)
    {
        TextAsset json = (TextAsset)Resources.Load(path);
        string jsonStr = json.text;

        var jsonData = JSON.Parse(jsonStr);

        content = jsonData[n]["CONTENT"];
        questType = jsonData[n]["QUEST_TYPE"];
        progressType = jsonData[n]["PROGRESS_TYPE"];
        progressGoal = jsonData[n]["PROGRESS_GOAL"];
        reward = jsonData[n]["REWARD_VOLT"];
    }
}
