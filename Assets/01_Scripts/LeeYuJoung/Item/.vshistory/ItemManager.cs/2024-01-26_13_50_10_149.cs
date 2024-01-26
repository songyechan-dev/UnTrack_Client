using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    PhotonView pv;
    public enum ITEMTYPE
    {
        WOOD,      // ¸ñÀç
        STEEL,     // Ã¶Àç
        DROPPEDTRACK,  // ¿¬°áµÇÁö ¾ÊÀº Æ®·¢
        BUCKET,    // ¹°Åë
        DYNAMITE,  // ÆøÅº
        AX,        // µµ³¢
        PICK       // °î±ªÀÌ
    }
    public ITEMTYPE itemType;

    private void FixedUpdate()
    {
        if (pv != null && pv.IsMine)
        {
            PhotonTransformView transformView = GetComponent<PhotonTransformView>();
            if (transformView != null)
            {
                transformView.enabled = false;
            }
        }
        else
        {
            PhotonTransformView transformView = GetComponent<PhotonTransformView>();
            if (transformView != null)
            {
                transformView.enabled = true;
            }
        }
    }
}
