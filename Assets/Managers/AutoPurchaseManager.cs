using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoPurchaseManager : MonoBehaviour
{
    // �ڵ� �ڿ� ȹ�� ����
    public float firstAutoGainInterval = 10f;  // 1�ܰ� ȹ�� �ֱ�
    public int firstAutoGainAmount = 1;  // 1�ܰ� �ڿ���

    public float secondAutoGainInterval = 8f;  // 2�ܰ� ȹ�� �ֱ�
    public int secondAutoGainAmount = 2;  // 2�ܰ� �ڿ���

    public float thirdAutoGainInterval = 6f;  // 3�ܰ� ȹ�� �ֱ�
    public int thirdAutoGainAmount = 3;  // 3�ܰ� �ڿ���

    // ���� ���
    public int firstBuyCost = 200;  
    public int secondBuyCost = 500;  
    public int thirdBuyCost = 1000;  

    private int currentPurchaseCount = 0;  // ���� ���� Ƚ��
    private bool isPurchased = false;  // ���� ����
    private float timeSinceLastGain = 0f;  // ������ ȹ�� �ð�
    private ResourceManager resourceManager; 

    public Button buyButton;
    public TextMeshProUGUI costText;  // ���� ��� �ؽ�Ʈ
    public GameObject upgradeCompletedImage;  // ���׷��̵� �Ϸ� �̹���

    public TextMeshProUGUI autoGainStatusText;  // "x�ʸ��� �ڿ� +y" �ؽ�Ʈ

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();  
        buyButton.onClick.AddListener(OnBuyButtonClicked);  
        upgradeCompletedImage.SetActive(false); 
        UpdateButtonState();  // ��ư �ʱ�ȭ
    }

    void Update()
    {
        // �ڵ� �ڿ� ȹ��
        if (isPurchased) 
        {
            timeSinceLastGain += Time.deltaTime; 

            if (timeSinceLastGain >= GetAutoGainInterval())  
            {
                resourceManager.AddResources(GetAutoGainAmount());  
                timeSinceLastGain = 0f;  // Ÿ�̸� ����
            }
        }

        // �ڵ� �ڿ� ȹ�� ���� �ؽ�Ʈ
        if (autoGainStatusText != null && isPurchased && currentPurchaseCount > 0)
        {
            // "x�ʸ��� �ڿ� +y" 
            autoGainStatusText.text = $"���� ���׷��̵� : {GetAutoGainInterval()}�ʸ��� �ڿ� +{GetAutoGainAmount()}";
        }

        UpdateButtonState();
    }

    // ���� ���� ���
    int GetCurrentBuyCost()
    {
        if (currentPurchaseCount == 0) return firstBuyCost; 
        if (currentPurchaseCount == 1) return secondBuyCost;  
        if (currentPurchaseCount == 2) return thirdBuyCost;  
        return 0;  // ���� �Ұ�
    }

    // �ڿ� ȹ�� �ֱ� 
    float GetAutoGainInterval()
    {
        switch (currentPurchaseCount)
        {
            case 0: return 0f;  // �ʱ� ����
            case 1: return firstAutoGainInterval;  // 1�ܰ�
            case 2: return secondAutoGainInterval;  // 2�ܰ�
            case 3: return thirdAutoGainInterval;  // 3�ܰ�
            default: return thirdAutoGainInterval;  // 3�ܰ� ����
        }
    }

    // �ڵ� �ڿ� ȹ�淮
    int GetAutoGainAmount()
    {
        switch (currentPurchaseCount)
        {
            case 0: return 0;  // �ʱ� ����
            case 1: return firstAutoGainAmount;  // 1�ܰ�
            case 2: return secondAutoGainAmount;  // 2�ܰ�
            case 3: return thirdAutoGainAmount;  // 3�ܰ�
            default: return thirdAutoGainAmount;  // 3�ܰ� ����
        }
    }

    // ���� ��ư Ŭ��
    void OnBuyButtonClicked()
    {
        int cost = GetCurrentBuyCost();  
        if (resourceManager.SpendResources(cost) && currentPurchaseCount < 3)
        {
            currentPurchaseCount++;  // ���� Ƚ��

            // ù ��° ���� ����
            if (currentPurchaseCount > 0)
            {
                isPurchased = true;  
            }

            if (currentPurchaseCount == 3)
            {
                
                isPurchased = true;
            }

            UpdateButtonState();
        }
    }

    // ���� ��ư ����
    void UpdateButtonState()
    {
        // �ڿ� ���, ���� ���� x, �ִ� ���� Ƚ�� �̴�
        if (resourceManager.resourceAmount >= GetCurrentBuyCost() && currentPurchaseCount < 3)
        {
            buyButton.interactable = true;  // ��ư Ȱ��ȭ
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);  // ��ư ������
        }
        else
        {
            buyButton.interactable = false;  // ��ư ��Ȱ��ȭ
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  // ��ư ������
        }

        // �ִ� ���� Ƚ�� ����
        if (currentPurchaseCount >= 3)
        {
            buyButton.interactable = false;  // ��ư ��Ȱ��ȭ
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);  // ��ư ������

            // �Ϸ� ǥ��
            if (costText != null)
            {
                costText.text = "�Ϸ�!";  // �ؽ�Ʈ
            }

            // ���׷��̵� �Ϸ� �̹���
            upgradeCompletedImage.SetActive(true);
        }
        else
        {
            // ���� ���� �ܰ� ��� ǥ��
            if (costText != null)
            {
                costText.text = FormatResourceAmount(GetCurrentBuyCost());
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
