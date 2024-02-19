using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProjectUtils.ObjectPooling;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BulletSpawner : MonoBehaviour
{
    [field: SerializeField] public GameObject bulletPrefab { get; private set; }
    private Stats _characterStats;
    
    [SerializeField] private Transform firePoint;
    private float _firePointDistance;
    private Vector2 _firePointDirection;
    
    private List<BulletModifierInfo> _modifiers;
    
    private List<BulletShotModifier> _shotModifiers;
    
    private float _lastShotTime;

    public void Initialize(List<BulletModifierInfo> modifiers)
    {
        _characterStats = GetComponent<StatsManager>().Stats;
        
        _modifiers = new List<BulletModifierInfo>();
        _shotModifiers = new List<BulletShotModifier>();
      
        foreach (var mod in modifiers)
        {
            if(mod is ShotModifierInfo shotModifier)
            {
                var modifier = shotModifier.GetBulletShotModification(this);

                var found = _shotModifiers.Find(x => x.GetType() == modifier.GetType());
                if (found != null)
                {
                    found.Apply();
                    continue;
                }

                modifier.Apply();
                _shotModifiers.Add(modifier);
            }
            else
            {
                _modifiers.Add(mod);
            }
        }

        //Sort lists on priority
        _shotModifiers = _shotModifiers.OrderByDescending(x => x.Priority).ToList();
        _modifiers = _modifiers.OrderByDescending(x => x.priority).ToList();
        
        _firePointDistance = Vector2.Distance(firePoint.position, transform.position);
        
        _lastShotTime = float.MinValue;
    }

    public bool SpawnBullet(Vector2 direction)
    {
        if(_characterStats.AttackSpeed > Time.time - _lastShotTime) return false;
        
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
        _firePointDirection = direction;
        return true;
    }

    public void MoveFirePoint(Vector2 direction)
    {
        firePoint.position = _firePointDistance*direction + (Vector2) transform.position;
        firePoint.right = direction;
    }

    public Vector3 GetFirePoint()
    {
        return firePoint.position;
    }
    
    public Vector2 GetFirePointDirection()
    {
        return _firePointDirection;
    }

    public float GetFirePointDistance()
    {
        return _firePointDistance;
    }
    
    public void ResetTimer(){
        _lastShotTime = Time.time;
    }
}