using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;  // ���� �ڿ� �� �ؽ�Ʈ
    public int resourceAmount = 0;  // �ڿ� ��
    public int resourcePerClick = 1;  // �ڿ� ȹ�淮
    public Collider2D clickableArea;  // Ŭ�� ���� ����

    void Start()
    {
        UpdateResourceDisplay();  // �ʱ� �ڿ� ǥ��
    }

    void Update()
    {
        if (Input.touchCount > 0)  // �����
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleClick(touch.position);
            }
        }
        else if (Input.GetMouseButtonDown(0))  // ���콺
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

    // �ڿ� ȹ��
    void CollectResources()
    {
        resourceAmount += resourcePerClick;
        UpdateResourceDisplay();
    }

    // �ڿ� UI ������Ʈ
    void UpdateResourceDisplay()
    {
        resourceText.text = resourceAmount.ToString();
    }
}
