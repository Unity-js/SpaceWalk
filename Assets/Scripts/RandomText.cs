using UnityEngine;
using TMPro;  
using System.Collections;

public class RandomText : MonoBehaviour
{
    public string[] messages;

    public TextMeshProUGUI messageText;

    public float messageDisplayTime = 2f; // �޽��� ��� �� ������� �ð�

    private void Start()
    {
        messageText.gameObject.SetActive(false);

        StartCoroutine(DisplayRandomMessage());
    }

    IEnumerator DisplayRandomMessage()
    {
        while (true)
        {
            string randomMessage = messages[Random.Range(0, messages.Length)];

            messageText.text = randomMessage;
            messageText.gameObject.SetActive(true);

            yield return new WaitForSeconds(messageDisplayTime);

            messageText.gameObject.SetActive(false);

            yield return new WaitForSeconds(1f);
        }
    }
}
