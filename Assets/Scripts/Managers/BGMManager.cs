using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    public AudioSource bgmAudioSource;  // BGM�� ����ϴ� AudioSource
    public Slider volumeSlider;         // ���� ������ ���� �����̴�

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);  // �� ��ȯ �ÿ��� �� ������Ʈ�� �ı����� ����
    }

    void Start()
    {
        // ���� �̹� ������ �����̴��� �����Ǿ� ������ �����̴��� ���� �ʱ�ȭ
        if (volumeSlider != null && bgmAudioSource != null)
        {
            volumeSlider.value = bgmAudioSource.volume;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);  // �����̴��� ��ȭ�� ������ ���� ������Ʈ
        }
    }

    // �����̴� ���� �°� BGM ���� ���� (public���� ���� �����ϰ� ����)
    public void UpdateVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
    }

    // �ٸ� ������ BGMManager�� ������ �� �ֵ��� �ϴ� ���� �޼���
    public static BGMManager GetInstance()
    {
        return instance;
    }
}
