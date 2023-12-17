public class HealthBuff : StatsManagerEffect
{
    private HealthComponent _healthComponent;
    public HealthBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float healthIncrement,
        IncrementType incrementType) : base(target, duration, maxStacks, timerType, healthIncrement, incrementType)
    {
        _healthComponent = target.GetComponent<HealthComponent>();
    }

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        _healthComponent.SetMaxHealth(IncrementStat(EffectTarget.Stats.MaxHealth, EffectTarget.BaseStats.MaxHealth));
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            _healthComponent.SetMaxHealth(IncrementStat(EffectTarget.Stats.MaxHealth, EffectTarget.BaseStats.MaxHealth));
        }
    }
}

