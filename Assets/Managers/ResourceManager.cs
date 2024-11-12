using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;  // ���� �ڿ� �� 
    public int resourceAmount = 0;  // �ڿ� ��
    public int resourcePerClick = 1;  // Ŭ�� �� ��� �ڿ���
    public Collider2D clickableArea;  // Ŭ�� ���� (2d �ݶ��̴� ���)

    void Start()
    {
        UpdateResourceDisplay();  // �ʱ� �ڿ�
    }

    void Update()
    {
        if (Input.touchCount > 0)  // ��ġ
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
            CollectResources();  // �ڿ� ����
        }
    }

    // �ڿ� ȹ��
    public void CollectResources()
    {
        resourceAmount += resourcePerClick;
        UpdateResourceDisplay();  // UI ������Ʈ
    }

    // �ڿ� �Ҹ�
    public bool SpendResources(int cost)
    {
        if (resourceAmount >= cost)
        {
            resourceAmount -= cost;  // �ڿ� �Ҹ�
            UpdateResourceDisplay();  // UI ������Ʈ
            return true;  // �ڿ� �Ҹ� 
        }
        return false;  // �ڿ� ����
    }

    // �ڿ� UI ������Ʈ
    private void UpdateResourceDisplay()
    {
        resourceText.text = resourceAmount.ToString();  // �ڿ� UI �ؽ�Ʈ ����
    }
}
