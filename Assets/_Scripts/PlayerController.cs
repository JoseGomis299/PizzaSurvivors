using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _direction;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private BulletSpawner _bulletSpawner;
    
    private CharacterMovement _playerMovement;

    public List<BulletModifierInfo> modifiers;
    private void Awake()
    {
        //_animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bulletSpawner = GetComponent<BulletSpawner>();
        
        _playerMovement = GetComponent<CharacterMovement>();
        
        _bulletSpawner.Initialize(modifiers);
    }
    
    private void Update()
    {
        if(Time.timeScale == 0f) return;
        
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _direction.Normalize();
        
        if (_direction.x > 0) _spriteRenderer.flipX = false;
        else if (_direction.x < 0) _spriteRenderer.flipX = true;
        
        Vector2 mousePos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mousePos.Normalize();
        
        _bulletSpawner.MoveFirePoint(mousePos);
        
        if (Input.GetMouseButton(0))
        {
            _bulletSpawner.SpawnBullet(mousePos);
        }
    }
    
    private void FixedUpdate()
    {
        _playerMovement.UpdateMovement(_direction);
    }
}