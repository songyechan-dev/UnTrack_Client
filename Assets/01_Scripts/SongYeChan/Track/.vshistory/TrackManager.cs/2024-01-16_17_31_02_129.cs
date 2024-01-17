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
                        leftTrackInfo.SetMyDirection(TrackInfo.MyDirection.RIGHT,new Vector3(0,90f,0));
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
                        rightTrackInfo.SetMyDirection(TrackInfo.MyDirection.LEFT, new Vector3(0, -90f, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 90f, 0));
                        _trackInfo.isElectricityFlowing = true;
   
                        if (!rightTrackInfo.isElectricityFlowing)
                        {
                            Debug.Log("오른쪽이꺼져있다");
                        }
                    }
                }

                if (Physics.Raycast(new Ray(track.transform.position, Vector3.forward), out forwardHit, maxDistance))
                {
                    if (forwardHit.transform.CompareTag(trackTagName))
                    {
                        TrackInfo forwardTrackInfo = forwardHit.transform.GetComponent<TrackInfo>();
                        forwardTrackInfo.SetMyDirection(TrackInfo.MyDirection.BACK, new Vector3(0, 180f, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 0f, 0));
                        _trackInfo.isElectricityFlowing = true;

                        if (!forwardTrackInfo.isElectricityFlowing)
                        {
                            Debug.Log("앞쪽이 꺼져있다");
                        }
                    }
                }

                if (Physics.Raycast(new Ray(track.transform.position, Vector3.back), out backHit, maxDistance))
                {
                    if (backHit.transform.CompareTag(trackTagName))
                    {
                        TrackInfo backTrackInfo = backHit.transform.GetComponent<TrackInfo>();
                        backTrackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 0f, 0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.BACK, new Vector3(0, 180f, 0));
                        _trackInfo.isElectricityFlowing = true;

                        if (!backTrackInfo.isElectricityFlowing)
                        {
                            Debug.Log("뒤쪽이 꺼져있다");
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

                if ((forwardHit.transform != null && forwardHit.transform.gameObject != finalTrack) && (backHit.transform != null && backHit.transform.gameObject != finalTrack)
                || (forwardHit.transform != null && forwardHit.transform.gameObject != finalTrack) && (backHit.transform == null)
                || (backHit.transform != null && backHit.transform.gameObject != finalTrack) && (forwardHit.transform == null))
                {
                    trackConnectFailed = true;
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                    Debug.Log("마지막 라인 아님");
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

                if (forwardHit.transform == null && backHit.transform == null)
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
                    trackConnectFailed = false;
                }
                forwardHit = new RaycastHit();
                backHit = new RaycastHit();
                leftHit = new RaycastHit();
                rightHit = new RaycastHit();
            }
        }
    }

    void SetCurvedTrack(TrackInfo.MyDirection _myDirection,TrackInfo trackInfo)
    {
    
    }
}
