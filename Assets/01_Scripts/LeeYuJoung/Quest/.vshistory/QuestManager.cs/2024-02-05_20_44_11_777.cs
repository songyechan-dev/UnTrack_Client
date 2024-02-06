using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using LeeYuJoung;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class QuestManager : MonoBehaviourPun
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
        DontDestroyOnLoad(gameObject);
        dataPath = "QuestData";
    }

    public void Init()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InitQuest();
            QuestTextChange(content, progress, progressGoal);
        }
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
        //if (PhotonNetwork.IsMasterClient)
        //{
            if (!isCompleted && progressType.Equals(_progressType))
            {
                progress += _amount;
                QuestTextChange(content, progress, progressGoal);

                if (progress >= progressGoal)
                {
                    Debug.Log(":::: Quest Completed ::::");
                    isCompleted = true;
                }
                object[] data = new object[] { _amount };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.QUEST_PROGRESS, data, raiseEventOptions, SendOptions.SendReliable);

            }
        //}
        
    }

    // 시간형 퀘스트 진행도 업그레이드 → StateManager.cs에서 
    public void UpdateProgress()
    {
        if (!isCompleted && questType.Equals("Time"))
        {
            progress = (int)TimeManager.Instance().GetCurTime();
            Debug.Log(progress);

            if (progress <= progressGoal)
            {
                Debug.Log(":::: Quest Completed ::::");
                isCompleted = true;
            }
        }
    }

    // 퀘스트 성공 시 보상 지급 → 라운드 종료 후 실행 
    public void CheckCompletion()
    {
        if(isCompleted && PhotonNetwork.IsMasterClient)
        {
            StateManager.Instance().SetVolt(true, reward);
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
        UIManager.Instance().volt03.text = StateManager.Instance().voltNum.ToString();
        if (questType.Equals("Time"))
        {
            UIManager.Instance().questIndex03.transform.Find("Quest").GetComponent<Text>().text = _content;
        }
        else
        {
            UIManager.Instance().questIndex03.transform.Find("Quest").GetComponent<Text>().text = _content + $" ( {_progress} / {_progressGaol} )";
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

        object[] data = new object[] { content,questType,progressType,progressGoal,reward };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.QUEST_JSON_INFO, data, raiseEventOptions, SendOptions.SendReliable);
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.QUEST_JSON_INFO)
        {
            // 다른 플레이어들이 호출한 RPC로 미터 값을 받음
            isCompleted = false;
            progress = 0;
            object[] receivedData = photonEvent.CustomData as object[];
            Debug.Log(receivedData.Length);
            content = receivedData[0] as string;
            questType = receivedData[1] as string;
            progressType = receivedData[2] as string;
            progressGoal = (int)receivedData[3];
            reward = (int)receivedData[4];
            QuestTextChange(content, progress, progressGoal);
        }
        else if (photonEvent.Code == (int)SendDataInfo.Info.QUEST_PROGRESS)
        {
            object[] receivedData = photonEvent.CustomData as object[];
            int _amount = (int)receivedData[0];
            progress += _amount;
            QuestTextChange(content, progress, progressGoal);

            if (progress >= progressGoal)
            {
                Debug.Log(":::: Quest Completed ::::");
                isCompleted = true;
            }
        }
    }


    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

}
