using UnityEngine;
using UnityEngine.UI; 
using TMPro;           

public class ButtonController : MonoBehaviour
{
    public Button button1; 
    public Button button2;  
    public TMP_Text button2Text;  

    void Start()
    {
        button1.onClick.AddListener(OnButton1Click);
    }

    void OnButton1Click()
    {
        button2Text.text = "╥ндо";
    }
}
