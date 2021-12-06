using UnityEngine;
using UnityEngine.Video;

public class DebugController : MonoBehaviour
{
    [Header("References")]
    public GameObject DebugGui;
    public AudioSource MainAudioSource;
    public VideoPlayer MainVideoPlayer;
    public GameObject PlayerMock;
    public GameObject[] Scenes;

    [Header("Assets")]
    public AudioClip[] AudioClips;
    public VideoClip[] VideoClips;

    private int _audioClip = 0;
    private int _videoClip = 0;
    private int _scene = 0;

    private void Start()
    {
        DebugGUI.LogPersistent("audio", "Audio: <none>");
        DebugGUI.LogPersistent("video", "Video: <none>");
        DebugGUI.LogPersistent("scene", "Scene: " + Scenes[_scene].name);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DebugGui.SetActive(!DebugGui.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _audioClip = (_audioClip + 1) % (AudioClips.Length + 1);
            MainAudioSource.Stop();
            if (_audioClip < AudioClips.Length)
            {
                MainAudioSource.PlayOneShot(AudioClips[_audioClip]);
                DebugGUI.LogPersistent("audio", "Audio: " + AudioClips[_audioClip].name);
            } else
            {
                DebugGUI.LogPersistent("audio", "Audio: <none>");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _videoClip = (_videoClip + 1) % (VideoClips.Length + 1);
            if (_videoClip == VideoClips.Length)
            {
                MainVideoPlayer.gameObject.SetActive(false);
            } else
            {
                MainVideoPlayer.gameObject.SetActive(true);
                MainVideoPlayer.clip = VideoClips[_videoClip];
            }
            DebugGUI.LogPersistent("video", "Video: " + VideoClips[_videoClip].name);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Scenes[_scene].SetActive(false);
            _scene = (_scene + 1) % Scenes.Length;
            Scenes[_scene].SetActive(true);
            DebugGUI.LogPersistent("scene", "Scene: " + Scenes[_scene].name);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayerMock.SetActive(!PlayerMock.activeSelf);
        }
    }
}
