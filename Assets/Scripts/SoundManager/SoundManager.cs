using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource _backgroundAudioSource;
    [SerializeField] AudioSource _uiAudioSource;
    [SerializeField] AudioSource _effectAudioSource;

    [SerializeField] AudioClip _backgroundSound;
    [SerializeField] AudioClip[] _uiClips;
    [SerializeField] AudioClip[] _effectClips;

    protected override void init()
    {
        _backgroundAudioSource.clip = _backgroundSound;
        PlayBackgroundSound();
    }
    public void PlayBackgroundSound()
    {
        _backgroundAudioSource?.Play();
    }

    public void StopBackgroundSound()
    {
        _backgroundAudioSource?.Stop();
    }

    /// <summary>
    /// 호출시마다 반전됨
    /// </summary>
    public void MuteBackgroundSound()
    {
        if (_backgroundAudioSource != null)
        {
            _backgroundAudioSource.mute = !_backgroundAudioSource.mute;
        }
    }

    /// <summary>
    /// UI 사운드 디스플레이용 메서드
    /// UISoundClipEnum 의 값을 받으면 저장되어 있는 배열의 클립을 플레이해줌
    /// </summary>
    /// <param name="selUiClip">UISoundClipEnum.cs 형식 참고</param>
    public void PlayUiSound(UISoundClipEnum selUiClip)
    {
        ChageUiAudioClip(selUiClip);
        _uiAudioSource.Play();
        ResetUiAudioClip();
    }

    private void ChageUiAudioClip(UISoundClipEnum selUiClip)
    {
        if(_uiClips.Length <= (int)selUiClip)
        {
            _uiAudioSource.clip = null;
            return;
        }
        _uiAudioSource.clip =  _uiClips[(int)selUiClip]; ;
    }
    private void ResetUiAudioClip()
    {
        _uiAudioSource.clip = null;
    }
}
