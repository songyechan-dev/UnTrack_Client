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
                //left감지
                RaycastHit leftHit, rightHit;

                // 왼쪽 감지
                if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out leftHit, maxDistance))
                {
                    if (leftHit.transform.CompareTag(trackTagName))
                    {
                        leftHit.transform.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.RIGHT);
                        track.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.RIGHT);
                        Debug.Log($"왼쪽 감지됨{leftHit.transform.gameObject.GetInstanceID()}");
                    }
                }

                // 오른쪽 감지
                if (Physics.Raycast(new Ray(track.transform.position, Vector3.right), out rightHit, maxDistance))
                {
                    if (rightHit.transform.CompareTag(trackTagName))
                    {
                        rightHit.transform.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.LEFT);
                        track.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.LEFT);
                        Debug.Log($"오른쪽 감지됨{rightHit.transform.gameObject.GetInstanceID() }");
                    }
                }

                if ((rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing) || (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing))
                {
                    track.transform.GetComponent<TrackInfo>().isElectricityFlowing = true;
                    if (rightHit.transform != null && leftHit.transform != null)
                    {
                        rightHit.transform.GetComponent<TrackInfo>().SetElectricityFlowing(true);
                        leftHit.transform.GetComponent<TrackInfo>().SetElectricityFlowing(true);
                    }
                }

                //사이에 설치 할때
                if (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>() != null && rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>() != null && leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log((leftHit.transform).localEulerAngles);
                    Debug.Log((leftHit.transform.GetComponent<TrackInfo>().myDirection));
                    rightHit.transform.GetComponent<TrackInfo>().ChangeMyDirection(leftHit.transform.GetComponent<TrackInfo>().GetMyRotation());
                    rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing = true;

                    track.transform.GetComponent<TrackInfo>().ChangeMyDirection(leftHit.transform.GetComponent<TrackInfo>().GetMyRotation());
                    track.transform.GetComponent<TrackInfo>().isElectricityFlowing = true;
                    Debug.Log(track.transform.GetComponent<TrackInfo>().isElectricityFlowing);
                }
                if (leftHit.transform != null && leftHit.transform.GetComponent<TrackInfo>() != null && rightHit.transform != null && rightHit.transform.GetComponent<TrackInfo>() != null && rightHit.transform.GetComponent<TrackInfo>().isElectricityFlowing)
                {
                    Debug.Log((rightHit.transform.localEulerAngles));
                    Debug.Log(rightHit.transform.GetComponent<TrackInfo>().myDirection);
                    leftHit.transform.GetComponent<TrackInfo>().ChangeMyDirection(rightHit.transform.GetComponent<TrackInfo>().GetMyRotation());
                    leftHit.transform.GetComponent<TrackInfo>().isElectricityFlowing = true;

                    track.transform.GetComponent<TrackInfo>().ChangeMyDirection(rightHit.transform.GetComponent<TrackInfo>().GetMyRotation());
                    track.transform.GetComponent<TrackInfo>().isElectricityFlowing = true;
                    Debug.Log(track.transform.GetComponent<TrackInfo>().isElectricityFlowing);
                }
                leftHit = new RaycastHit();
                rightHit = new RaycastHit();
            }
        }
    }
}
