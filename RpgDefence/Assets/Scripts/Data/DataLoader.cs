using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    로드할 데이터 유형에 맞춰서 key, value 형식으로 데이터를 담게끔 인터페이스를 만듬
    ex) key를 int로 찾을껀지 object로 찾을건지.. .등등 다르기 때문에 유연하게 설정함
*/
public interface DataLoader<T, t>
{
    Dictionary<T, t> MakeDict();
}
