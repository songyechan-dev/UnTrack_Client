using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
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
}
