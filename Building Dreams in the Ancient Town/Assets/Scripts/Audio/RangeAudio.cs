using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RangeAudio : MonoBehaviour
{
    [Header("=== 范围音效设置 ===")]
    public AudioClip rangeClip; // 拖入范围音效
    [Tooltip("最大感应距离")] public float maxRange = 15f;
    [Tooltip("最大音量")] public float maxVolume = 1f;
    [Tooltip("目标（主角/玩家）")] public Transform target;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = rangeClip;
        audioSource.loop = true; // 循环播放
        audioSource.volume = 0; // 初始静音
        audioSource.playOnAwake = false;
        audioSource.Play();
    }

    void Update()
    {
        if (target == null || rangeClip == null) return;

        // 计算距离
        float distance = Vector3.Distance(transform.position, target.position);

        // 根据距离计算音量
        if (distance <= maxRange)
        {
            // 距离越近音量越大
            float volume = Mathf.Lerp(maxVolume, 0, distance / maxRange);
            audioSource.volume = volume;
        }
        else
        {
            // 超出范围静音
            audioSource.volume = 0;
        }
    }
}