using TMPro;
using UnityEngine;

public class UIManager_Game : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text indicatorText;

    private void Start()
    {
        SpawningSystem.OnRoundStart += UpdateRoundText;
        SpawningSystem.OnRoundEnd += HideText;
        SpawningSystem.OnTimerChanged += UpdateTimerText;
        CurrencyManager.OnMoneyChanged += UpdateMoneyText;
    }

    private void UpdateRoundText(int round)
    {
        roundText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        indicatorText.gameObject.SetActive(false);
        
        roundText.text = "ROUND " + round;
    }
    
    private void UpdateTimerText(float time)
    {
        timerText.text = time.ToString("F2");
    }
    
    private void UpdateMoneyText(int money)
    {
        moneyText.text = "$ "+money;
    }
    
    private void HideText(int round)
    {
        roundText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        indicatorText.gameObject.SetActive(true);
    }
}
