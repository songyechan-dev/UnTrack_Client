using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public string planeTagName = "Plane";
    public string trackTagName = "Track";
    public string droppedTrackTagName = "DroppedTrack";
    public GameObject trackPrefab;
    public Material droppedTrackPrefabMaterial;
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

                if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out leftHit, maxDistance))
                {
                    if (leftHit.transform.CompareTag(trackTagName))
                    {
                        TrackInfo leftTrackInfo = leftHit.transform.GetComponent<TrackInfo>();
                        leftTrackInfo.prevAngle = leftTrackInfo.transform.eulerAngles;
                        leftTrackInfo.SetMyDirection(TrackInfo.MyDirection.RIGHT, new Vector3(0, 90f, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 90f, 0));
                        _trackInfo.isElectricityFlowing = true;
                        if (leftTrackInfo.isElectricityFlowing)
                        {
                            electricityFlowingList.Add(leftHit.transform);
                        }
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
                        rightTrackPrevAngle = rightTrackInfo.transform.eulerAngles;
                        rightTrackInfo.SetMyDirection(TrackInfo.MyDirection.LEFT, new Vector3(0, -90f, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, -90f, 0));
                        _trackInfo.isElectricityFlowing = true;

                        if (rightTrackInfo.isElectricityFlowing)
                        {
                            electricityFlowingList.Add(rightHit.transform);
                        }
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
                        forwardTrackPrevAngle = forwardTrackInfo.transform.eulerAngles;
                        forwardTrackPrevDirection = forwardTrackInfo.GetMyRotation();
                        forwardTrackInfo.SetMyDirection(TrackInfo.MyDirection.BACK, new Vector3(0, 180f, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 180f, 0));
                        _trackInfo.isElectricityFlowing = true;

                        if (forwardTrackInfo.isElectricityFlowing)
                        {
                            electricityFlowingList.Add(forwardHit.transform);
                        }
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
                        backTrackPrevAngle = backTrackInfo.transform.eulerAngles;
                        backTrackPrevDirection = backTrackInfo.GetMyRotation();
                        backTrackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.BACK, new Vector3(0, 0, 0));
                        _trackInfo.isElectricityFlowing = true;

                        if (backTrackInfo.isElectricityFlowing)
                        {
                            electricityFlowingList.Add(backTrackInfo.transform);
                        }
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
                    for (int i = 0; i < electricityFlowingList.Count; i++)
                    {
                        electricityFlowingList[i].
                    }
                }

                if (!trackConnectFailed)
                {
                    finalTrack = _trackInfo.gameObject;
                }
                else
                {
                    if (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                    {
                        Debug.Log("왼쪽이 다시 변경되야됨");
                        leftHit.transform.GetComponent<TrackInfo>().SetMyDirection(leftTrackPrevDirection, leftTrackPrevAngle);
                    }

                    if (rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                    {
                        Debug.Log("오른쪽이 다시 변경되야됨");
                        rightHit.transform.GetComponent<TrackInfo>().SetMyDirection(rightTrackPrevDirection, rightTrackPrevAngle);
                    }

                    if (forwardHit.transform != null && forwardHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                    {
                        Debug.Log("위쪽");
                        forwardHit.transform.GetComponent<TrackInfo>().SetMyDirection(forwardTrackPrevDirection, forwardTrackPrevAngle);
                    }

                    if (backHit.transform != null && backHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                    {
                        Debug.Log("아랫쪽");
                        backHit.transform.GetComponent<TrackInfo>().SetMyDirection(backTrackPrevDirection, backTrackPrevAngle);
                    }

                    trackConnectFailed = false;
                }
                leftHit = new RaycastHit();
                rightHit = new RaycastHit();
                forwardHit = new RaycastHit();
                backHit = new RaycastHit();
            }
        }
    }

}
