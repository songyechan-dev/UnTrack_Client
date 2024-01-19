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
                //left����
                RaycastHit leftHit, rightHit;

                // ���� ����
                if (Physics.Raycast(new Ray(track.transform.position, Vector3.left), out leftHit, maxDistance))
                {
                    if (leftHit.transform.CompareTag(trackTagName))
                    {
                        leftHit.transform.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.RIGHT);
                        track.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.RIGHT);
                        Debug.Log("���� ������");
                    }
                }

                // ������ ����
                if (Physics.Raycast(new Ray(track.transform.position, Vector3.right), out rightHit, maxDistance))
                {
                    if (rightHit.transform.CompareTag(trackTagName))
                    {
                        rightHit.transform.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.LEFT);
                        track.GetComponent<TrackInfo>().ChangeMyDirection(TrackInfo.MyDirection.LEFT);
                        Debug.Log("������ ������");
                    }
                }

                // ���ʿ� ��� ������ �����Ǿ����� üũ
                bool railsDetectedOnBothSides = leftHit.transform != null && rightHit.transform != null;

                if (railsDetectedOnBothSides)
                {
                    // ���ʿ� ������ ��� ������
                    Debug.Log("���ʿ� ������ �ֽ��ϴ�.");
                }
            }
        }
    }
}
