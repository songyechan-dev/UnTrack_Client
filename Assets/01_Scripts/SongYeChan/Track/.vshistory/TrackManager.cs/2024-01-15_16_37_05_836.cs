using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public string planeTagName = "Plane";
    public string trackTagName = "Track";
    public GameObject trackPrefab;
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
                RaycastHit leftHit, rightHit, upHit, downHit;

                if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out leftHit, maxDistance))
                {
                    if (leftHit.transform.CompareTag(trackTagName))
                    {
                        leftHit.transform.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.RIGHT);
                        leftHit.transform.GetComponent<TrackInfo>().SetArroundTrack(track.transform, TrackInfo.ArroundTrackDirection.RIGHT);
                        track.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.RIGHT);
                        track.transform.GetComponent<TrackInfo>().SetArroundTrack(track.transform, TrackInfo.ArroundTrackDirection.LEFT);
                        Debug.Log($"¿ÞÂÊ °¨ÁöµÊ{leftHit.transform.gameObject.GetInstanceID()}");
                    }
                }

                if (Physics.Raycast(new Ray(track.transform.position, Vector3.right), out rightHit, maxDistance))
                {
                    if (rightHit.transform.CompareTag(trackTagName))
                    {
                        rightHit.transform.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.LEFT);
                        leftHit.transform.GetComponent<TrackInfo>().SetArroundTrack(track.transform, TrackInfo.ArroundTrackDirection.LEFT);
                        track.GetComponent<TrackInfo>().SetMyDirection(TrackInfo.MyDirection.LEFT);
                        track.transform.GetComponent<TrackInfo>().SetArroundTrack(track.transform, TrackInfo.ArroundTrackDirection.RIGHT);
                        Debug.Log($"¿ÞÂÊ °¨ÁöµÊ{leftHit.transform.gameObject.GetInstanceID()}");
                    }
                }


            }
        }
    }
}
