using UnityEngine;
using TMPro;
public class ResourceManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;  // 현재 자원 양 
    public int resourceAmount = 0;  // 자원 양
    public int resourcePerClick = 1;  // 클릭 시 얻는 자원량
    public Collider2D clickableArea;  // 클릭 영역 (2D 콜라이더 사용)

    void Start()
    {
        UpdateResourceDisplay();  // 초기 자원
    }

    void Update()
    {
        if (Input.touchCount > 0)  // 터치
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleClick(touch.position);
            }
        }
        else if (Input.GetMouseButtonDown(0))  // 마우스
        {
            HandleClick(Input.mousePosition);
        }
    }

    void HandleClick(Vector2 inputPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);

        if (clickableArea.bounds.Contains(worldPosition))
        {
            CollectResources();  // 자원 수집
        }
    }

    // 자원 획득
    public void CollectResources()
    {
        resourceAmount += resourcePerClick;
        UpdateResourceDisplay();  // UI 업데이트
    }

    // 자원 소모
    public bool SpendResources(int cost)
    {
        if (resourceAmount >= cost)
        {
            resourceAmount -= cost;  // 자원 소모
            UpdateResourceDisplay();  // UI 업데이트
            return true;  // 자원 소모 
        }
        return false;  // 자원 부족
    }
    public void AddResources(int amount) // 자동 자원 획득
    {
        resourceAmount += amount;
        UpdateResourceDisplay();  // UI 업데이트
    }

    // 자원 UI 업데이트
    private void UpdateResourceDisplay()
    {
        resourceText.text = FormatResourceAmount(resourceAmount);  // 자원 양을 단위별
    }

    // 자원 양을 k, M, B 단위
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
