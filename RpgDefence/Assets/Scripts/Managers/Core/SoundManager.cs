using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MP3 Player(뭘로 음악을 재생할 것인지) -> AudioSource(음악 재생장치)
// MP3 음반(어떤 음악을 재생할 것인지)   -> AudioClip(음반)
// 듣는사람 -> AudioListener

public class SoundManager
{
    // Bgm, 그나머지 용도로 일단 2개로 분류
    AudioSource[] _audioSources = new AudioSource[(int)Defines.Sound.MaxCount];

    // 캐싱 역할
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };            
            Object.DontDestroyOnLoad(root); // 새로운 씬으로 넘어갈때도 SoundManager 오브젝트를 파괴하지 않음

            string[] soundNames = System.Enum.GetNames(typeof(Defines.Sound));
            for(int i=0; i<soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform; // root의 하위로 포함되게 설정
            }

            _audioSources[(int)Defines.Sound.Bgm].loop = true;
        }
    }

    // 음원이 없는 버전(음원이 위치하는 경로만 가지고 있음)
    // 해당 위치에 있는 음원을 가져와서 음원이 있는 버전을 호출
    public void Play(string path, Defines.Sound type = Defines.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    // 음원이 있는 버전 
    // 가져온 음원을 음악 플레이어 장치에 올려서 플레이
    public void Play(AudioClip audioClip, Defines.Sound type = Defines.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null) return;

        if (type == Defines.Sound.Bgm)
        {              
            AudioSource audioSource = _audioSources[(int)Defines.Sound.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {            
            AudioSource audioSource = _audioSources[(int)Defines.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    // 씬이 변경될때 기존에 가지고 있는 음반들을 계속 들고가면 메모리 용량을 많이 잡아먹기 때문에 적절하게 데이터를 날려줘야됨
    // 캐시를 비우는 작업을 수행
    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClips.Clear();        
    }

    AudioClip GetOrAddAudioClip(string path, Defines.Sound type = Defines.Sound.Effect)
    {
        // 음원 경로를 찾는 부분 
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;
        if (type == Defines.Sound.Bgm)
        {
            // 음원을 가져오는 부분 
            audioClip = Managers.Resource.Load<AudioClip>(path);
            if (audioClip == null)            
                Debug.Log($"Audio Clip Missing! {path}");
        }
        else
        {
            // 매번 해당 폴더에서 그 음악을 찾아서 가져오는 작업이 많아지는 경우를 대비해서 캐싱 작업을 하도록 만듬
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                if (audioClip == null)
                    Debug.Log($"Audio Clip Missing! {path}");
                else                
                    _audioClips.Add(path, audioClip);                                                 
            }            
        }        
        
        return audioClip;
    }
}
