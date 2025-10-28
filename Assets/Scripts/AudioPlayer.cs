using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip background;
    public AudioClip throwsound;
    public AudioClip destroysound;

    public static AudioPlayer player;

    private void Awake()
    {
        if (player == null)
        {
            player = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = background;
        audioSource.loop = true;
        audioSource.volume = 0.8f;
        audioSource.Play();

    }

    public void OnThrowSound()
    {
        audioSource.PlayOneShot(throwsound,0.6f);
    }

    public void OnDestroySound()
    {
        audioSource.PlayOneShot(destroysound,0.2f);
    }

}
