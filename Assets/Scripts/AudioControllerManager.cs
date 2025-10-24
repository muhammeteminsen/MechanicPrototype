using UnityEngine;
using UnityEngine.Pool;

public class AudioControllerManager
{
    private ObjectPool<AudioSource> _audioSourcePool;
    private readonly GameObject _audioSourcePrefab;
    public AudioControllerManager(GameObject audioSourcePrefab)
    {
        _audioSourcePrefab = audioSourcePrefab;
        _audioSourcePool = new ObjectPool<AudioSource>(CreateNewAudioSource);
    }
    private AudioSource CreateNewAudioSource()
    {
        GameObject audioSourceObject = Object.Instantiate(_audioSourcePrefab);
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        return audioSource;
    }
    public AudioSource GetSource()
    {
        AudioSource source = _audioSourcePool.Get();
        return source;
    }

    public void ReleaseSource(AudioSource source)
    {
        _audioSourcePool.Release(source);
    }
}
