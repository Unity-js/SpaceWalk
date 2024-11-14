using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;  // MainScene에서 볼륨을 조절할 슬라이더

    void Start()
    {
        // BGMManager의 인스턴스를 가져옴
        BGMManager bgmManager = BGMManager.GetInstance();

        if (bgmManager != null && bgmManager.bgmAudioSource != null)
        {
            // 슬라이더 초기값 설정 (BGM의 현재 볼륨 값)
            volumeSlider.value = bgmManager.bgmAudioSource.volume;
            volumeSlider.onValueChanged.AddListener(bgmManager.UpdateVolume);  // 볼륨이 변경되면 UpdateVolume 호출
        }
        else
        {
            Debug.LogError("BGMManager or AudioSource not found!");
        }
    }
}
