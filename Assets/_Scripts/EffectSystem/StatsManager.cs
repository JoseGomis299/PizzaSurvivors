using UnityEngine;

public class StatsManager : MonoBehaviour, IEffectTarget
{
    [SerializeField] private Stats stats;
    private Stats _stats;

    public Stats BaseStats => stats;
    public Stats Stats => _stats;
    
    private EffectManager _effectManager;

    private void Awake() {
        _stats = ScriptableObject.CreateInstance<Stats>();
        _stats.SetValues(stats);
        _effectManager = new EffectManager(this);
    }
    
    public void ApplyEffect(BaseEffect effect) {
        _effectManager.ApplyEffect(effect);
    }
    
    public void ReApplyEffects() {
        _effectManager.ReApplyEffects();
    }
}