using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private BulletSpawner _bulletSpawner;

    public List<BulletModifierInfo> modifiers;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bulletSpawner = GetComponent<BulletSpawner>();
        
        _bulletSpawner.Initialize(modifiers);
    }
    
    private void Update()
    {
        if(Time.timeScale == 0f) return;
        
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _movement.Normalize();
        
        Vector2 mousePos = Helpers.Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mousePos.Normalize();
        
        _bulletSpawner.MoveFirePoint(mousePos);
        
        if (Input.GetMouseButtonDown(0))
        {
            _bulletSpawner.SpawnBullet(mousePos);
        }
    }
    
    private void FixedUpdate()
    {
        _rb.velocity = _movement * speed;
        
        if (_movement.x > 0) _spriteRenderer.flipX = false;
        else if (_movement.x < 0) _spriteRenderer.flipX = true;
    }
}