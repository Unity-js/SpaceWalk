using UnityEngine;
using UnityEngine.UI;

public class ButtonParticle : MonoBehaviour
{
    public ParticleSystem particlePrefab;
    public Button button;  
    public AudioSource audioSource;

    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        Vector3 buttonPosition = button.transform.position;

        ParticleSystem newParticle = Instantiate(particlePrefab, buttonPosition, Quaternion.identity);

        var particleRenderer = newParticle.GetComponent<Renderer>();

        if (particleRenderer != null)
        {
            particleRenderer.sortingLayerName = "Particle"; 
            particleRenderer.sortingOrder = 6;  
        }

        newParticle.Play();

        Destroy(newParticle.gameObject, newParticle.main.duration);

        if (audioSource != null)
        {
            audioSource.Play();  // »ç¿îµå
        }
    }
}
