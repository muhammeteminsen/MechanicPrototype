using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TelekinesisAbility : MonoBehaviour
{
    [SerializeField] private Image forwardLayer;
    [SerializeField] private Image backLayer;
    [SerializeField] private float decreaseTime = 0.1f;
    [SerializeField] private float increaseTime = 3;
    [SerializeField] private float abilityAmount;
    private float _defaultAbilityAmount;
    private CancellationTokenSource _cts;

    private void Awake()
    {
        _defaultAbilityAmount = forwardLayer.fillAmount;
        _cts = new CancellationTokenSource();
    }

    public async UniTaskVoid DecreaseBar()
    {
        float initialAmount = forwardLayer.fillAmount;
        float targetAmount = initialAmount - abilityAmount / 100;
        try
        {
            float elapsed = 0f;
            _defaultAbilityAmount = targetAmount;
            while (elapsed < decreaseTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / decreaseTime);
                forwardLayer.fillAmount = Mathf.Lerp(forwardLayer.fillAmount, targetAmount, t);
                await UniTask.Yield(cancellationToken: _cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("sadasadasdfffffds");
        }
        
    }
    public async UniTaskVoid IncreaseBar()
    {
        try
        {
            await UniTask.WaitForSeconds(1f, cancellationToken: _cts.Token);
            float targetAmount = 1;
            float elapsed = 0f;
            float initialAmount = forwardLayer.fillAmount;
            while (elapsed < increaseTime)
            {
                if (Mathf.Approximately(initialAmount, targetAmount)) break;
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / increaseTime);
                forwardLayer.fillAmount =
                    Mathf.Lerp(forwardLayer.fillAmount, targetAmount, t);
                _defaultAbilityAmount = forwardLayer.fillAmount;
                await UniTask.Yield(cancellationToken: _cts.Token);
               
            }
            forwardLayer.fillAmount = targetAmount;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("IncreaseBar canceled");
        }
        finally
        {
            Debug.Log("IncreaseBar finished");
        }
    }

    public bool IsUseAbility()
    {
        return _defaultAbilityAmount > abilityAmount/100;
    }
    public void CancellationToken()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }
    
    
    
}
