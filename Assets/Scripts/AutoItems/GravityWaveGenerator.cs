using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GravityWaveGenerator : MonoBehaviour
{
    public float firstEffectInterval = 5f;  // 1�ܰ� �ֱ�
    public float secondEffectInterval = 4f;  // 2�ܰ� �ֱ�
    public float thirdEffectInterval = 3f;  // 3�ܰ� �ֱ�

    public float firstEffectMultiplier = 1.5f;  // 1�ܰ� ����
    public float secondEffectMultiplier = 2f;  // 2�ܰ� ����
    public float thirdEffectMultiplier = 3f;  // 3�ܰ� ����

    public int firstUpgradeCost = 3000;
    public int secondUpgradeCost = 6000;
    public int thirdUpgradeCost = 12000;

    private int currentUpgradeLevel = 0;  
    private float timeSinceLastEffect = 0f;  
    private bool isGravityWaveActive = false;  
    private float originalResourcePerClick;  // resourcePerClick �� ����
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
        UpdateButtonState();  // ��ư �ʱ�ȭ
    }

    void Update()
    {
        // �߷� �ĵ� 
        if (isGravityWaveActive)
        {
            timeSinceLastEffect += Time.deltaTime;

            if (timeSinceLastEffect >= GetEffectInterval())
            {
                // ���
                if (!isMultiplying)
                {
                    ApplyGravityWaveEffect();  // ��� 
                    isMultiplying = true;
                    effectDuration = GetEffectDuration();  // ���� ���� ���� �ð� 
                }
                else
                {
                    // ��� ������ 
                    effectDuration -= Time.deltaTime;
                    if (effectDuration <= 0f)
                    {
                        ResetEffect();  // ����
                        isMultiplying = false;
                        timeSinceLastEffect = 0f;  // Ÿ�̸� 
                    }
                }
            }
        }

        if (gravityWaveStatusText != null && isGravityWaveActive)
        {
            gravityWaveStatusText.text = $"���� ���׷��̵� :  {GetEffectInterval()} �ʸ��� Ŭ�� �ڿ��� +{GetEffectMultiplier()}��";
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
            case 1: return firstEffectInterval;  // 1�ܰ�
            case 2: return secondEffectInterval;  // 2�ܰ�
            case 3: return thirdEffectInterval;  // 3�ܰ�
            default: return thirdEffectInterval;  // 3�ܰ� ����
        }
    }

    float GetEffectDuration()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 0f;  
            case 1: return 5f;  // 1�ܰ�
            case 2: return 4f;  // 2�ܰ�
            case 3: return 3f;  // 3�ܰ�
            default: return 3f;  // 3�ܰ� ����
        }
    }

    float GetEffectMultiplier()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 1f;  
            case 1: return firstEffectMultiplier;  // 1�ܰ�
            case 2: return secondEffectMultiplier;  // 2�ܰ�
            case 3: return thirdEffectMultiplier;  // 3�ܰ�
            default: return thirdEffectMultiplier;  // 3�ܰ� ����
        }
    }

    // �߷� �ĵ� (��� ���ϱ�)
    void ApplyGravityWaveEffect()
    {
        // resourcePerClick �� ����
        if (originalResourcePerClick == 0)
        {
            originalResourcePerClick = resourceManager.resourcePerClick;
        }

        // ��� ����
        resourceManager.resourcePerClick = Mathf.RoundToInt(originalResourcePerClick * GetEffectMultiplier());
    }

    // ȿ�� ���� (��� ������)
    void ResetEffect()
    {
        if (originalResourcePerClick != 0)
        {
            // ��� ������
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
        // �ڿ� ���, ���� ���׷��̵� x, �ִ� ���׷��̵� �̴�
        if (resourceManager.resourceAmount >= GetCurrentUpgradeCost() && currentUpgradeLevel < 3)
        {
            upgradeButton.interactable = true;  // ��ư Ȱ��ȭ
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);  // ��ư ������
        }
        else
        {
            upgradeButton.interactable = false;  // ��ư ��Ȱ��ȭ
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  // ��ư ������
        }

        // �ִ� ���׷��̵�
        if (currentUpgradeLevel >= 3)
        {
            upgradeButton.interactable = false;  // ��ư ��Ȱ��ȭ
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);  // ��ư ������

            if (costText != null)
            {
                costText.text = "�Ϸ�!";  
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

    // ���� ��ȯ 
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
            return amount.ToString();  // 1000 �̸�
        }
    }
}
