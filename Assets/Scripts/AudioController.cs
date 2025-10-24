using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Audio Source Settings")]
    [SerializeField] private AudioClip audioClip;

    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool loop;
    [Range(0f, 1f), SerializeField] private float volume = 1f;
    [SerializeField, Range(-1f, 1f)] private float stereoPan;
    [SerializeField, Range(-1f, 1f)] private float spatialBlend;

    [Header("3D Settings")] [SerializeField, Range(0f, 5f)]
    private float dopplerLevel = 1f;

    [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Custom;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 500f;
    [SerializeField] private AnimationCurve rollOfCurve = AnimationCurve.Linear(0, 1, 1, 0);

    [Header("Audio Position")] public Transform audioPosition;
    [SerializeField] private GameObject audioSourcePrefab;
    private AudioSource _audioSource;
    private AudioControllerManager _audioControllerManager;

    private void Awake()
    {
        _audioControllerManager = new AudioControllerManager(audioSourcePrefab);
    }
    public void PlaySound(AudioClip targetClip = null, Transform targetPosition = null)
    {
        Initialize(targetClip,targetPosition);
        _audioSource.Play();
    }

    private void Initialize(AudioClip targetClip = null, Transform targetPosition = null)
    {
        _audioSource = _audioControllerManager.GetSource();
        _audioSource.clip = targetClip ?? audioClip;
        _audioSource.playOnAwake = playOnAwake;
        _audioSource.loop = loop;
        _audioSource.volume = volume;
        _audioSource.panStereo = stereoPan;
        if (spatialBlend > 0f)
        {
            _audioSource.spatialBlend = spatialBlend;
            _audioSource.dopplerLevel = dopplerLevel;
            _audioSource.rolloffMode = rolloffMode;
            _audioSource.minDistance = minDistance;
            _audioSource.maxDistance = maxDistance;
            _audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, rollOfCurve);
            _audioSource.transform.SetParent(targetPosition?.transform);
            _audioSource.transform.position = Vector3.zero;
        }
        _audioControllerManager.ReleaseSource(_audioSource);
    }
}