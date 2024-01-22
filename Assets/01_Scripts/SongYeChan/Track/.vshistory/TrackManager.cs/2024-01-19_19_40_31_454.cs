using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TrackManager : MonoBehaviour
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

    private void HandleTrackCreation(GameObject track, TrackInfo _trackInfo, RaycastHit hit, Vector3 direction, TrackInfo.MyDirection prevDirection, Vector3 prevAngle)
    {
        if (hit.transform.CompareTag(trackTagName))
        {
            TrackInfo trackInfo = hit.transform.GetComponent<TrackInfo>();
            trackInfo.prevAngle = prevAngle;
            trackInfo.SetMyDirection(prevDirection, direction);
            _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, direction);
            _trackInfo.isElectricityFlowing = true;

            if (trackInfo.isElectricityFlowing)
            {
                electricityFlowingList.Add(hit.transform);
            }
        }
        else
        {
            hit = new RaycastHit();
        }
    }





    public void TrackCreate(Ray _ray)
    {
        RaycastHit hit;
        Ray ray = _ray;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag(planeTagName))
            {
                GameObject track = Instantiate(trackPrefab);
                track.transform.position = hit.transform.position + new Vector3(0, trackPrefab.transform.localScale.y / 2, 0);
                track.AddComponent<TrackInfo>();
                track.tag = trackTagName;
                TrackInfo _trackInfo = track.GetComponent<TrackInfo>();
                RaycastHit leftHit, rightHit, forwardHit, backHit;

                HandleTrackCreation(track, _trackInfo, leftHit, Vector3.left, TrackInfo.MyDirection.RIGHT, new Vector3(0, 90f, 0));
                HandleTrackCreation(track, _trackInfo, rightHit, Vector3.right, TrackInfo.MyDirection.LEFT, new Vector3(0, -90f, 0));
                HandleTrackCreation(track, _trackInfo, forwardHit, Vector3.forward, TrackInfo.MyDirection.BACK, new Vector3(0, 180f, 0));
                HandleTrackCreation(track, _trackInfo, backHit, Vector3.back, TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0));

                if ((leftHit.transform != null && leftHit.transform.gameObject != finalTrack) && (rightHit.transform != null && rightHit.transform.gameObject != finalTrack) 
                    || (leftHit.transform != null && leftHit.transform.gameObject != finalTrack) && (rightHit.transform == null)
                    || (rightHit.transform != null && rightHit.transform.gameObject != finalTrack) && (leftHit.transform == null)
                    || (forwardHit.transform != null && forwardHit.transform.gameObject != finalTrack) && (backHit.transform != null && backHit.transform.gameObject != finalTrack)
                    || (forwardHit.transform != null && forwardHit.transform.gameObject != finalTrack) && (backHit.transform == null)
                    || (backHit.transform != null && backHit.transform.gameObject != finalTrack) && (forwardHit.transform == null)
                    )
                {
                    trackConnectFailed = true;
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                    Debug.Log("마지막 라인 아님");
                }

                if ((leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform == null)
                    || (leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform != null 
                    && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
                {
                    trackConnectFailed = true;
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                }
                if ((rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform == null)
                    ||(rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform != null 
                    && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
                {
                    trackConnectFailed = true;
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                }

                if ((forwardHit.transform != null && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && backHit.transform == null)
                    || (forwardHit.transform != null && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && backHit.transform != null
                    && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
                {
                    trackConnectFailed = true;
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                }
                if ((backHit.transform != null && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && forwardHit.transform == null)
                    || (backHit.transform != null && !backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && forwardHit.transform != null
                    && !forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
                {
                    trackConnectFailed = true;
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                }

                if (forwardHit.transform == null && backHit.transform == null && leftHit.transform == null && rightHit.transform == null)
                {
                    trackConnectFailed = true;
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
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
                        trackConnectFailed = true;
                        track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                        track.tag = droppedTrackTagName;
                        _trackInfo.isElectricityFlowing = false;
                    }

                }

                if (!trackConnectFailed)
                {
                    finalTrack = _trackInfo.gameObject;
                    track.AddComponent<Outline>();
                    track.GetComponent<Outline>().OutlineColor = Color.white;
                    track.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
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
        }
        electricityFlowingList.Clear();
    }

}
