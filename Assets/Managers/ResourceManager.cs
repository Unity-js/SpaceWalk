using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;  // 현재 자원 양 
    public int resourceAmount = 0;  // 자원 양
    public int resourcePerClick = 1;  // 클릭 시 얻는 자원량
    public Collider2D clickableArea;  // 클릭 영역 (2d 콜라이더 사용)

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

    // 자원 UI 업데이트
    private void UpdateResourceDisplay()
    {
        resourceText.text = resourceAmount.ToString();  // 자원 UI 텍스트 갱신
    }
}
