using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoClickBot : MonoBehaviour
{
    public float firstAutoClickInterval = 3f;  // 1�ܰ� �ֱ�
    public int firstAutoClickAmount = 1;  // 1�ܰ� Ŭ�� 

    public float secondAutoClickInterval = 2f;  // 2�ܰ� �ֱ�
    public int secondAutoClickAmount = 2;  // 2�ܰ� Ŭ�� 

    public float thirdAutoClickInterval = 1f;  // 3�ܰ� �ֱ�
    public int thirdAutoClickAmount = 3;  // 3�ܰ� Ŭ��

    // ���� ���
    public int firstUpgradeCost = 500;
    public int secondUpgradeCost = 1000;
    public int thirdUpgradeCost = 2000;

    private int currentUpgradeLevel = 0;  
    private float timeSinceLastClick = 0f;  
    private ResourceManager resourceManager;

    public Button upgradeButton;
    public TextMeshProUGUI costText;  
    public GameObject upgradeCompletedImage; 

    public TextMeshProUGUI autoClickStatusText;  

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        upgradeCompletedImage.SetActive(false);
        UpdateButtonState();  
    }

    void Update()
    {
        // �ڵ� Ŭ��
        if (currentUpgradeLevel > 0)
        {
            timeSinceLastClick += Time.deltaTime;

            if (timeSinceLastClick >= GetAutoClickInterval())
            {
                // Ŭ�� �ø��� �ڿ� ȹ���� ����
                for (int i = 0; i < GetAutoClickAmount(); i++)
                {
                    resourceManager.CollectResources();  
                }

                timeSinceLastClick = 0f;  
            }
        }

        if (autoClickStatusText != null && currentUpgradeLevel > 0)
        {
            autoClickStatusText.text = $"���� ���׷��̵� : {GetAutoClickInterval()}�ʸ��� {GetAutoClickAmount()}�� Ŭ��";
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

    // Ŭ�� �ֱ� 
    float GetAutoClickInterval()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 0f;  
            case 1: return firstAutoClickInterval;  // 1�ܰ�
            case 2: return secondAutoClickInterval;  // 2�ܰ�
            case 3: return thirdAutoClickInterval;  // 3�ܰ�
            default: return thirdAutoClickInterval;  // 3�ܰ� ����
        }
    }

    // �ڵ� Ŭ�� Ƚ��
    int GetAutoClickAmount()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 0; 
            case 1: return firstAutoClickAmount;  // 1�ܰ�
            case 2: return secondAutoClickAmount;  // 2�ܰ�
            case 3: return thirdAutoClickAmount;  // 3�ܰ�
            default: return thirdAutoClickAmount;  // 3�ܰ� ����
        }
    }
    void OnUpgradeButtonClicked()
    {
        int cost = GetCurrentUpgradeCost();
        if (resourceManager.SpendResources(cost) && currentUpgradeLevel < 3)
        {
            currentUpgradeLevel++; 

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

            // �Ϸ� 
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
