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

    public GameObject finalTrack;
    public bool trackConnectFailed = false;

    private List<Transform> electricityFlowingList = new List<Transform>();

    public void TrackCreate(Ray _ray)
    {
        RaycastHit hit;
        Ray ray = _ray;

        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag(planeTagName))
        {
            GameObject track = Instantiate(trackPrefab, hit.transform.position + new Vector3(0, trackPrefab.transform.localScale.y / 2, 0), Quaternion.identity);
            track.AddComponent<TrackInfo>();
            track.tag = trackTagName;
            TrackInfo _trackInfo = track.GetComponent<TrackInfo>();

            CheckAndHandleRaycastHit(_trackInfo, track.transform.position, Vector3.left, TrackInfo.MyDirection.RIGHT, new Vector3(0, 90f, 0));
            CheckAndHandleRaycastHit(_trackInfo, track.transform.position, Vector3.right, TrackInfo.MyDirection.LEFT, new Vector3(0, -90f, 0));
            CheckAndHandleRaycastHit(_trackInfo, track.transform.position, Vector3.forward, TrackInfo.MyDirection.BACK, new Vector3(0, 180f, 0));
            CheckAndHandleRaycastHit(_trackInfo, track.transform.position, Vector3.back, TrackInfo.MyDirection.FORWARD, new Vector3(0, 0, 0));

            // Additional checks and logic...

            if (!trackConnectFailed)
            {
                finalTrack = _trackInfo.gameObject;
                track.AddComponent<Outline>();
                track.GetComponent<Outline>().OutlineColor = Color.white;
                track.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;

                foreach (var electricityFlowingTrack in electricityFlowingList)
                {
                    electricityFlowingTrack.GetComponent<TrackInfo>().GetOnFactoriesObject();
                }
            }
            else
            {
                // Handle trackConnectFailed...
            }

            ResetRaycastHitVariables();
        }
    }

    private void CheckAndHandleRaycastHit(TrackInfo _trackInfo, Vector3 origin, Vector3 direction, TrackInfo.MyDirection trackDirection, Vector3 rotation)
    {
        RaycastHit hit;
        if (TryRaycast(origin, direction, out hit) && hit.transform.CompareTag(trackTagName))
        {
            HandleRaycastHit(_trackInfo, hit, trackDirection, rotation);
        }
    }

    private bool TryRaycast(Vector3 origin, Vector3 direction, out RaycastHit hit)
    {
        return Physics.Raycast(new Ray(origin, direction), out hit, maxDistance);
    }

    private void HandleRaycastHit(TrackInfo _trackInfo, RaycastHit hit, TrackInfo.MyDirection trackDirection, Vector3 rotation)
    {
        TrackInfo hitTrackInfo = hit.transform.GetComponent<TrackInfo>();
        hitTrackInfo.prevAngle = hitTrackInfo.transform.eulerAngles;
        hitTrackInfo.SetMyDirection(trackDirection, rotation);
        _trackInfo.SetMyDirection(TrackInfo.MyDirection.FORWARD, rotation);
        _trackInfo.isElectricityFlowing = true;

        if (hitTrackInfo.isElectricityFlowing)
        {
            electricityFlowingList.Add(hit.transform);
        }
    }

    private void ResetRaycastHitVariables()
    {
        electricityFlowingList.Clear();
        trackConnectFailed = false;
    }
}
