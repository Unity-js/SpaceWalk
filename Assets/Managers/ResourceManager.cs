using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;  // 현재 자원 양 텍스트
    public int resourceAmount = 0;  // 자원 양
    public int resourcePerClick = 1;  // 자원 획득량
    public Collider2D clickableArea;  // 클릭 가능 영역

    void Start()
    {
        UpdateResourceDisplay();  // 초기 자원 표시
    }

    void Update()
    {
        if (Input.touchCount > 0)  // 모바일
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
            CollectResources();
        }
    }

    // 자원 획득
    void CollectResources()
    {
        resourceAmount += resourcePerClick;
        UpdateResourceDisplay();
    }

    // 자원 UI 업데이트
    void UpdateResourceDisplay()
    {
        resourceText.text = resourceAmount.ToString();
    }
}
