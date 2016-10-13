using UnityEngine;
using UnityEngine.UI;

public class VideoPlayer : MonoBehaviour
{
    [SerializeField]
    private RawImage _movieRender;

    [SerializeField]
    private Image _previewImage;

    [SerializeField]
    private AudioSource _audioSource;

    private MovieTexture _movie;
    private Sprite _firstFrame;

    public void SetMovie(MovieTexture movie, Sprite firstFrameSprite)
    {
        _firstFrame = firstFrameSprite;
        _movie = movie;
        _previewImage.gameObject.SetActive(true);
        _previewImage.overrideSprite = _firstFrame;
        _movieRender.texture = movie;
        _audioSource.clip = _movie.audioClip;
    }

    [ContextMenu("Play")]
    public void Play()
    {
        _previewImage.gameObject.SetActive(false);
        _movie.Play();
        _audioSource.Play();
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        _previewImage.gameObject.SetActive(true);
        _movie.Stop();
        _audioSource.Stop();
    }
}
