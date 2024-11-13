using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoPurchaseManager : MonoBehaviour
{
    // 자동 자원 획득 변수
    public float firstAutoGainInterval = 10f;  // 1단계 획득 주기
    public int firstAutoGainAmount = 1;  // 1단계 자원량

    public float secondAutoGainInterval = 8f;  // 2단계 획득 주기
    public int secondAutoGainAmount = 2;  // 2단계 자원량

    public float thirdAutoGainInterval = 6f;  // 3단계 획득 주기
    public int thirdAutoGainAmount = 3;  // 3단계 자원량

    // 구매 비용
    public int firstBuyCost = 200;  
    public int secondBuyCost = 500;  
    public int thirdBuyCost = 1000;  

    private int currentPurchaseCount = 0;  // 현재 구매 횟수
    private bool isPurchased = false;  // 구매 여부
    private float timeSinceLastGain = 0f;  // 마지막 획득 시간
    private ResourceManager resourceManager; 

    public Button buyButton;
    public TextMeshProUGUI costText;  // 구매 비용 텍스트
    public GameObject upgradeCompletedImage;  // 업그레이드 완료 이미지

    public TextMeshProUGUI autoGainStatusText;  // "x초마다 자원 +y" 텍스트

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();  
        buyButton.onClick.AddListener(OnBuyButtonClicked);  
        upgradeCompletedImage.SetActive(false); 
        UpdateButtonState();  // 버튼 초기화
    }

    void Update()
    {
        // 자동 자원 획득
        if (isPurchased) 
        {
            timeSinceLastGain += Time.deltaTime; 

            if (timeSinceLastGain >= GetAutoGainInterval())  
            {
                resourceManager.AddResources(GetAutoGainAmount());  
                timeSinceLastGain = 0f;  // 타이머 리셋
            }
        }

        // 자동 자원 획득 상태 텍스트
        if (autoGainStatusText != null && isPurchased && currentPurchaseCount > 0)
        {
            // "x초마다 자원 +y" 
            autoGainStatusText.text = $"현재 업그레이드 : {GetAutoGainInterval()}초마다 자원 +{GetAutoGainAmount()}";
        }

        UpdateButtonState();
    }

    // 현재 구매 비용
    int GetCurrentBuyCost()
    {
        if (currentPurchaseCount == 0) return firstBuyCost; 
        if (currentPurchaseCount == 1) return secondBuyCost;  
        if (currentPurchaseCount == 2) return thirdBuyCost;  
        return 0;  // 구매 불가
    }

    // 자원 획득 주기 
    float GetAutoGainInterval()
    {
        switch (currentPurchaseCount)
        {
            case 0: return 0f;  // 초기 상태
            case 1: return firstAutoGainInterval;  // 1단계
            case 2: return secondAutoGainInterval;  // 2단계
            case 3: return thirdAutoGainInterval;  // 3단계
            default: return thirdAutoGainInterval;  // 3단계 이후
        }
    }

    // 자동 자원 획득량
    int GetAutoGainAmount()
    {
        switch (currentPurchaseCount)
        {
            case 0: return 0;  // 초기 상태
            case 1: return firstAutoGainAmount;  // 1단계
            case 2: return secondAutoGainAmount;  // 2단계
            case 3: return thirdAutoGainAmount;  // 3단계
            default: return thirdAutoGainAmount;  // 3단계 이후
        }
    }

    // 구매 버튼 클릭
    void OnBuyButtonClicked()
    {
        int cost = GetCurrentBuyCost();  
        if (resourceManager.SpendResources(cost) && currentPurchaseCount < 3)
        {
            currentPurchaseCount++;  // 구매 횟수

            // 첫 번째 구매 이후
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

    // 구매 버튼 상태
    void UpdateButtonState()
    {
        // 자원 충분, 아직 구매 x, 최대 구매 횟수 미달
        if (resourceManager.resourceAmount >= GetCurrentBuyCost() && currentPurchaseCount < 3)
        {
            buyButton.interactable = true;  // 버튼 활성화
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);  // 버튼 불투명
        }
        else
        {
            buyButton.interactable = false;  // 버튼 비활성화
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  // 버튼 반투명
        }

        // 최대 구매 횟수 도달
        if (currentPurchaseCount >= 3)
        {
            buyButton.interactable = false;  // 버튼 비활성화
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);  // 버튼 반투명

            // 완료 표시
            if (costText != null)
            {
                costText.text = "완료!";  // 텍스트
            }

            // 업그레이드 완료 이미지
            upgradeCompletedImage.SetActive(true);
        }
        else
        {
            // 구매 가능 단계 비용 표시
            if (costText != null)
            {
                costText.text = FormatResourceAmount(GetCurrentBuyCost());
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
