using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    public Button buyButton;  // 버튼
    public int buyCost = 100;  // 가격
    public int increasePerClick = 1;  // 획득 자원 증가량
    private bool isPurchased = false;  // 구매 여부
    private ResourceManager resourceManager;

    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        UpdateButtonState();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    void Update()
    {

        UpdateButtonState();
    }

    // 버튼 변경
    void UpdateButtonState()
    {
        // 자원 충분, 아직 구매 x 
        if (resourceManager.resourceAmount >= buyCost && !isPurchased)
        {
            buyButton.interactable = true;  // 버튼 활성화
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);  // 버튼 불투명
        }
        else
        {
            buyButton.interactable = false;  // 버튼 비활성화
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  // 버튼 반투명
        }
    }

    // 구매 버튼 클릭
    void OnBuyButtonClicked()
    {
        if (!isPurchased && resourceManager.SpendResources(buyCost))  // 자원 충분, 아직 구매 x
        {
            // 자원 획득량 증가
            resourceManager.resourcePerClick += increasePerClick;  // 클릭 시 획득 자원량 증가
            isPurchased = true;  // 완료
            UpdateButtonState();  // 버튼 상태 업데이트
        }
    }
}
