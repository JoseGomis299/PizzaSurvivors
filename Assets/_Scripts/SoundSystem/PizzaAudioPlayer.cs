using UnityEngine;

public class PizzaAudioPlayer : BaseAudioPlayer
{
    [SerializeField] private AudioClip addIngredientSound;
    [SerializeField] private AudioClip removeIngredientSound;
    protected override void SubscribeToEvents()
    {
        Pizza.OnIngredientPlaced += HandleAddIngredientSound;
        Pizza.OnIngredientRemoved += HandleRemoveIngredientSound;
    }

    protected override void UnsubscribeToEvents()
    {
        Pizza.OnIngredientPlaced -= HandleAddIngredientSound;
        Pizza.OnIngredientRemoved -= HandleRemoveIngredientSound;
    }

    private void HandleAddIngredientSound()
    {
        AudioManager.Instance.PlaySound(addIngredientSound);
    }

    private void HandleRemoveIngredientSound()
    {
        AudioManager.Instance.PlaySound(removeIngredientSound);
    }
}