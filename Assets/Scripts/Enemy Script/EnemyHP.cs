using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyHP
{
    int EnemyHp { get; set; }
    bool IsEnemyHit { get; set; }
    bool IsEnemyDead { get; set; }

    void EnemyDamage(int damage, int attackMethod);

    public void EnemyDying();
}
