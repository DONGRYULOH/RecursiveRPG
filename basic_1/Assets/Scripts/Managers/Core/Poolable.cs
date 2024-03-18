using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 풀링할 대상
// 이 컴포넌트를 가지고 있으면 메모리 풀링 대상으로 인식
public class Poolable : MonoBehaviour
{
    public bool IsUsing;
}
