using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    public Button buyButton;  // ��ư
    public int buyCost = 100;  // ����
    public int increasePerClick = 1;  // ȹ�� �ڿ� ������
    private bool isPurchased = false;  // ���� ����
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

    // ��ư ����
    void UpdateButtonState()
    {
        // �ڿ� ���, ���� ���� x 
        if (resourceManager.resourceAmount >= buyCost && !isPurchased)
        {
            buyButton.interactable = true;  // ��ư Ȱ��ȭ
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);  // ��ư ������
        }
        else
        {
            buyButton.interactable = false;  // ��ư ��Ȱ��ȭ
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);  // ��ư ������
        }
    }

    // ���� ��ư Ŭ��
    void OnBuyButtonClicked()
    {
        if (!isPurchased && resourceManager.SpendResources(buyCost))  // �ڿ� ���, ���� ���� x
        {
            // �ڿ� ȹ�淮 ����
            resourceManager.resourcePerClick += increasePerClick;  // Ŭ�� �� ȹ�� �ڿ��� ����
            isPurchased = true;  // �Ϸ�
            UpdateButtonState();  // ��ư ���� ������Ʈ
        }
    }
}
