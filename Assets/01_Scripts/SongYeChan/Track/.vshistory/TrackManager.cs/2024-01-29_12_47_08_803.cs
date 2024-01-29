using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;

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

    private List<Transform> electricityFlowingList = new List<Transform>();
    public GameObject factoriesObjectPrefab;

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

            GameObject track = PhotonNetwork.Instantiate("Track", new Vector3(Mathf.Round(_ray.origin.x), 0 + (trackPrefab.transform.localScale.y / 2), Mathf.Round(_ray.origin.z)),Quaternion.identity);
            track.tag = trackTagName;
            track.AddComponent<TrackInfo>();
            object[] data = new object[] { track.GetComponent<PhotonView>().ViewID};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_CRAETED_INFO, data, raiseEventOptions, SendOptions.SendReliable);

            TrackInfo _trackInfo = track.GetComponent<TrackInfo>();
            RaycastHit leftHit, rightHit, forwardHit, backHit;

            if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out leftHit, maxDistance))
            {
                if (leftHit.transform.CompareTag(trackTagName))
                {
                    TrackInfo leftTrackInfo = leftHit.transform.GetComponent<TrackInfo>();
                    if (!PhotonView.Find(leftTrackInfo.GetComponent<PhotonView>().ViewID).IsMine)
                    {
                        Debug.Log("이즈마인아님");
                        leftTrackInfo.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    leftTrackInfo.prevAngle = leftTrackInfo.transform.eulerAngles;
                    leftTrackInfo.SetMyDirection(TrackInfo.MyDirection.RIGHT, new Vector3(0, 90f, 0));
                    _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 90f, 0));
                    _trackInfo.isElectricityFlowing = true;
                    if (leftTrackInfo.isElectricityFlowing)
                    {
                        electricityFlowingList.Add(leftHit.transform);
                    }

                    object[] trackData = new object[] { _trackInfo.GetComponent<PhotonView>().ViewID,leftTrackInfo.GetComponent<PhotonView>().ViewID, (int)TrackInfo.MyDirection.RIGHT, (int)TrackInfo.MyDirection.FORWARD, new Vector3(0, 90f, 0), new Vector3(0, 90f, 0) };
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
                if (rightHit.transform.CompareTag(trackTagName))
                {
                    TrackInfo rightTrackInfo = rightHit.transform.GetComponent<TrackInfo>();
                    if (!PhotonView.Find(rightTrackInfo.GetComponent<PhotonView>().ViewID).IsMine)
                    {
                        Debug.Log("이즈마인아님");
                        rightTrackInfo.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    rightTrackInfo.prevAngle = rightTrackInfo.transform.eulerAngles;
                    rightTrackInfo.SetMyDirection(TrackInfo.MyDirection.LEFT, new Vector3(0, -90f, 0));
                    _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, -90f, 0));
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
                if (forwardHit.transform.CompareTag(trackTagName))
                {
                    TrackInfo forwardTrackInfo = forwardHit.transform.GetComponent<TrackInfo>();
                    if (!PhotonView.Find(forwardTrackInfo.GetComponent<PhotonView>().ViewID).IsMine)
                    {
                        Debug.Log("이즈마인아님");
                        forwardTrackInfo.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    forwardTrackInfo.prevAngle = forwardTrackInfo.transform.eulerAngles;
                    forwardTrackInfo.SetMyDirection(TrackInfo.MyDirection.BACK, new Vector3(0, 180f, 0));
                    _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 180f, 0));
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
                if (backHit.transform.CompareTag(trackTagName))
                {
                    TrackInfo backTrackInfo = backHit.transform.GetComponent<TrackInfo>();
                    if (!PhotonView.Find(backTrackInfo.GetComponent<PhotonView>().ViewID).IsMine)
                    {
                        Debug.Log("이즈마인아님");
                        backTrackInfo.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    backTrackInfo.prevAngle = backTrackInfo.transform.eulerAngles;
                    backTrackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0));
                    _trackInfo.SetMyDirection(TrackInfo.MyDirection.BACK, new Vector3(0, 0, 0));
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
            }

            if ((leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform == null)
                || (leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform != null 
                && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
            {
                TrackConnectFailed(track);
            }
            if ((rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform == null)
                ||(rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform != null 
                && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
            {
                TrackConnectFailed(track);
            }

            if ((forwardHit.transform != null && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && backHit.transform == null)
                || (forwardHit.transform != null && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && backHit.transform != null
                && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
            {
                TrackConnectFailed(track);
            }
            if ((backHit.transform != null && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && forwardHit.transform == null)
                || (backHit.transform != null && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && forwardHit.transform != null
                && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
            {
                TrackConnectFailed(track);
            }

            if (forwardHit.transform == null && backHit.transform == null && leftHit.transform == null && rightHit.transform == null)
            {
                TrackConnectFailed(track);
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
                }

            }

            if (!trackConnectFailed)
            {
                finalTrack = _trackInfo.gameObject;
                track.AddComponent<Outline>();
                track.GetComponent<Outline>().OutlineColor = Color.white;
                track.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
                PhotonNetwork.RaiseEvent((int)SendDataInfo.Info.TRACK_CRAETED_INFO, data, raiseEventOptions, SendOptions.SendReliable);
                if (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    leftHit.transform.GetComponent<TrackInfo>().GetOnFactoriesObject();
                }
                if (rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    rightHit.transform.GetComponent<TrackInfo>().GetOnFactoriesObject();
                }
                if (forwardHit.transform != null && forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    forwardHit.transform.GetComponent<TrackInfo>().GetOnFactoriesObject();
                }
                if (backHit.transform != null && backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    backHit.transform.GetComponent<TrackInfo>().GetOnFactoriesObject();
                }
            }
            else
            {
                if (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log("왼쪽이 다시 변경되야됨");
                    leftHit.transform.GetComponent<TrackInfo>().SetMyDirection(leftTrackPrevDirection, leftHit.transform.GetComponent<TrackInfo>().prevAngle);
                }

                if (rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log("오른쪽이 다시 변경되야됨");
                    rightHit.transform.GetComponent<TrackInfo>().SetMyDirection(rightTrackPrevDirection, rightHit.transform.GetComponent<TrackInfo>().prevAngle);
                }

                if (forwardHit.transform != null && forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log("위쪽");
                    forwardHit.transform.GetComponent<TrackInfo>().SetMyDirection(forwardTrackPrevDirection, forwardHit.transform.GetComponent<TrackInfo>().prevAngle);
                }

                if (backHit.transform != null && backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log("아랫쪽");
                    backHit.transform.GetComponent<TrackInfo>().SetMyDirection(backTrackPrevDirection, backHit.transform.GetComponent<TrackInfo>().prevAngle);
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
        aroundTrackInfo.SetMyDirection((TrackInfo.MyDirection)_aroundTrackDirection, _aroundTrackRotation);
        _trackInfo.SetMyDirection((TrackInfo.MyDirection)_trackDirection, _trackRotation);
        _trackInfo.isElectricityFlowing = true;
        if (aroundTrackInfo.isElectricityFlowing)
        {
            electricityFlowingList.Add(_aroundTrack.transform);
        }
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
            finalTrack = track;
            track.AddComponent<Outline>();
            track.GetComponent<Outline>().OutlineColor = Color.white;
            track.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;

        }
        if (photonEvent.Code == (int)SendDataInfo.Info.TRACK_INFO)
        {
            object[] receivedData = (object[])photonEvent.CustomData;
            int trackViewId = (int)receivedData[0];
            int droppedSlotViewID = (int)receivedData[1];

            GameObject _droppedSlot = PhotonView.Find(droppedSlotViewID).gameObject;
            GameObject track = PhotonView.Find(trackViewId).gameObject;
            trackConnectFailed = true;
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
