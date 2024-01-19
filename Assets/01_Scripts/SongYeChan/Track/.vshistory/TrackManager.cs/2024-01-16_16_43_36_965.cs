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
                RaycastHit leftHit, rightHit, upHit, downHit;

                if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out leftHit, maxDistance))
                {
                    if (leftHit.transform.CompareTag(trackTagName))
                    {
                        TrackInfo leftTrackInfo = leftHit.transform.GetComponent<TrackInfo>();
                        leftTrackInfo.SetMyDirection(TrackInfo.MyDirection.RIGHT,new Vector3(0,90f,0));
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, new Vector3(0, 90f, 0));
                        _trackInfo.isElectricityFlowing = true;
                        finalTrack = _trackInfo.gameObject;
                        if (!leftTrackInfo.isElectricityFlowing)
                        {
                            Debug.Log("øﬁ¬ ¿Ã≤®¡Æ¿÷¥Ÿ");
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
                        finalTrack = _trackInfo.gameObject;
                        if (!rightTrackInfo.isElectricityFlowing)
                        {
                            Debug.Log("ø¿∏•¬ ¿Ã≤®¡Æ¿÷¥Ÿ");
                        }
                    }
                }

                if ((leftHit.transform != null && leftHit.transform != finalTrack) && (rightHit.transform != null && rightHit.transform != finalTrack) 
                    || (leftHit.transform != null && leftHit.transform != finalTrack) && (rightHit.transform == null)
                    || (rightHit.transform != null && rightHit.transform != finalTrack) && (leftHit.transform == null)
                    )
                {
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                    finalTrack = null;
                }

                if ((leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform == null)
                    || (leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform != null 
                    && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
                {
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                    finalTrack = null;
                }
                if ((rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform == null)
                    ||(rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform != null 
                    && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
                {
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                }

                if (leftHit.transform == null && rightHit.transform == null)
                {
                    track.GetComponent<MeshRenderer>().material = droppedTrackPrefabMaterial;
                    track.tag = droppedTrackTagName;
                    _trackInfo.isElectricityFlowing = false;
                }
                leftHit = new RaycastHit();
                rightHit = new RaycastHit();
            }
        }
    }

    void SetCurvedTrack(TrackInfo.MyDirection _myDirection,TrackInfo trackInfo)
    {
    
    }

    void SetFinalTrack(GameObject gameObject)
    {
        if (finalTrackList.Count > 0)
        {
            finalTrackList.Dequeue();
        }
        finalTrackList.Enqueue(gameObject);
    }
}
