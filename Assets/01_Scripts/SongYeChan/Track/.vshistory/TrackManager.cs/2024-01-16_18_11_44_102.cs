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
                RaycastHit leftHit, rightHit, forwordHit, backHit;

                if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out leftHit, maxDistance))
                {
                    if (leftHit.transform.CompareTag(trackTagName))
                    {
                        TrackInfo leftTrackInfo = leftHit.transform.GetComponent<TrackInfo>();
                        leftTrackPrevAngle = leftTrackInfo.transform.eulerAngles;
                        leftTrackPrevDirection = leftTrackInfo.GetMyRotation();
                        leftTrackInfo.SetMyDirection(TrackInfo.MyDirection.RIGHT, new Vector3(0, 90f, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 90f, 0));
                        _trackInfo.isElectricityFlowing = true;
                        if (!leftTrackInfo.isElectricityFlowing)
                        {
                            Debug.Log("왼쪽이꺼져있다");
                        }
                    }
                }
   

                if (Physics.Raycast(new Ray(track.transform.position, Vector3.right), out rightHit, maxDistance))
                {
                    if (rightHit.transform.CompareTag(trackTagName))
                    {
                        TrackInfo rightTrackInfo = rightHit.transform.GetComponent<TrackInfo>();
                        rightTrackPrevAngle = rightTrackInfo.transform.eulerAngles;
                        rightTrackPrevDirection = rightTrackInfo.GetMyRotation();
                        rightTrackInfo.SetMyDirection(TrackInfo.MyDirection.LEFT, new Vector3(0, -90f, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 90f, 0));
                        _trackInfo.isElectricityFlowing = true;

                        if (!rightTrackInfo.isElectricityFlowing)
                        {
                            Debug.Log("오른쪽이꺼져있다");
                        }
                    }
                }



                if ((leftHit.transform != null && leftHit.transform.gameObject != finalTrack) && (rightHit.transform != null && rightHit.transform.gameObject != finalTrack) 
                    || (leftHit.transform != null && leftHit.transform.gameObject != finalTrack) && (rightHit.transform == null)
                    || (rightHit.transform != null && rightHit.transform.gameObject != finalTrack) && (leftHit.transform == null)
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

                if (leftHit.transform == null && rightHit.transform == null)
                {
                    trackConnectFailed = true;
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
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
                    trackConnectFailed = false;
                }
                leftHit = new RaycastHit();
                rightHit = new RaycastHit();
            }
        }
    }

    void SetCurvedTrack(TrackInfo.MyDirection _myDirection,TrackInfo trackInfo)
    {
    
    }
}
