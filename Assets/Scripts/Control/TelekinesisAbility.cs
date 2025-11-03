using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TelekinesisAbility : MonoBehaviour
{
    [Header("Layers")] [SerializeField] private GameObject layers;
    [SerializeField] private Image forwardLayer;
    [SerializeField] private Image backLayer;
    [Header("Settings")] [SerializeField] private float decreaseTime = 0.1f;
    [SerializeField] private float increaseTime = 3;
    [SerializeField] private float abilityAmount;
    [Header("Feedback")] [SerializeField] private Color abilityOverColor = Color.red;
    [SerializeField] private Color abilityOverColorBackLayer = Color.brown;
    [SerializeField] private Color abilityNormalColor = Color.white;

    private float _defaultAbilityAmount;
    private CancellationTokenSource _cts;
    private float _initialLayersScaleY;
    private bool _isAbilityOver;
    private readonly Dictionary<Image, float> _layerDict = new Dictionary<Image, float>();

    private void Awake()
    {
        _cts = new CancellationTokenSource();
    }

    private void Start()
    {
        _defaultAbilityAmount = forwardLayer.fillAmount;
        _initialLayersScaleY = layers.transform.localScale.y;
        Image[] layersImage = layers.GetComponentsInChildren<Image>(includeInactive: true);
        foreach (var layer in layersImage)
            _layerDict.Add(layer, layer.color.a);
    }

    public async UniTaskVoid Decrease()
    {
        LayerAnimation(tween: () => layers.transform.DOScaleY(_initialLayersScaleY, 0.1f));
        foreach (var kvp in _layerDict) LayerAnimation(() => kvp.Key.DOFade(kvp.Value, 0.1f));
        float initialAmount = forwardLayer.fillAmount;
        float targetAmount = Mathf.Clamp01(initialAmount - abilityAmount / 100);
        try
        {
            float elapsed = 0f;
            _defaultAbilityAmount = targetAmount;
            if (_defaultAbilityAmount <= 0)
            {
                LayerColorChange(backLayer, abilityOverColorBackLayer);
                _isAbilityOver = true;
            }

            while (elapsed < decreaseTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / decreaseTime);
                forwardLayer.fillAmount = Mathf.Lerp(forwardLayer.fillAmount, targetAmount, t);
                await UniTask.Yield(cancellationToken: _cts.Token);
            }

            forwardLayer.fillAmount = targetAmount;
            if (_isAbilityOver) LayerColorChange(forwardLayer, abilityOverColor);
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
        }
    }


    public async UniTaskVoid Increase()
    {
        try
        {
            await UniTask.WaitForSeconds(2f, cancellationToken: _cts.Token);
            if (_isAbilityOver) LayerColorChange(backLayer, abilityNormalColor);
            float initialAmount = forwardLayer.fillAmount;
            float targetAmount = 1;
            float elapsed = 0f;
            while (elapsed < increaseTime)
            {
                if (Mathf.Approximately(forwardLayer.fillAmount, targetAmount)) break;
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / increaseTime);
                forwardLayer.fillAmount =
                    Mathf.Lerp(initialAmount, targetAmount, t);
                _defaultAbilityAmount = forwardLayer.fillAmount;
                await UniTask.Yield(cancellationToken: _cts.Token);
            }

            _isAbilityOver = false;
            forwardLayer.fillAmount = targetAmount;
            _defaultAbilityAmount = targetAmount;
            float endValue = 0;
            LayerColorChange(forwardLayer, abilityNormalColor);
            await UniTask.WaitForSeconds(.2f, cancellationToken: _cts.Token);
            LayerAnimation(tween: () => layers.transform.DOScaleY(endValue, 0.1f));
            foreach (var kvp in _layerDict) LayerAnimation(() => kvp.Key.DOFade(endValue, 0.1f));
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
        }
    }

    private void LayerColorChange(Image layer, Color targetColor)
    {
        layer.color = new Color(targetColor.r, targetColor.g, targetColor.b, layer.color.a);
    }

    private void LayerAnimation(Func<Tween> tween, Action onComplete = null)
    {
        Tween layerTween = tween();
        layerTween.OnComplete(() => { onComplete?.Invoke(); });
    }

    public bool IsUseAbility()
    {
        return !_isAbilityOver;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public void Cancellation()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }
}