using System;
using System.Collections.Generic;
using System.Linq;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class BulletSpawner : MonoBehaviour, IEffectTarget
{
    [SerializeField] private GameObject bulletPrefab;
    private Stats _characterStats;
    
    [SerializeField] private Transform firePoint;
    private float _firePointDistance;
    
    private List<BulletModifierInfo> _modifiers;
    
    private Dictionary<Type, BulletShotModifier> _shootModifiers;
    private List<BulletShotModifier> _shootModifiersList;

    //private HashSet<Bullet> _bullets;

    public void Initialize(List<BulletModifierInfo> modifiers)
    {
        _characterStats = GetComponent<StatsManager>().Stats;
        _shootModifiers = new Dictionary<Type, BulletShotModifier>();
        _modifiers = new List<BulletModifierInfo>();
        //_bullets = new HashSet<Bullet>();
        
        foreach (var modifier in modifiers)
        {
            if (modifier is BulletShotModifierInfo)
                ApplyEffect(modifier.GetModifier(this) as BulletShotModifier);
            else _modifiers.Add(modifier);
        }

        _shootModifiersList = _shootModifiers.Values.OrderByDescending(x => x.Priority).ToList();
        
        _firePointDistance = Vector2.Distance(firePoint.position, transform.position);
    }

    public void SpawnBullet(Vector2 direction)
    {
        //Get all BulletShotData (directions, modifiers)
        List<BulletShotData> shotData = new List<BulletShotData>();
        shotData.Add(new BulletShotData(firePoint.position, _firePointDistance, direction, null));
        
        foreach (var shootModifier in _shootModifiersList)
        {
            List<BulletShotData> temp = new List<BulletShotData>();
            foreach (var data in shotData)
            {
                temp.AddRange(shootModifier.GetModifications(data));
            }
            shotData = temp;
        }

        //Add modifiers that are not BulletShootModifiers and sort them by priority
        List<BulletModifierInfo> modifiers = new List<BulletModifierInfo>(_modifiers);
        if(shotData[0].Modifiers != null) modifiers.AddRange(shotData[0].Modifiers);
        modifiers = modifiers.OrderByDescending(x => x.priority).ToList();
        
        //Spawn bullets
        foreach (var bulletShot in shotData)
        {
            Vector2 dir = bulletShot.Direction.normalized;

            GameObject bullet = ObjectPool.Instance.InstantiateFromPool(bulletPrefab, bulletShot.StartPosition, Quaternion.identity, true);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bullet.transform.right = dir;
            
            bulletComponent.Initialize(dir, modifiers, this, _characterStats);
            //_bullets.Add(bulletComponent);
        }
    }

    // private void FixedUpdate()
    // {
    //     if(_bullets == null) return;
    //
    //     foreach (var bullet in _bullets)
    //         bullet.Move();
    // }
    
    public void MoveFirePoint(Vector2 direction)
    {
        firePoint.position = _firePointDistance*direction + (Vector2) transform.position;
        firePoint.right = direction;
    }
    
    public void ApplyEffect(IEffect effect)
    {
        if(!_shootModifiers.ContainsKey(effect.GetType()))
            _shootModifiers.Add(effect.GetType(), (BulletShotModifier) effect);
        
        _shootModifiers[effect.GetType()].Apply();
       //_shootModifiers[effect.GetType()].RemainsAfterHit--;
    }
    
    public void ReApplyEffects() { }
}