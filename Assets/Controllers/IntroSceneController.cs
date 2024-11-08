using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class IntroSceneController : MonoBehaviour
{
    public TextMeshProUGUI creditText;  
    public TextMeshProUGUI titleText;
    public Button button;
    public Image image;

    public float scrollSpeed = 1f;     
    public string sceneToLoad = "MainScene"; 
    
    public void StartCredits() // ��ư Ŭ�� �� ȣ��
    {
        button.gameObject.SetActive(false);  
        titleText.gameObject.SetActive(false);
        image.gameObject.SetActive(false);

        StartCoroutine(AnimateCredits());   
    }

    private IEnumerator AnimateCredits()
    {
        creditText.gameObject.SetActive(true); // �ؽ�Ʈ Ȱ��ȭ

        float originalY = creditText.transform.position.y; 
        float targetY = originalY + 20f; // ���� ��ġ 

        while (creditText.transform.position.y < targetY)
        {
            creditText.transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
            yield return null; 
        }

        //�� ��ȯ
        StartCoroutine(LoadNextScene());
    }
    private IEnumerator LoadNextScene() // �ڷ�ƾ
    {
        yield return new WaitForSeconds(0f); 

        SceneManager.LoadScene(sceneToLoad);
    }
}
