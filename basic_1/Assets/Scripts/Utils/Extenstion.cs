using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extenstion
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }
}
