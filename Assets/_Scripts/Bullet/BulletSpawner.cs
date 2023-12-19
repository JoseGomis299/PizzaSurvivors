using System;
using System.Collections.Generic;
using System.Linq;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [field: SerializeField] public GameObject bulletPrefab { get; private set; }
    private Stats _characterStats;
    
    [SerializeField] private Transform firePoint;
    private float _firePointDistance;
    
    private List<BulletModifierInfo> _modifiers;
    
    private List<BulletShotModifier> _shotModifiers;
    
    private float _lastShotTime;

    public void Initialize(List<BulletModifierInfo> modifiers, List<ShotModifierInfo> shotModifiers)
    {
        var shootModifiers = new Dictionary<ShotModifierInfo.ShootModifierType, BulletShotModifier>();
        _characterStats = GetComponent<StatsManager>().Stats;
        _modifiers = new List<BulletModifierInfo>(modifiers);
        _shotModifiers = new List<BulletShotModifier>();

        foreach (var mod in shotModifiers)
        {
            if (shootModifiers.ContainsKey(mod.type))
            {
                shootModifiers[mod.type].Apply();
                continue;
            }

            var modifier = mod.GetBulletShotModification(this);
            modifier.Apply();

            shootModifiers.Add(mod.type, modifier);
            _shotModifiers.Add(modifier);
        }
        
        //Sort lists on priority
        _shotModifiers = _shotModifiers.OrderByDescending(x => x.Priority).ToList();
        _modifiers = _modifiers.OrderByDescending(x => x.priority).ToList();
        
        _firePointDistance = Vector2.Distance(firePoint.position, transform.position);
        
        _lastShotTime = float.MinValue;
    }

    public void SpawnBullet(Vector2 direction)
    {
        if(_characterStats.AttackSpeed > Time.time - _lastShotTime) return;
        
        //Get all BulletShotData (directions, modifiers)
        List<BulletShotData> shotData = new List<BulletShotData>();
        shotData.Add(new BulletShotData(firePoint.position, _firePointDistance, direction, null));
        
        foreach (var shotModifier in _shotModifiers)
        {
            List<BulletShotData> temp = new List<BulletShotData>();
            foreach (var data in shotData)
            {
                temp.AddRange(shotModifier.GetModifiedShotData(data));
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
        
        _lastShotTime = Time.time;
    }

    public void MoveFirePoint(Vector2 direction)
    {
        firePoint.position = _firePointDistance*direction + (Vector2) transform.position;
        firePoint.right = direction;
    }
}