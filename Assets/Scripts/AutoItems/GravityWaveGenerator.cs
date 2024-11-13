using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GravityWaveGenerator : MonoBehaviour
{
    public float firstEffectInterval = 5f;  // 1단계 주기
    public float secondEffectInterval = 4f;  // 2단계 주기
    public float thirdEffectInterval = 3f;  // 3단계 주기

    public float firstEffectMultiplier = 1.5f;  // 1단계 배율
    public float secondEffectMultiplier = 2f;  // 2단계 배율
    public float thirdEffectMultiplier = 3f;  // 3단계 배율

    public int firstUpgradeCost = 3000;
    public int secondUpgradeCost = 6000;
    public int thirdUpgradeCost = 12000;

    private int currentUpgradeLevel = 0;  
    private float timeSinceLastEffect = 0f;  
    private bool isGravityWaveActive = false;  
    private float originalResourcePerClick;  // resourcePerClick 값 저장
    private ResourceManager resourceManager;

    private bool isMultiplying = false;  
    private float effectDuration = 0f;  

    public Button upgradeButton;
    public TextMeshProUGUI costText; 
    public GameObject upgradeCompletedImage;  

    public TextMeshProUGUI gravityWaveStatusText;  

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        upgradeCompletedImage.SetActive(false);
        UpdateButtonState();  // 버튼 초기화
    }

    void Update()
    {
        // 중력 파동 
        if (isGravityWaveActive)
        {
            timeSinceLastEffect += Time.deltaTime;

            if (timeSinceLastEffect >= GetEffectInterval())
            {
                // 배수
                if (!isMultiplying)
                {
                    ApplyGravityWaveEffect();  // 배수 
                    isMultiplying = true;
                    effectDuration = GetEffectDuration();  // 레벨 따른 지속 시간 
                }
                else
                {
                    // 배수 나누기 
                    effectDuration -= Time.deltaTime;
                    if (effectDuration <= 0f)
                    {
                        ResetEffect();  // 복원
                        isMultiplying = false;
                        timeSinceLastEffect = 0f;  // 타이머 
                    }
                }
            }
        }

        if (gravityWaveStatusText != null && isGravityWaveActive)
        {
            gravityWaveStatusText.text = $"현재 업그레이드 :  {GetEffectInterval()} 초마다 클릭 자원량 +{GetEffectMultiplier()}배";
        }

        UpdateButtonState();
    }

    int GetCurrentUpgradeCost()
    {
        if (currentUpgradeLevel == 0) return firstUpgradeCost;
        if (currentUpgradeLevel == 1) return secondUpgradeCost;
        if (currentUpgradeLevel == 2) return thirdUpgradeCost;
        return 0;  
    }

    float GetEffectInterval()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 0f;  
            case 1: return firstEffectInterval;  // 1단계
            case 2: return secondEffectInterval;  // 2단계
            case 3: return thirdEffectInterval;  // 3단계
            default: return thirdEffectInterval;  // 3단계 이후
        }
    }

    float GetEffectDuration()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 0f;  
            case 1: return 5f;  // 1단계
            case 2: return 4f;  // 2단계
            case 3: return 3f;  // 3단계
            default: return 3f;  // 3단계 이후
        }
    }

    float GetEffectMultiplier()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 1f;  
            case 1: return firstEffectMultiplier;  // 1단계
            case 2: return secondEffectMultiplier;  // 2단계
            case 3: return thirdEffectMultiplier;  // 3단계
            default: return thirdEffectMultiplier;  // 3단계 이후
        }
    }

    // 중력 파동 (배수 곱하기)
    void ApplyGravityWaveEffect()
    {
        // resourcePerClick 값 저장
        if (originalResourcePerClick == 0)
        {
            originalResourcePerClick = resourceManager.resourcePerClick;
        }

        // 배수 적용
        resourceManager.resourcePerClick = Mathf.RoundToInt(originalResourcePerClick * GetEffectMultiplier());
    }

    // 효과 복원 (배수 나누기)
    void ResetEffect()
    {
        if (originalResourcePerClick != 0)
        {
            // 배수 나누기
            resourceManager.resourcePerClick = Mathf.RoundToInt(originalResourcePerClick);
        }
    }

    void OnUpgradeButtonClicked()
    {
        int cost = GetCurrentUpgradeCost();
        if (resourceManager.SpendResources(cost) && currentUpgradeLevel < 3)
        {
            currentUpgradeLevel++;  
            isGravityWaveActive = true;  

            UpdateButtonState();
        }
    }

    void UpdateButtonState()
    {
        // 자원 충분, 아직 업그레이드 x, 최대 업그레이드 미달
        if (resourceManager.resourceAmount >= GetCurrentUpgradeCost() && currentUpgradeLevel < 3)
        {
            upgradeButton.interactable = true;  // 버튼 활성화
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);  // 버튼 불투명
        }
        else
        {
            upgradeButton.interactable = false;  // 버튼 비활성화
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  // 버튼 반투명
        }

        // 최대 업그레이드
        if (currentUpgradeLevel >= 3)
        {
            upgradeButton.interactable = false;  // 버튼 비활성화
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);  // 버튼 반투명

            if (costText != null)
            {
                costText.text = "완료!";  
            }

            upgradeCompletedImage.SetActive(true);
        }
        else
        {
            if (costText != null)
            {
                costText.text = FormatResourceAmount(GetCurrentUpgradeCost());
            }
        }
    }

    // 단위 변환 
    private string FormatResourceAmount(int amount)
    {
        if (amount >= 1_000_000_000)
        {
            return (amount / 1_000_000_000f).ToString("0.##") + "B";  // B
        }
        else if (amount >= 1_000_000)
        {
            return (amount / 1_000_000f).ToString("0.##") + "M";  // M
        }
        else if (amount >= 1_000)
        {
            return (amount / 1_000f).ToString("0.##") + "k";  // k
        }
        else
        {
            return amount.ToString();  // 1000 미만
        }
    }
}
