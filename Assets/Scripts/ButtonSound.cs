using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public Button button;
    public AudioSource audioSource;

    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (audioSource != null)
        {
            audioSource.Play();  // »ç¿îµå
        }
    }
}
