using System;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class RectangleCombosContainer : MonoBehaviour
{
    [Header("Combos (Must be aligned upwards)")]
    [SerializeField] private List<RectangleCombo> rectangleCombos;
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
        foreach (RectangleCombo rectangleCombo in rectangleCombos)
        {
            foreach (var attack in rectangleCombo.Attacks)
            {
                if (attack.MatchRange)
                {
                    attack.Size = new Vector2(attack.Size.x, attackRange-0.5f);
                    attack.RelativePosition = new Vector2(0,  (attackRange-0.5f)/2 + 0.5f);
                }
            }
        }
    }

    public void Attack(int combo, Vector2 direction, LayerMask layerMask)
    {
        _lastDirection = direction;
        float rotation = -Vector2.SignedAngle(_lastDirection, Vector2.up);

        foreach (RectangleAttack rectangleAttack in rectangleCombos[combo].Attacks)
        {
            if ((rectangleCombos[combo].FlipXWhenFacingLeft && direction.x >= 0 && !_flipped) || (_flipped && direction.x < 0))
            {
                rectangleAttack.RelativePosition = new Vector2(-rectangleAttack.RelativePosition.x, rectangleAttack.RelativePosition.y);
            }

            rectangleAttack.GetAttack(_statsManager.Stats.Attack, transform.position).Attack((Vector2) transform.position + rectangleAttack.RelativePosition.Rotate(rotation), rectangleAttack.Angle + rotation, layerMask);
        }
        
        _flipped = direction.x >= 0;
    }
    
    public void AttackAll(int combo, Vector2 direction, LayerMask layerMask)
    {
        _lastDirection = direction;
        float rotation = -Vector2.SignedAngle(_lastDirection, Vector2.up);

        foreach (RectangleAttack rectangleAttack in rectangleCombos[combo].Attacks)
        {
            if ((rectangleCombos[combo].FlipXWhenFacingLeft && direction.x >= 0 && !_flipped) || (_flipped && direction.x < 0))
            {
                rectangleAttack.RelativePosition = new Vector2(-rectangleAttack.RelativePosition.x, rectangleAttack.RelativePosition.y);
            }

            rectangleAttack.GetAttack(_statsManager.Stats.Attack, transform.position).AttackAll((Vector2) transform.position + rectangleAttack.RelativePosition.Rotate(rotation), rectangleAttack.Angle + rotation, layerMask);
        }
        
        _flipped = direction.x >= 0;
    }

    private void OnDrawGizmos()
    {
        for (var i = 0; i < rectangleCombos.Count; i++)
        {
            if (!rectangleCombos[i].ShowGizmos) continue;
            
            float rotation = -Vector2.SignedAngle(_lastDirection, Vector2.up);
            
            foreach (RectangleAttack rectangleAttack in rectangleCombos[i].Attacks)
            {
                rectangleAttack.Shape.DrawGizmos((Vector2) transform.position + rectangleAttack.RelativePosition.Rotate(rotation), rectangleAttack.Angle + rotation, _colors[i]);
            }
        }

        Debug.Log(_lastDirection);
    }

}

[Serializable]
public class RectangleAttack
{
    [SerializeField] private float damageMultiplier = 1;
    [SerializeField] private Element element = Element.None;
    [SerializeField] private float knockback = 0;
    
    public Rectangle Shape => new Rectangle(Size);
    public Vector2 Size = Vector2.one;
    public Vector2 RelativePosition = Vector2.zero;
    public float Angle = 0; 
    
    public bool MatchRange = true;
    
    public MeleeAttack GetAttack(float damage, Vector3 position)
    {
        return new MeleeAttack(new Damage(damage * damageMultiplier, element, knockback, position), Shape);
    }
}

[Serializable]
public class RectangleCombo
{
    public List<RectangleAttack> Attacks;
    public bool FlipXWhenFacingLeft = true;
    public bool ShowGizmos = true;
    public bool MatchRange = true;
}