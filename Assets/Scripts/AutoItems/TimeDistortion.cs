using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeDistortionDevice : MonoBehaviour
{
    public float[] effectDurations = { 10f, 20f, 30f };  // 지속 시간
    public float[] speedMultipliers = { 1.2f, 1.5f, 2.0f };  // 배율
    public float[] cooldownTimes = { 120f, 60f, 30f };  // 쿨타임

    public int[] upgradeCosts = { 5000, 10000, 20000 };  
    private int currentUpgradeLevel = 0;  
    private bool isEffectActive = false;  
    private float effectTimeRemaining = 0f;  
    private float cooldownTimeRemaining = 0f;  
    private ResourceManager resourceManager;  

    public Button upgradeButton; 
    public TextMeshProUGUI costText; 
    public GameObject upgradeCompletedImage;  
    public TextMeshProUGUI statusText; 

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        upgradeCompletedImage.SetActive(false);
        UpdateButtonState(); 
    }

    void Update()
    {
        // 시간 왜곡 장치
        if (isEffectActive)
        {
            effectTimeRemaining -= Time.deltaTime;

            if (effectTimeRemaining <= 0f)
            {
                // 효과 끝나면 원래 속도
                Time.timeScale = 1f;
                isEffectActive = false;
                cooldownTimeRemaining = cooldownTimes[currentUpgradeLevel - 1];  // 쿨타임
            }
        }
        else if (cooldownTimeRemaining > 0f)
        {
            // 쿨타임 중 게임 속도 원래 상태로
            Time.timeScale = 1f;
            cooldownTimeRemaining -= Time.deltaTime;
        }

        UpdateStatusText();
        UpdateButtonState();  
    }

    void UpdateStatusText()
    {
        if (statusText != null)
        {
            if (currentUpgradeLevel == 0)
            {
                statusText.text = "일정 시간마다 시간의 흐름을 왜곡하여 게임 속도를 배로 증가시킵니다."; 
            }
            else if (currentUpgradeLevel == 1)
            {
                statusText.text = $"현재 업그레이드: {effectDurations[0]}초 동안 {speedMultipliers[0]}배 증가 \n(쿨타임 2분)";
            }
            else if (currentUpgradeLevel == 2)
            {
                statusText.text = $"현재 업그레이드: {effectDurations[1]}초 동안 {speedMultipliers[1]}배 증가 \n(쿨타임 1분)";
            }
            else if (currentUpgradeLevel == 3)
            {
                statusText.text = $"현재 업그레이드: {effectDurations[2]}초 동안 {speedMultipliers[2]}배 증가 \n(쿨타임 30초)";
            }
        }
    }

    void OnUpgradeButtonClicked()
    {
        int cost = GetCurrentUpgradeCost();
        if (resourceManager.SpendResources(cost) && currentUpgradeLevel < 3)
        {
            currentUpgradeLevel++;  
            isEffectActive = true; 
            effectTimeRemaining = effectDurations[currentUpgradeLevel - 1]; 
            Time.timeScale = speedMultipliers[currentUpgradeLevel - 1];  
            UpdateButtonState();
        }
    }

    void UpdateButtonState()
    {
        if (resourceManager.resourceAmount >= GetCurrentUpgradeCost() && currentUpgradeLevel < 3)
        {
            upgradeButton.interactable = true;
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);  // 버튼 색상 변경
        }
        else
        {
            upgradeButton.interactable = false;
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  // 버튼 비활성화
        }

        if (currentUpgradeLevel >= 3)
        {
            upgradeButton.interactable = false;
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);  // 버튼 비활성화
            if (costText != null)
            {
                costText.text = "완료!";  // 완료 텍스트 
            }
            upgradeCompletedImage.SetActive(true);  // 업그레이드 완료 이미지
        }
        else
        {
            if (costText != null)
            {
                costText.text = FormatResourceAmount(GetCurrentUpgradeCost());
            }
        }
    }
    private int GetCurrentUpgradeCost()
    {
        if (currentUpgradeLevel >= 0 && currentUpgradeLevel < upgradeCosts.Length)
        {
            return upgradeCosts[currentUpgradeLevel];  
        }
        return 0;  
    }

    // 자원 양을 k, M, B 단위로 표시
    private string FormatResourceAmount(int amount)
    {
        if (amount >= 1_000_000_000)
        {
            return (amount / 1_000_000_000f).ToString("0.##") + "B";
        }
        else if (amount >= 1_000_000)
        {
            return (amount / 1_000_000f).ToString("0.##") + "M";
        }
        else if (amount >= 1_000)
        {
            return (amount / 1_000f).ToString("0.##") + "k";
        }
        else
        {
            return amount.ToString();
        }
    }
}
