using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoControllerFINAL : MonoBehaviour
{
    public VideoPlayer video;
    public Slider volumeSlider;
    public Slider progressSlider;
    public int nextSceneIndex;

    bool isDragging = false;
    public double skipSeconds = 10;

    void Start()
    {
        video.prepareCompleted += OnPrepared;
        video.loopPointReached += OnEnd;
        video.Prepare();

        volumeSlider.value = 1f;
        progressSlider.minValue = 0f;
        progressSlider.maxValue = 1f;
    }

    void OnPrepared(VideoPlayer vp)
    {
        video.Play();
    }

    void Update()
    {
        // update slider doar dacă NU tragi de el
        if (video.isPlaying && !isDragging && video.length > 0)
        {
            progressSlider.value = (float)(video.time / video.length);
        }

        // volum
        video.SetDirectAudioVolume(0, volumeSlider.value);
    }

    void OnEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneIndex);
    }

    // ===== UI =====

    public void Skip()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }

    // când începi să tragi sliderul
    public void StartDrag()
    {
        isDragging = true;
    }

    // când îi dai drumul
    public void EndDrag()
    {
        isDragging = false;
        video.time = progressSlider.value * video.length;
    }

    // +10 sec
    public void Plus10()
    {
        video.time = Mathf.Min((float)video.length, (float)(video.time + skipSeconds));
    }

    // -10 sec
    public void Minus10()
    {
        video.time = Mathf.Max(0f, (float)(video.time - skipSeconds));
    }
}
