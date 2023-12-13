public class BulletSpeedBuff : StatsManagerEffect
{
    public BulletSpeedBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float bulletSpeedIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, bulletSpeedIncrement, incrementType) { }

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.AdditionalBulletsSpeed = IncrementStat(EffectTarget.Stats.AdditionalBulletsSpeed, EffectTarget.BaseStats.AdditionalBulletsSpeed);
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.AdditionalBulletsSpeed = IncrementStat(EffectTarget.Stats.AdditionalBulletsSpeed, EffectTarget.BaseStats.AdditionalBulletsSpeed);
        }
    }
}