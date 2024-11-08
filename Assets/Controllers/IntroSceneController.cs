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
    
    public void StartCredits() // 버튼 클릭 시 호출
    {
        button.gameObject.SetActive(false);  
        titleText.gameObject.SetActive(false);
        image.gameObject.SetActive(false);

        StartCoroutine(AnimateCredits());   
    }

    private IEnumerator AnimateCredits()
    {
        creditText.gameObject.SetActive(true); // 텍스트 활성화

        float originalY = creditText.transform.position.y; 
        float targetY = originalY + 20f; // 최종 위치 

        while (creditText.transform.position.y < targetY)
        {
            creditText.transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
            yield return null; 
        }

        //씬 전환
        StartCoroutine(LoadNextScene());
    }
    private IEnumerator LoadNextScene() // 코루틴
    {
        yield return new WaitForSeconds(0f); 

        SceneManager.LoadScene(sceneToLoad);
    }
}
