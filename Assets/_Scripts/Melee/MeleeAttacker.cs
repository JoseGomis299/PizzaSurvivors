using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour
{
    private float _lastAttackTime;
    private Vector2 lastDir;

    [SerializeField] private Vector2 hitboxMult = new Vector2(1f, 0.25f);

    private Rectangle lastHitbox;
    
    // Start is called before the first frame update
    void Start()
    {
        _lastAttackTime = float.MinValue;
    }

    public void Initialize(List<BulletModifierInfo> modifiers)
    {
        _lastAttackTime = float.MinValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool MeleeAttack(Transform enemy, Transform player, float timeBetweenAttacks, float attackRange, float dmg, LayerMask layer)
    {
        Console.WriteLine("Start");
        if (timeBetweenAttacks < Time.time - _lastAttackTime){return false;}

        Rectangle attackHitbox = new Rectangle(new Vector2(attackRange * hitboxMult.x, attackRange * hitboxMult.y));
        lastHitbox = attackHitbox;
        MeleeAttack attack = new MeleeAttack(new Damage(dmg, Element.None, 0, Vector3.zero), attackHitbox);

        Vector2 dir = player.position - enemy.position;
        lastDir = dir;

        attack.Attack((Vector2)enemy.position + ( dir / 2f), dir.normalized, layer);
        
        _lastAttackTime = Time.time;

        return true;
    }

    public void ResetTimer()
    {
        _lastAttackTime = Time.time;
    }

    private void OnDrawGizmos()
    {
        lastHitbox.DrawGizmos((Vector2)transform.position +  lastDir / 2f, lastDir.normalized, Color.red);
    }
}
