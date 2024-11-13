using UnityEngine;
using TMPro;
public class ResourceManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;  // ���� �ڿ� �� 
    public int resourceAmount = 0;  // �ڿ� ��
    public int resourcePerClick = 1;  // Ŭ�� �� ��� �ڿ���
    public Collider2D clickableArea;  // Ŭ�� ���� (2D �ݶ��̴� ���)

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
    public void AddResources(int amount) // �ڵ� �ڿ� ȹ��
    {
        resourceAmount += amount;
        UpdateResourceDisplay();  // UI ������Ʈ
    }

    // �ڿ� UI ������Ʈ
    private void UpdateResourceDisplay()
    {
        resourceText.text = FormatResourceAmount(resourceAmount);  // �ڿ� ���� ������
    }

    // �ڿ� ���� k, M, B ����
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
