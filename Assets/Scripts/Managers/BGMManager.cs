using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    public AudioSource bgmAudioSource;  // BGM을 담당하는 AudioSource
    public Slider volumeSlider;         // 볼륨 조절을 위한 슬라이더

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 이 오브젝트는 파괴되지 않음
    }

    void Start()
    {
        // 만약 이미 씬에서 슬라이더가 설정되어 있으면 슬라이더의 값도 초기화
        if (volumeSlider != null && bgmAudioSource != null)
        {
            volumeSlider.value = bgmAudioSource.volume;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);  // 슬라이더가 변화할 때마다 볼륨 업데이트
        }
    }

    // 슬라이더 값에 맞게 BGM 볼륨 조정 (public으로 접근 가능하게 수정)
    public void UpdateVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
    }

    // 다른 씬에서 BGMManager에 접근할 수 있도록 하는 정적 메서드
    public static BGMManager GetInstance()
    {
        return instance;
    }
}
