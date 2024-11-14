using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; 

public class AutoClickBot : MonoBehaviour
{
    public float firstAutoClickInterval = 3f;  // 1단계 주기
    public int firstAutoClickAmount = 1;  // 1단계 클릭 

    public float secondAutoClickInterval = 2f;  // 2단계 주기
    public int secondAutoClickAmount = 2;  // 2단계 클릭 

    public float thirdAutoClickInterval = 1f;  // 3단계 주기
    public int thirdAutoClickAmount = 3;  // 3단계 클릭

    // 구매 비용
    public int firstUpgradeCost = 500;
    public int secondUpgradeCost = 1000;
    public int thirdUpgradeCost = 2000;

    private int currentUpgradeLevel = 0;
    private ResourceManager resourceManager;

    public Button upgradeButton;
    public TextMeshProUGUI costText;
    public GameObject upgradeCompletedImage;

    public TextMeshProUGUI autoClickStatusText;

    private Coroutine autoClickCoroutine; 

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        upgradeCompletedImage.SetActive(false);
        UpdateButtonState();

        StartAutoClickCoroutine();
    }

    void Update()
    {
        if (autoClickStatusText != null && currentUpgradeLevel > 0)
        {
            autoClickStatusText.text = $"현재 업그레이드 : {GetAutoClickInterval()}초마다 {GetAutoClickAmount()}번 클릭";
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

    // 클릭 주기 
    float GetAutoClickInterval()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 0f;
            case 1: return firstAutoClickInterval;  // 1단계
            case 2: return secondAutoClickInterval;  // 2단계
            case 3: return thirdAutoClickInterval;  // 3단계
            default: return thirdAutoClickInterval;  // 3단계 이후
        }
    }

    // 자동 클릭 횟수
    int GetAutoClickAmount()
    {
        switch (currentUpgradeLevel)
        {
            case 0: return 0;
            case 1: return firstAutoClickAmount;  // 1단계
            case 2: return secondAutoClickAmount;  // 2단계
            case 3: return thirdAutoClickAmount;  // 3단계
            default: return thirdAutoClickAmount;  // 3단계 이후
        }
    }

    void OnUpgradeButtonClicked()
    {
        int cost = GetCurrentUpgradeCost();
        if (resourceManager.SpendResources(cost) && currentUpgradeLevel < 3)
        {
            currentUpgradeLevel++;

            RestartAutoClickCoroutine(); // 업그레이드 후에 재시작!!

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

            // 완료 
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

    // 자동 클릭 코루틴
    private IEnumerator AutoClickCoroutine()
    {
        while (currentUpgradeLevel > 0)
        {
            for (int i = 0; i < GetAutoClickAmount(); i++)
            {
                resourceManager.CollectResources();
            }

            yield return new WaitForSeconds(GetAutoClickInterval());
        }
    }

    // 자동 클릭 코루틴 시작
    private void StartAutoClickCoroutine()
    {
        if (autoClickCoroutine != null)
        {
            StopCoroutine(autoClickCoroutine);  // 기존 코루틴 존재? > 정지
        }

        autoClickCoroutine = StartCoroutine(AutoClickCoroutine());
    }

    // 업그레이드 > 코루틴 재시작
    private void RestartAutoClickCoroutine()
    {
        if (autoClickCoroutine != null)
        {
            StopCoroutine(autoClickCoroutine);  // 기존 코루틴 정지
        }

        StartAutoClickCoroutine();  // 시작
    }
}
