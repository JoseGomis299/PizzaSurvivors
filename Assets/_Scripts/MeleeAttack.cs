public readonly struct MeleeAttack
{
    public readonly Damage Damage;
    public readonly IShape Shape;
    
    public MeleeAttack(Damage damage, IShape shape)
    {
        Damage = damage;
        Shape = shape;
    }
}