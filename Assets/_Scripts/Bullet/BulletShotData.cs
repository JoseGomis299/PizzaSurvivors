using System.Collections.Generic;
using UnityEngine;

public struct BulletShotData
{
    public Vector2 StartPosition;
    public readonly float StartPositionDistance;

    public Vector2 Direction;
    public List<BulletModifierInfo> Modifiers;

    public BulletShotData(Vector2 startPosition, float startPositionDistanceDistance, Vector2 direction, List<BulletModifierInfo> modifiers)
    {
        StartPosition = startPosition;
        Direction = direction;
        Modifiers = modifiers;
        StartPositionDistance = startPositionDistanceDistance;
    }
    
    public void RotateStartPositionTo(Vector2 direction)
    {
        Vector2 originalPos = StartPosition - Direction*StartPositionDistance;
        StartPosition = StartPositionDistance*direction + originalPos;
        Direction = direction;
        
        this = new BulletShotData(StartPosition, StartPositionDistance, Direction, Modifiers);
    }
}