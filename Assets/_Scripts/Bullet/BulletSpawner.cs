using System;
using System.Collections.Generic;
using System.Linq;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class BulletSpawner : MonoBehaviour, IEffectTarget
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Stats characterStats;
    
    [SerializeField] private Transform firePoint;
    private float _firePointDistance;
    
    private List<BulletModifierInfo> _modifiers;
    
    private Dictionary<Type, BulletShootModifier> _shootModifiers;
    private List<BulletShootModifier> _shootModifiersList;

    private HashSet<Bullet> _bullets;

    public void Initialize(List<BulletModifierInfo> modifiers)
    {
        _modifiers = modifiers;
        _shootModifiers = new Dictionary<Type, BulletShootModifier>();
        _bullets = new HashSet<Bullet>();
        
        foreach (var modifier in modifiers)
        {
            var mod = modifier.GetModifier(this);
            if (mod is BulletShootModifier shootModifier)
            {
                ApplyEffect(shootModifier);
            }
        }

        _shootModifiersList = _shootModifiers.Values.OrderByDescending(x => x.Priority).ToList();
        
        _firePointDistance = Vector2.Distance(firePoint.position, transform.position);
    }

    public void SpawnBullet(Vector2 direction)
    {
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
        List<BulletModifierInfo> modifiers = _modifiers.Where(x => x.GetModifier(this).GetType() != typeof(BulletShootModifier)).ToList();
        if(shotData[0].Modifiers != null) modifiers.AddRange(shotData[0].Modifiers);
        modifiers = modifiers.OrderByDescending(x => x.priority).ToList();
        
        foreach (var bulletShot in shotData)
        {
            Vector2 dir = bulletShot.Direction.normalized;

            GameObject bullet = ObjectPool.Instance.InstantiateFromPool(bulletPrefab, bulletShot.StartPosition, Quaternion.identity, true);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bullet.transform.right = dir;
            
            bulletComponent.Initialize(dir, modifiers, this, characterStats);
            _bullets.Add(bulletComponent);
        }
    }

    private void FixedUpdate()
    {
        if(_bullets == null) return;
        
        foreach (var bullet in _bullets.Where(bullet => bullet.gameObject.activeInHierarchy))
            bullet.Move();
    }
    
    public void MoveFirePoint(Vector2 direction)
    {
        firePoint.position = _firePointDistance*direction + (Vector2) transform.position;
        firePoint.right = direction;
    }

    public void ApplyEffect(BaseEffect effect)
    {
        if(!_shootModifiers.ContainsKey(effect.GetType()))
            _shootModifiers.Add(effect.GetType(), (BulletShootModifier) effect);
        
        _shootModifiers[effect.GetType()].Apply();
        _shootModifiers[effect.GetType()].RemainsAfterHit--;
    }
    public void ReApplyEffects() { }
}