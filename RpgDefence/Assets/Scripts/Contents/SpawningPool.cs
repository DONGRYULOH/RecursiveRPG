using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    int _monsterCount = 0;

    int _reserveCount = 0;

    int _keepMonsterCount = 0;

    Vector3 _spawnPos;

    float _spawnRadius = 15.0f;

    float _spawnTime = 5.0f;

    public void AddMonsterCount(int value)
    {
        _monsterCount += value;
    }

    public void SetKeepMonsterCount(int count)
    {
        _keepMonsterCount = count;
    }


    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    // Update is called once per frame
    void Update()
    {
        while(_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    // 바로 몬스터를 생성하는게 아니라 몇초정도 기다렸다가 생성할것이기 때문에 기다리는 시간동안 다른 작업이 실행될수 있도록 코루틴 처리
    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));

        GameObject go = Managers.Game.Spawn(Defines.WorldObject.Monster, "Warrior");
        NavMeshAgent nma = go.GetOrAddComponent<NavMeshAgent>(); // 길찾기 컴포넌트를 이용해서 스폰할 수 있는 영역인지 판단

        Vector3 randomPos;
        while (true)
        {
            Vector3 randomDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
            randomDir.y = 0;
            randomPos = go.transform.position + randomDir;

            // 스폰될 수 있는 영역인지 체크
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randomPos, path))
                break;
        }

        go.transform.position = randomPos;
        _reserveCount--;
    }
}
