using System;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class CombosContainer : MonoBehaviour
{
    [Header("Combos (Must be aligned upwards)")]
    [SerializeField] private List<Combo> combos;
    private Color[] _colors = {Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white, Color.black};
    private Vector2 _lastDirection = Vector2.up;
    private StatsManager _statsManager;
    private bool _flipped = false;

    public void Start()
    {
        _statsManager = GetComponent<StatsManager>();
        _flipped = false;
    }
    
    public void Initialize(float attackRange)
    {
        foreach (Combo combo in combos)
        {
            foreach (var attack in combo.Attacks)
            {
                if (attack.MatchRange)
                {
                    switch (attack.ShapeType)
                    {
                        case ShapeType.Rectangle:
                            attack.Size = new Vector2(attack.Size.x, attackRange-0.5f);
                            attack.RelativePosition = new Vector2(0,  (attackRange-0.5f)/2 + 0.5f);
                            break;
                        case ShapeType.Line:
                            attack.Size = new Vector2(attackRange-0.5f, attack.Size.y);
                            attack.RelativePosition = new Vector2(0,  0.5f);
                            attack.Angle = 90;
                            break;
                        case ShapeType.Circle:
                            attack.Size = new Vector2(attackRange-0.5f, attack.Size.y);
                            attack.RelativePosition = new Vector2(0,  attackRange);
                            break;
                        case ShapeType.Capsule:
                            attack.Size = new Vector2(attack.Size.x, attackRange-0.5f);
                            attack.RelativePosition = new Vector2(0,  (attackRange-0.5f)/2 + 0.5f);
                            break;
                    }
                }
            }
        }
    }

    public void Attack(int combo, Vector2 direction, LayerMask layerMask)
    {
        _lastDirection = direction;
        float rotation = -Vector2.SignedAngle(_lastDirection, Vector2.up);

        foreach (Attack attack in combos[combo].Attacks)
        {
            if ((combos[combo].FlipXWhenFacingLeft && direction.x >= 0 && !_flipped) || (_flipped && direction.x < 0))
            {
                attack.RelativePosition = new Vector2(-attack.RelativePosition.x, attack.RelativePosition.y);
            }

            attack.GetAttack(_statsManager.Stats.Attack, transform.position).Attack((Vector2) transform.position + attack.RelativePosition.Rotate(rotation), attack.Angle + rotation, layerMask);
        }
        
        _flipped = direction.x >= 0;
    }
    
    public void AttackAll(int combo, Vector2 direction, LayerMask layerMask)
    {
        _lastDirection = direction;
        float rotation = -Vector2.SignedAngle(_lastDirection, Vector2.up);

        foreach (Attack attack in combos[combo].Attacks)
        {
            if ((combos[combo].FlipXWhenFacingLeft && direction.x >= 0 && !_flipped) || (_flipped && direction.x < 0))
            {
                attack.RelativePosition = new Vector2(-attack.RelativePosition.x, attack.RelativePosition.y);
            }

            attack.GetAttack(_statsManager.Stats.Attack, transform.position).AttackAll((Vector2) transform.position + attack.RelativePosition.Rotate(rotation), attack.Angle + rotation, layerMask);
        }
        
        _flipped = direction.x >= 0;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(combos == null) return;
        for (var i = 0; i < combos.Count; i++)
        {
            if (!combos[i].ShowGizmos) continue;
            
            float rotation = -Vector2.SignedAngle(_lastDirection, Vector2.up);
            
            foreach (Attack attack in combos[i].Attacks)
            {
                attack.GetShape().DrawGizmos((Vector2) transform.position + attack.RelativePosition.Rotate(rotation), attack.Angle + rotation, _colors[i]);
            }
        }
    }

    private void OnValidate()
    {
        Initialize(GetComponent<MeleeEnemy>().AttackRange);
    }
#endif
}

[Serializable]
public class Attack
{
    [SerializeField] private float damageMultiplier = 1;
    [SerializeField] private Element element = Element.None;
    [SerializeField] private float knockback = 0;
    
    public ShapeType ShapeType;
    public IShape GetShape()
    {
        return ShapeType switch
        {
            ShapeType.Rectangle => new Rectangle(Size),
            ShapeType.Circle => new Circle(Size.x),
            ShapeType.Capsule => new Capsule(Size, Orientation),
            ShapeType.Line => new Line(Size.x)
        };
    } 

    public Vector2 Size = Vector2.one;
    public Vector2 RelativePosition = Vector2.zero;
    public float Angle = 0;
    public CapsuleDirection2D Orientation;
    
    public bool MatchRange = true;
    
    public MeleeAttack GetAttack(float damage, Vector3 position)
    {
        return new MeleeAttack(new Damage(damage * damageMultiplier, element, knockback, position), GetShape());
    }
}

[Serializable]
public class Combo
{
    public List<Attack> Attacks;
    public bool FlipXWhenFacingLeft = true;
    public bool ShowGizmos = true;
}

public enum ShapeType
{
    Rectangle,
    Circle,
    Capsule,
    Line
}
