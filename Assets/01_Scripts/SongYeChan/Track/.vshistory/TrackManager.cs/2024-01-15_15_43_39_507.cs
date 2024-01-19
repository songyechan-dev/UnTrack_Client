using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public string planeTagName = "Plane";
    public string trackTagName = "Track";
    public GameObject trackPrefab;
    public float maxDistance = 1f;

    public void TrackCreate(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag(planeTagName))
        {
            CreateTrack(hit.point);
        }
    }

    private void CreateTrack(Vector3 position)
    {
        GameObject track = Instantiate(trackPrefab, position + Vector3.up * trackPrefab.transform.localScale.y / 2f, Quaternion.identity);
        track.AddComponent<TrackInfo>().tag = trackTagName;

        RaycastHit leftHit, rightHit;

        if (CheckTrackDirection(track, Vector3.left, out leftHit))
        {
            SetTrackDirection(track, leftHit.transform, TrackInfo.MyDirection.RIGHT);
        }

        if (CheckTrackDirection(track, Vector3.right, out rightHit))
        {
            SetTrackDirection(track, rightHit.transform, TrackInfo.MyDirection.LEFT);
        }

        CheckElectricityFlow(track, leftHit, rightHit);
    }

    private bool CheckTrackDirection(GameObject track, Vector3 direction, out RaycastHit hit)
    {
        if (Physics.Raycast(new Ray(track.transform.position, direction), out hit, maxDistance))
        {
            return hit.transform.CompareTag(trackTagName);
        }

        return false;
    }

    private void SetTrackDirection(GameObject track, Transform otherTrack, TrackInfo.MyDirection direction)
    {
        otherTrack.GetComponent<TrackInfo>().ChangeMyDirection(direction);
        track.GetComponent<TrackInfo>().ChangeMyDirection(direction);
        Debug.Log($"{direction} °¨ÁöµÊ: {otherTrack.gameObject.GetInstanceID()}");
    }

    private void CheckElectricityFlow(GameObject track, RaycastHit leftHit, RaycastHit rightHit)
    {
        if ((rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing) ||
            (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
        {
            track.GetComponent<TrackInfo>().isElectricityFlowing = true;
        }

        if (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>() != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
        {
            SetElectricityFlow(track, leftHit.transform.GetComponent<TrackInfo>());
        }

        if (rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>() != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
        {
            SetElectricityFlow(track, rightHit.transform.GetComponent<TrackInfo>());
        }
    }

    private void SetElectricityFlow(GameObject track, TrackInfo otherTrackInfo)
    {
        track.GetComponent<TrackInfo>().ChangeMyDirection(otherTrackInfo.GetMyRotation());
        track.GetComponent<TrackInfo>().isElectricityFlowing = true;
        Debug.Log($"{track.GetComponent<TrackInfo>().isElectricityFlowing}");
    }
}
