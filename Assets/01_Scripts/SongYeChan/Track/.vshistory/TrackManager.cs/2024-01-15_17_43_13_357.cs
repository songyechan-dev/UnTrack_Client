using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public string planeTagName = "Plane";
    public string trackTagName = "Track";
    public GameObject trackPrefab;
    public GameObject droppedTrackPrefab;
    public float maxDistance = 1f;
    public bool isChangedDistance = false;
    public Vector3 electricityObjectDirection;
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
                        leftTrackInfo.SetMyDirection(TrackInfo.MyDirection.RIGHT);
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.RIGHT);
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
                        rightTrackInfo.SetMyDirection(TrackInfo.MyDirection.LEFT);
                        _trackInfo.SetMyDirection(TrackInfo.MyDirection.LEFT);
                        if (!rightTrackInfo.isElectricityFlowing)
                        {
                            Debug.Log("ø¿∏•¬ ¿Ã≤®¡Æ¿÷¥Ÿ");
                        }
                    }
                }

                if (leftHit.transform != null && !leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && rightHit.transform == null)
                {
                    track.GetComponent<Renderer>().material = droppedTrackPrefab.GetComponent<Renderer>().material;
                }
                if (rightHit.transform != null && !rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing && leftHit.transform == null)
                {
                    track.GetComponent<Renderer>().material = droppedTrackPrefab.GetComponent<Renderer>().material;
                }



            }
        }
    }
}
