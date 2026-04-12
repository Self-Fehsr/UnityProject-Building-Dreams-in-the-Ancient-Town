using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // 单例，全局调用

    [Header("=== 音频设置 ===")]
    [Tooltip("主音量")] public float masterVolume = 1f;
    [Tooltip("背景音乐音量")] public float bgmVolume = 0.6f;
    [Tooltip("音效音量")] public float sfxVolume = 1f;

    [Header("=== 自动加载路径（无需修改）===")]
    public string bgmPath = "Audio/BGM";
    public string sfxPath = "Audio/SFX";

    // 自动加载的音频库
    private Dictionary<string, AudioClip> bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    // 播放器
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 初始化播放器
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        sfxPlayer = gameObject.AddComponent<AudioSource>();

        bgmPlayer.loop = true; // 背景音乐循环
        bgmPlayer.volume = bgmVolume;
    }

    void Start()
    {
        // 自动加载所有音频
        LoadAllAudio();
        PlayBGM("Silent Horror - Decrepit");
    }

    /// <summary>
    /// 自动加载 Resources 文件夹下的所有音频
    /// 注意：所有音频必须放在 Resources/Audio/... 下
    /// </summary>
    void LoadAllAudio()
    {
        AudioClip[] bgms = Resources.LoadAll<AudioClip>(bgmPath);
        foreach (var clip in bgms)
        {
            if (!bgmDict.ContainsKey(clip.name))
                bgmDict.Add(clip.name, clip);
        }

        AudioClip[] sfxs = Resources.LoadAll<AudioClip>(sfxPath);
        foreach (var clip in sfxs)
        {
            if (!sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
        }

        Debug.Log($"加载完成：BGM {bgmDict.Count} 个，音效 {sfxDict.Count} 个");
    }

    #region 背景音乐控制
    public void PlayBGM(string clipName)
    {
        if (bgmDict.TryGetValue(clipName, out AudioClip clip))
        {
            bgmPlayer.clip = clip;
            bgmPlayer.Play();
        }
        else
        {
            Debug.LogWarning("BGM不存在：" + clipName);
        }
    }

    public void StopBGM() => bgmPlayer.Stop();
    #endregion

    #region 全局音效控制
    public void PlaySFX(string clipName)
    {
        if (sfxDict.TryGetValue(clipName, out AudioClip clip))
        {
            sfxPlayer.PlayOneShot(clip, sfxVolume);
        }
        else
        {
            Debug.LogWarning("音效不存在：" + clipName);
        }
    }
    #endregion
}