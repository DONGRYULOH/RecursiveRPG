using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{        
    public enum CursorType
    {
        None,                
        NextChapter,
        Store
    }
    static public CursorType _cursorType = CursorType.None;       
}
