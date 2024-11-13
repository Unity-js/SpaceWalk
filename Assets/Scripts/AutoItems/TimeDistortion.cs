using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeDistortionDevice : MonoBehaviour
{
    public float[] effectDurations = { 10f, 20f, 30f };  // ���� �ð�
    public float[] speedMultipliers = { 1.2f, 1.5f, 2.0f };  // ����
    public float[] cooldownTimes = { 120f, 60f, 30f };  // ��Ÿ��

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
        // �ð� �ְ� ��ġ
        if (isEffectActive)
        {
            effectTimeRemaining -= Time.deltaTime;

            if (effectTimeRemaining <= 0f)
            {
                // ȿ�� ������ ���� �ӵ�
                Time.timeScale = 1f;
                isEffectActive = false;
                cooldownTimeRemaining = cooldownTimes[currentUpgradeLevel - 1];  // ��Ÿ��
            }
        }
        else if (cooldownTimeRemaining > 0f)
        {
            // ��Ÿ�� �� ���� �ӵ� ���� ���·�
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
                statusText.text = "���� �ð����� �ð��� �帧�� �ְ��Ͽ� ���� �ӵ��� ��� ������ŵ�ϴ�."; 
            }
            else if (currentUpgradeLevel == 1)
            {
                statusText.text = $"���� ���׷��̵�: {effectDurations[0]}�� ���� {speedMultipliers[0]}�� ���� \n(��Ÿ�� 2��)";
            }
            else if (currentUpgradeLevel == 2)
            {
                statusText.text = $"���� ���׷��̵�: {effectDurations[1]}�� ���� {speedMultipliers[1]}�� ���� \n(��Ÿ�� 1��)";
            }
            else if (currentUpgradeLevel == 3)
            {
                statusText.text = $"���� ���׷��̵�: {effectDurations[2]}�� ���� {speedMultipliers[2]}�� ���� \n(��Ÿ�� 30��)";
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
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);  // ��ư ���� ����
        }
        else
        {
            upgradeButton.interactable = false;
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  // ��ư ��Ȱ��ȭ
        }

        if (currentUpgradeLevel >= 3)
        {
            upgradeButton.interactable = false;
            upgradeButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);  // ��ư ��Ȱ��ȭ
            if (costText != null)
            {
                costText.text = "�Ϸ�!";  // �Ϸ� �ؽ�Ʈ 
            }
            upgradeCompletedImage.SetActive(true);  // ���׷��̵� �Ϸ� �̹���
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

    // �ڿ� ���� k, M, B ������ ǥ��
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
