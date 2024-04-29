using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool isFire;
    Vector3 direction;
    float range = 10f; // ÃÑ¾Ë »çÁ¤°Å¸®
    Vector3 startPos;  // ÃÑ¾Ë ½ÃÀÛ ÁÂÇ¥    
    float speed = 10f;

    public void Fire(Vector3 dir, Vector3 playerPos)
    {
        direction = dir;
        isFire = true;
        startPos = new Vector3(playerPos.x, 0.5f, playerPos.z);
        transform.position = new Vector3(playerPos.x, 0.5f, playerPos.z);        
    }
    
    void Update()
    {
        if (isFire)
        {            
            transform.position += direction * speed * Time.deltaTime;
            Vector3 bulletMoveRange = transform.position - startPos;            
            if (Mathf.Abs(bulletMoveRange.magnitude) >= range)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var monster = collision.collider.GetComponent<MonsterController>();
        if(monster != null)
        {
            var stat = monster.gameObject.GetComponent<MonsterStat>();
            stat.OnAttacked(Managers.Game.GetPlayer().GetComponent<Stat>());
        }
        Destroy(gameObject);
    }
}
