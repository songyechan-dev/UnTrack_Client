using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackManager : MonoBehaviourPun
{
    public string planeTagName = "Plane";
    public string trackTagName = "Track";
    public string droppedTrackTagName = "DroppedTrack";
    public GameObject trackPrefab;
    public Material droppedTrackPrefabMaterial;
    public Material trackPrefabMaterial;
    public float maxDistance = 1f;
    public bool isChangedDistance = false;
    public GameObject finalTrack;
    public bool trackConnectFailed = false;
    public GameObject droppedSlotPrefab;
    [Header("Prev Direction Info")]
    public TrackInfo.MyDirection leftTrackPrevDirection;
    public Vector3 leftTrackPrevAngle;

    public TrackInfo.MyDirection rightTrackPrevDirection;
    public Vector3 rightTrackPrevAngle;

    public TrackInfo.MyDirection forwardTrackPrevDirection;
    public Vector3 forwardTrackPrevAngle;

    public TrackInfo.MyDirection backTrackPrevDirection;
    public Vector3 backTrackPrevAngle;

    [SerializeField]
    private List<Transform> electricityFlowingList = new List<Transform>();
    public GameObject factoriesObjectPrefab;

    [SerializeField]
    private Mesh trackLeftMesh;
    [SerializeField]
    private Mesh trackRightMesh;
    void AddComponent(int viewID)
    {
        GameObject go = PhotonView.Find(viewID).gameObject;
        go.AddComponent<TrackInfo>();
    }


    public void TrackCreate(Ray _ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(_ray, out hit))
        {
            Debug.Log($"_ray.origin :: {_ray.origin}");
            Debug.Log($"hit.transform :: {hit.transform.position}");
            GameObject track = PhotonNetwork.Instantiate("Track", new Vector3(Mathf.Round(_ray.origin.x), hit.transform.localPosition.y + (0.05f), Mathf.Round(_ray.origin.z)), trackPrefab.transform.rotation);
            track.tag = trackTagName;
            track.AddComponent<TrackInfo>();
            object[] data = new object[] { track.GetComponent<PhotonView>().ViewID};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_CRAETED_INFO, data, raiseEventOptions, SendOptions.SendReliable);

            TrackInfo _trackInfo = track.GetComponent<TrackInfo>();
            RaycastHit leftHit, rightHit, forwardHit, backHit;

            if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out leftHit, maxDistance))
            {
                Debug.Log(leftHit.transform.name);
                if (leftHit.transform.CompareTag(trackTagName))
                {
                    TrackInfo leftTrackInfo = leftHit.transform.GetComponent<TrackInfo>();

                    Debug.Log("이즈마인아님");
                    leftTrackInfo.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    leftTrackInfo.prevMyMesh = leftTrackInfo.GetComponent<MeshFilter>().mesh;
                    Debug.Log("레프트의 각도 :::::" + leftTrackInfo.transform.localEulerAngles.y);
                    if (Mathf.Abs(leftTrackInfo.transform.localEulerAngles.y) == 180f)
                    {
                        leftTrackInfo.GetComponent<MeshFilter>().mesh = trackLeftMesh;
                        Destroy(leftTrackInfo.transform.GetComponent<BoxCollider>());
                        leftTrackInfo.AddComponent<BoxCollider>();
                    }
                    else if (Mathf.Abs(leftTrackInfo.transform.localEulerAngles.y) == 0)
                    {
                        leftTrackInfo.GetComponent<MeshFilter>().mesh = trackRightMesh;
                        Destroy(leftTrackInfo.transform.GetComponent<BoxCollider>());
                        leftTrackInfo.AddComponent<BoxCollider>();
                    }
                    leftTrackInfo.prevAngle = leftTrackInfo.transform.eulerAngles;
                    leftTrackInfo.SetMyDirection(TrackInfo.MyDirection.RIGHT, new Vector3(MapInfo.trackDefaultXRotation, 90f, MapInfo.trackDefaultZRotation));
                    
                    
                    _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(MapInfo.trackDefaultXRotation, 90f, MapInfo.trackDefaultZRotation));
                    _trackInfo.isElectricityFlowing = true;
                    if (leftTrackInfo.isElectricityFlowing)
                    {
                        electricityFlowingList.Add(leftHit.transform);
                    }

                    object[] trackData = new object[] { _trackInfo.GetComponent<PhotonView>().ViewID,leftTrackInfo.GetComponent<PhotonView>().ViewID, (int)TrackInfo.MyDirection.RIGHT, (int)TrackInfo.MyDirection.FORWARD, new Vector3(MapInfo.trackDefaultXRotation, 90f, MapInfo.trackDefaultZRotation), new Vector3(MapInfo.trackDefaultXRotation, 90f, MapInfo.trackDefaultZRotation) };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.AROUND_TRACK_INFO, trackData, raiseEventOptions, SendOptions.SendReliable);

                }
                else
                {
                    leftHit = new RaycastHit();
                }
            }
   
            if (Physics.Raycast(new Ray(track.transform.position, Vector3.right), out rightHit, maxDistance))
            {
                Debug.Log(rightHit.transform.name);
                if (rightHit.transform.CompareTag(trackTagName))
                {
                    TrackInfo rightTrackInfo = rightHit.transform.GetComponent<TrackInfo>();
   
                    Debug.Log("이즈마인아님");
                    rightTrackInfo.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    rightTrackInfo.prevMyMesh = rightTrackInfo.GetComponent<MeshFilter>().mesh;
                    if (Mathf.Abs(rightTrackInfo.transform.localEulerAngles.y) == 0)
                    {
                        rightTrackInfo.GetComponent<MeshFilter>().mesh = trackLeftMesh;
                        Destroy(rightTrackInfo.transform.GetComponent<BoxCollider>());
                        rightTrackInfo.AddComponent<BoxCollider>();
                    }
                    else if (Mathf.Abs(rightTrackInfo.transform.localEulerAngles.y) == 180)
                    {
                        rightTrackInfo.GetComponent<MeshFilter>().mesh = trackRightMesh;
                        Destroy(rightTrackInfo.transform.GetComponent<BoxCollider>());
                        rightTrackInfo.AddComponent<BoxCollider>();
                    }
                    rightTrackInfo.prevAngle = rightTrackInfo.transform.eulerAngles;
                    rightTrackInfo.SetMyDirection(TrackInfo.MyDirection.LEFT, new Vector3(MapInfo.trackDefaultXRotation, -90f, MapInfo.trackDefaultZRotation));
                    //rightTrackInfo.GetComponent<MeshFilter>().mesh = trackRightMesh;
                    _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(MapInfo.trackDefaultXRotation, -90f, MapInfo.trackDefaultZRotation));
                    _trackInfo.isElectricityFlowing = true;

                    if (rightTrackInfo.isElectricityFlowing)
                    {
                        electricityFlowingList.Add(rightHit.transform);
                    }

                    object[] trackData = new object[] { _trackInfo.GetComponent<PhotonView>().ViewID, rightTrackInfo.GetComponent<PhotonView>().ViewID, (int)TrackInfo.MyDirection.LEFT, (int)TrackInfo.MyDirection.FORWARD, new Vector3(0, -90f, 0), new Vector3(0, -90f, 0) };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.AROUND_TRACK_INFO, trackData, raiseEventOptions, SendOptions.SendReliable);
                }
                else
                {
                    rightHit = new RaycastHit();
                }
            }

            if (Physics.Raycast(new Ray(track.transform.position, Vector3.forward), out forwardHit, maxDistance))
            {
                Debug.Log(forwardHit.transform.name);
                if (forwardHit.transform.CompareTag(trackTagName))
                {
                    TrackInfo forwardTrackInfo = forwardHit.transform.GetComponent<TrackInfo>();
    
                    Debug.Log("이즈마인아님");
                    Debug.Log("각도 :::: " + forwardTrackInfo.transform.localEulerAngles.y);

                    forwardTrackInfo.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    forwardTrackInfo.GetComponent<TrackInfo>().prevMyMesh = forwardTrackInfo.GetComponent<MeshFilter>().mesh;
                    if (forwardTrackInfo.transform.forward == Vector3.right)
                    {
                        Debug.Log("여기 호출");
                        forwardTrackInfo.GetComponent<MeshFilter>().mesh = trackRightMesh;

                        Destroy(forwardTrackInfo.transform.GetComponent<BoxCollider>());
                        forwardTrackInfo.AddComponent<BoxCollider>();
                    }
                    else if (forwardTrackInfo.transform.forward == Vector3.left)
                    {
                        Debug.Log("여기 호출");
                        forwardTrackInfo.GetComponent<MeshFilter>().mesh = trackLeftMesh;

                        Destroy(forwardTrackInfo.transform.GetComponent<BoxCollider>());
                        forwardTrackInfo.AddComponent<BoxCollider>();
                    }
                    forwardTrackInfo.prevAngle = forwardTrackInfo.transform.eulerAngles;
                    forwardTrackInfo.SetMyDirection(TrackInfo.MyDirection.BACK, new Vector3(MapInfo.trackDefaultXRotation, 180f, MapInfo.trackDefaultZRotation));
                    
                    _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(MapInfo.trackDefaultXRotation, 180f, MapInfo.trackDefaultZRotation));
                    _trackInfo.isElectricityFlowing = true;

                    if (forwardTrackInfo.isElectricityFlowing)
                    {
                        electricityFlowingList.Add(forwardHit.transform);
                    }
                    object[] trackData = new object[] { _trackInfo.GetComponent<PhotonView>().ViewID, forwardTrackInfo.GetComponent<PhotonView>().ViewID, (int)TrackInfo.MyDirection.FORWARD, (int)TrackInfo.MyDirection.BACK, new Vector3(0, 180, 0), new Vector3(0, 180, 0) };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.AROUND_TRACK_INFO, trackData, raiseEventOptions, SendOptions.SendReliable);
                }
                else
                {
                    forwardHit = new RaycastHit();
                }
            }

            if (Physics.Raycast(new Ray(track.transform.position, Vector3.back), out backHit, maxDistance))
            {
                Debug.Log(backHit.transform.name);
                if (backHit.transform.CompareTag(trackTagName))
                {
                    TrackInfo backTrackInfo = backHit.transform.GetComponent<TrackInfo>();
                   
                    Debug.Log("이즈마인아님");
                    backTrackInfo.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    backTrackInfo.GetComponent<TrackInfo>().prevMyMesh = backTrackInfo.transform.GetComponent<MeshFilter>().mesh;
                    Debug.Log("각도 :::: "+backTrackInfo.transform.localEulerAngles.y);
                    if (backTrackInfo.transform.forward == Vector3.right)
                    {
                        backTrackInfo.transform.GetComponent<MeshFilter>().mesh = trackLeftMesh;
                        Destroy(backTrackInfo.transform.GetComponent<BoxCollider>());
                        backTrackInfo.AddComponent<BoxCollider>();
                    }
                    else if (backTrackInfo.transform.forward == Vector3.left)
                    {
                        backTrackInfo.transform.GetComponent<MeshFilter>().mesh = trackRightMesh;
                        Destroy(backTrackInfo.transform.GetComponent<BoxCollider>());
                        backTrackInfo.AddComponent<BoxCollider>();
                    }

                    backTrackInfo.prevAngle = backTrackInfo.transform.eulerAngles;
                    backTrackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(MapInfo.trackDefaultXRotation, MapInfo.trackDefaultYRotation, MapInfo.trackDefaultZRotation));
                    
                    _trackInfo.SetMyDirection(TrackInfo.MyDirection.BACK, new Vector3(MapInfo.trackDefaultXRotation, MapInfo.trackDefaultYRotation, MapInfo.trackDefaultZRotation));
                    _trackInfo.isElectricityFlowing = true;

                    if (backTrackInfo.isElectricityFlowing)
                    {
                        electricityFlowingList.Add(backTrackInfo.transform);
                    }
                    object[] trackData = new object[] { _trackInfo.GetComponent<PhotonView>().ViewID, backTrackInfo.GetComponent<PhotonView>().ViewID, (int)TrackInfo.MyDirection.BACK, (int)TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.AROUND_TRACK_INFO, trackData, raiseEventOptions, SendOptions.SendReliable);
                }
                else
                {
                    backHit = new RaycastHit();
                }
            }

            if ((leftHit.transform != null && leftHit.transform.gameObject != finalTrack) && (rightHit.transform != null && rightHit.transform.gameObject != finalTrack) 
                || (leftHit.transform != null && leftHit.transform.gameObject != finalTrack) && (rightHit.transform == null)
                || (rightHit.transform != null && rightHit.transform.gameObject != finalTrack) && (leftHit.transform == null)
                || (forwardHit.transform != null && forwardHit.transform.gameObject != finalTrack) && (backHit.transform != null && backHit.transform.gameObject != finalTrack)
                || (forwardHit.transform != null && forwardHit.transform.gameObject != finalTrack) && (backHit.transform == null)
                || (backHit.transform != null && backHit.transform.gameObject != finalTrack) && (forwardHit.transform == null)
                )
            {
                TrackConnectFailed(track);
                Debug.Log("Failed");
            }

            if ((leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform == null)
                || (leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform != null 
                && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
            {
                TrackConnectFailed(track);
                Debug.Log("Failed");
            }
            if ((rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform == null)
                ||(rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform != null 
                && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
            {
                TrackConnectFailed(track);
                Debug.Log("Failed");
            }

            if ((forwardHit.transform != null && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && backHit.transform == null)
                || (forwardHit.transform != null && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && backHit.transform != null
                && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
            {
                TrackConnectFailed(track);
                Debug.Log("Failed");
            }
            if ((backHit.transform != null && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && forwardHit.transform == null)
                || (backHit.transform != null && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && forwardHit.transform != null
                && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
            {
                TrackConnectFailed(track);
                Debug.Log("Failed");
            }

            if (forwardHit.transform == null && backHit.transform == null && leftHit.transform == null && rightHit.transform == null)
            {
                TrackConnectFailed(track);
                Debug.Log("Failed");
            }

            if (electricityFlowingList.Count >= 2)
            {
                int count = 0;
                for (int i = 0; i < electricityFlowingList.Count; i++)
                {
                    if (electricityFlowingList[i].GetComponent<TrackInfo>().isFinishedTrack)
                    {
                        count++;
                        track.GetComponent<TrackInfo>().isFinishedTrack = true;
                    }
                }
                if (count <= 0)
                {
                    for (int i = 0; i < electricityFlowingList.Count; i++)
                    {
                        electricityFlowingList[i].localEulerAngles = electricityFlowingList[i].GetComponent<TrackInfo>().prevAngle;
                    }
                    TrackConnectFailed(track);
                    Debug.Log("Failed");
                }

            }

            if (!trackConnectFailed)
            {
                finalTrack = _trackInfo.gameObject;
                //track.AddComponent<Outline>();
                //track.GetComponent<Outline>().OutlineColor = Color.white;
                //track.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
                PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_CRAETED_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                if (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    leftHit.transform.GetComponent<TrackInfo>().GetOnFactoriesObject();
                    object[] factoriesObjectData = new object[] { leftHit.transform.GetComponent<PhotonView>().ViewID};
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_FACTORIESOBJECT_INFO, factoriesObjectData, raiseEventOptions, SendOptions.SendReliable);
                }
                if (rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    rightHit.transform.GetComponent<TrackInfo>().GetOnFactoriesObject();
                    object[] factoriesObjectData = new object[] { rightHit.transform.GetComponent<PhotonView>().ViewID };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_FACTORIESOBJECT_INFO, factoriesObjectData, raiseEventOptions, SendOptions.SendReliable);
                }
                if (forwardHit.transform != null && forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    forwardHit.transform.GetComponent<TrackInfo>().GetOnFactoriesObject();
                    object[] factoriesObjectData = new object[] { forwardHit.transform.GetComponent<PhotonView>().ViewID };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_FACTORIESOBJECT_INFO, factoriesObjectData, raiseEventOptions, SendOptions.SendReliable);

                }
                if (backHit.transform != null && backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    backHit.transform.GetComponent<TrackInfo>().GetOnFactoriesObject();
                    object[] factoriesObjectData = new object[] { backHit.transform.GetComponent<PhotonView>().ViewID };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_FACTORIESOBJECT_INFO, factoriesObjectData, raiseEventOptions, SendOptions.SendReliable);
                }
            }
            else
            {
                if (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log("왼쪽이 다시 변경되야됨");
                    leftHit.transform.GetComponent<TrackInfo>().SetMyDirection(leftTrackPrevDirection, leftHit.transform.GetComponent<TrackInfo>().prevAngle);
                    leftHit.transform.GetComponent<MeshFilter>().mesh = leftHit.transform.GetComponent<TrackInfo>().prevMyMesh;
                    object[] trackData = new object[] { leftHit.transform.GetComponent<PhotonView>().ViewID,(int)leftTrackPrevDirection,(Vector3)leftHit.transform.GetComponent<TrackInfo>().prevAngle };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_PREV_INFO, trackData, raiseEventOptions, SendOptions.SendReliable);
                }

                if (rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log("오른쪽이 다시 변경되야됨");
                    rightHit.transform.GetComponent<TrackInfo>().SetMyDirection(rightTrackPrevDirection, rightHit.transform.GetComponent<TrackInfo>().prevAngle);
                    rightHit.transform.GetComponent<MeshFilter>().mesh = rightHit.transform.GetComponent<TrackInfo>().prevMyMesh;
                    object[] trackData = new object[] { rightHit.transform.GetComponent<PhotonView>().ViewID, (int)rightTrackPrevDirection, (Vector3)rightHit.transform.GetComponent<TrackInfo>().prevAngle };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_PREV_INFO, trackData, raiseEventOptions, SendOptions.SendReliable);
                }

                if (forwardHit.transform != null && forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log("위쪽");
                    forwardHit.transform.GetComponent<TrackInfo>().SetMyDirection(forwardTrackPrevDirection, forwardHit.transform.GetComponent<TrackInfo>().prevAngle);
                    forwardHit.transform.GetComponent<MeshFilter>().mesh = forwardHit.transform.GetComponent<TrackInfo>().prevMyMesh;
                    object[] trackData = new object[] { forwardHit.transform.GetComponent<PhotonView>().ViewID, (int)forwardTrackPrevDirection, (Vector3)forwardHit.transform.GetComponent<TrackInfo>().prevAngle };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_PREV_INFO, trackData, raiseEventOptions, SendOptions.SendReliable);
                }

                if (backHit.transform != null && backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log("아랫쪽");
                    backHit.transform.GetComponent<TrackInfo>().SetMyDirection(backTrackPrevDirection, backHit.transform.GetComponent<TrackInfo>().prevAngle);
                    backHit.transform.GetComponent<MeshFilter>().mesh = backHit.transform.GetComponent<TrackInfo>().prevMyMesh;
                    object[] trackData = new object[] { backHit.transform.GetComponent<PhotonView>().ViewID, (int)backTrackPrevDirection, (Vector3)backHit.transform.GetComponent<TrackInfo>().prevAngle };
                    RaiseEventOptions rEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
                    PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_PREV_INFO, trackData, raiseEventOptions, SendOptions.SendReliable);
                }

                trackConnectFailed = false;
            }
            leftHit = new RaycastHit();
            rightHit = new RaycastHit();
            forwardHit = new RaycastHit();
            backHit = new RaycastHit();
        }
        electricityFlowingList.Clear();
    }

    void ResetTrackRotation(int viewId, int trackPrevDirection,Vector3 prevAngle)
    {
        GameObject track = PhotonView.Find(viewId).gameObject;
        TrackInfo.MyDirection prevDirection = (TrackInfo.MyDirection)trackPrevDirection;
        track.GetComponent<TrackInfo>().SetMyDirection(prevDirection, prevAngle);
        track.GetComponent<MeshFilter>().mesh = track.GetComponent<TrackInfo>().prevMyMesh;
        Destroy(track.transform.GetComponent<BoxCollider>());
        track.AddComponent<BoxCollider>();
    }

    void TrackConnectFailed(GameObject track)
    {
        trackConnectFailed = true;
        GameObject _droppedSlot = PhotonNetwork.Instantiate("DroppedSlot",track.transform.position,track.transform.rotation);
        _droppedSlot.tag = "DroppedSlot";
        _droppedSlot.name = "DroppedSlot";

        track.transform.parent = _droppedSlot.transform;
        track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
        track.tag = "Item";
        track.GetComponent<TrackInfo>().isElectricityFlowing = false;
        track.AddComponent<ItemManager>();
        track.GetComponent<ItemManager>().itemType = ItemManager.ITEMTYPE.DROPPEDTRACK;

        _droppedSlot.GetComponent<InventoryManager>().itemType = ItemManager.ITEMTYPE.DROPPEDTRACK;
        _droppedSlot.GetComponent<InventoryManager>().DroppedSlotIn(track);
        //track.layer = 0;


        object[] data = new object[] { track.GetComponent<PhotonView>().ViewID,_droppedSlot.GetComponent<PhotonView>().ViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_INFO, data, raiseEventOptions, SendOptions.SendReliable);
    }
    void SetAroundTrack(int _trackViewId, int _aroundTrackViewId, int _trackDirection, int _aroundTrackDirection, Vector3 _trackRotation, Vector3 _aroundTrackRotation)
    {
        GameObject _track = PhotonView.Find(_trackViewId).gameObject;
        GameObject _aroundTrack = PhotonView.Find(_aroundTrackViewId).gameObject;

        TrackInfo aroundTrackInfo = _aroundTrack.transform.GetComponent<TrackInfo>();
        TrackInfo _trackInfo = _track.transform.GetComponent<TrackInfo>();
        aroundTrackInfo.prevAngle = aroundTrackInfo.transform.eulerAngles;
        aroundTrackInfo.prevMyMesh = aroundTrackInfo.GetComponent<MeshFilter>().mesh;
        
        //메쉬 설정

        aroundTrackInfo.SetMyDirection((TrackInfo.MyDirection)_aroundTrackDirection, _aroundTrackRotation);
        _trackInfo.SetMyDirection((TrackInfo.MyDirection)_trackDirection, _trackRotation);
        _trackInfo.isElectricityFlowing = true;
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (int)SendDataInfo.Info.TRACK_CRAETED_INFO)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            int trackViewId = (int)receivedData[0];
            if (PhotonView.Find(trackViewId).GetComponent<TrackInfo>() == null)
            {
                AddComponent(trackViewId);
            }
            GameObject track = PhotonView.Find(trackViewId).gameObject;
            track.tag = trackTagName;
            finalTrack = track;
            //track.AddComponent<Outline>();
            //track.GetComponent<Outline>().OutlineColor = Color.white;
            //track.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;

        }
        if (photonEvent.Code == (int)SendDataInfo.Info.TRACK_INFO)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            int trackViewId = (int)receivedData[0];
            int droppedSlotViewID = (int)receivedData[1];

            GameObject _droppedSlot = PhotonView.Find(droppedSlotViewID).gameObject;
            GameObject track = PhotonView.Find(trackViewId).gameObject;
            _droppedSlot.tag = "DroppedSlot";
            _droppedSlot.name = "DroppedSlot";

            track.transform.parent = _droppedSlot.transform;
            track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
            track.tag = "Item";
            track.GetComponent<TrackInfo>().isElectricityFlowing = false;
            track.AddComponent<ItemManager>();
            track.GetComponent<ItemManager>().itemType = ItemManager.ITEMTYPE.DROPPEDTRACK;


            _droppedSlot.GetComponent<InventoryManager>().itemType = ItemManager.ITEMTYPE.DROPPEDTRACK;
            _droppedSlot.GetComponent<InventoryManager>().DroppedSlotIn(track);
        }
        if (photonEvent.Code == (int)SendDataInfo.Info.AROUND_TRACK_INFO)
        {
            object[] receivedData = (object[])photonEvent.CustomData;

            int _trackViewID = (int)receivedData[0];
            int _aroundTrackViewId = (int)receivedData[1];
            int _trackDirection = (int)receivedData[2];
            int _aroundDirection = (int)receivedData[3];
            Vector3 _trackRotation = (Vector3)receivedData[4];
            Vector3 _aroundRotation = (Vector3)receivedData[5];
            SetAroundTrack(_trackViewID, _aroundTrackViewId, _trackDirection, _aroundDirection, _trackRotation, _aroundRotation);
        }
        if (photonEvent.Code == (int)SendDataInfo.Info.TRACK_PREV_INFO)
        {
            object[] receivedData = (object[])photonEvent.CustomData;

            int _trackViewID = (int)receivedData[0];
            int _trackPrevDirection = (int)receivedData[1];
            Vector3 prevAngle = (Vector3)receivedData[2];
            ResetTrackRotation(_trackViewID, _trackPrevDirection, prevAngle);
        }
        if (photonEvent.Code == (int)SendDataInfo.Info.TRACK_FACTORIESOBJECT_INFO)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            int _trackViewID = (int)receivedData[0];
            TrackInfo _trackInfo = PhotonView.Find(_trackViewID).GetComponent<TrackInfo>();
            _trackInfo.GetOnFactoriesObject();
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
