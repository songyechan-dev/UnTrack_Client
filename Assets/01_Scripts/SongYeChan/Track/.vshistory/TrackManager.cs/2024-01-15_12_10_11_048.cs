using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public string planeTagName = "Plane";
    public string trackTagName = "Track";
    public GameObject trackPrefab;
    public float maxDistance = 1f;
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
                //left°¨Áö
                if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out hit,maxDistance))
                {
                    if (hit.transform.CompareTag(trackTagName))
                    {
                        hit.transform.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.RIGHT);
                        track.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.RIGHT);
                        Debug.Log("°¨ÁöµÊ");
                    }
                        
                }
            }
        }
    }
}
