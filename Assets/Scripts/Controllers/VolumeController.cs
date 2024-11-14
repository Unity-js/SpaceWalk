using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;  // MainScene���� ������ ������ �����̴�

    void Start()
    {
        // BGMManager�� �ν��Ͻ��� ������
        BGMManager bgmManager = BGMManager.GetInstance();

        if (bgmManager != null && bgmManager.bgmAudioSource != null)
        {
            // �����̴� �ʱⰪ ���� (BGM�� ���� ���� ��)
            volumeSlider.value = bgmManager.bgmAudioSource.volume;
            volumeSlider.onValueChanged.AddListener(bgmManager.UpdateVolume);  // ������ ����Ǹ� UpdateVolume ȣ��
        }
        else
        {
            Debug.LogError("BGMManager or AudioSource not found!");
        }
    }
}
