using UnityEngine;
using UnityEngine.UI;

public class VideoPlayer : MonoBehaviour
{
    [SerializeField]
    private Image _movieRender;

    [SerializeField]
    private AudioSource _audioSource;

    private MovieTexture _movie;
    private Sprite _firstFrame;

    public void SetMovie(MovieTexture movie, Sprite firstFrameSprite)
    {
        _firstFrame = firstFrameSprite;
        _movie = movie;
        _movieRender.material.mainTexture = movie;
        _audioSource.clip = _movie.audioClip;
    }

    [ContextMenu("Play")]
    public void Play()
    {
        _movieRender.sprite = null;
        _movie.Play();
        _audioSource.Play();
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        _movieRender.sprite = _firstFrame;
        _movie.Stop();
        _audioSource.Stop();
    }
}
