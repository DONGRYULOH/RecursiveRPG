using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;

    int _reserveCount = 0;

    int _keepMonsterCount = 0;

    float _spawnTime = 2.0f;

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
    
    void Update()
    {
        while(_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }

    // �ٷ� ���͸� �����ϴ°� �ƴ϶� �������� ��ٷȴٰ� �����Ұ��̱� ������ ��ٸ��� �ð����� �ٸ� �۾��� ����ɼ� �ֵ��� �ڷ�ƾ ó��
    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        
        string spawnMonsterName = null;
        if (Managers.Game.CurrentChpater == 1)
            spawnMonsterName = "Monster/Warrior";
        else if (Managers.Game.CurrentChpater == 2)
            spawnMonsterName = "Monster/Skeleton";
        else
            spawnMonsterName = "Monster/Skeleton";
        GameObject go = Managers.Game.Spawn(Defines.WorldObject.Monster, spawnMonsterName);

        NavMeshAgent nma = go.GetOrAddComponent<NavMeshAgent>(); // ��ã�� ������Ʈ�� �̿��ؼ� ������ �� �ִ� �������� �Ǵ�
        Vector3 randomPos;
        while (true)
        {
            Vector3 randomDir = Random.insideUnitSphere * Random.Range(0, 5.0f);
            randomDir.y = 0;
            randomPos = go.transform.position + randomDir;

            // ������ �� �ִ� �������� üũ
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randomPos, path))
                break;
        }

        go.transform.position = randomPos;
        _reserveCount--;
    }
}
