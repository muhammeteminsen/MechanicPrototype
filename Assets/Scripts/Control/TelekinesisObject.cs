using System;
using System.Threading;
using Control;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class TelekinesisObject : MonoBehaviour, ITelekenisable
{
    private Rigidbody _rb;
    private CancellationTokenSource _cts;
    private float _initialDamping;
    
    public void Initialize(Telekinesis telekinesis)
    {
        if (TryGetComponent(out Rigidbody rb))
        {
            _rb = rb;
            _rb.isKinematic = false;
            _rb.useGravity = false;
            _initialDamping = _rb.linearDamping;
            _rb.linearDamping = telekinesis.holdingDamping;
        }

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }

    private Tweener _jitterObjectTweener;

    public void JitterObject(Telekinesis telekinesis, float duration, float strength, int vibrato, float randomness)
    {
        if (_jitterObjectTweener != null) return;
        transform.SetParent(telekinesis.jitterPoint);
        _jitterObjectTweener = telekinesis.jitterPoint.DOShakePosition(
            duration: duration,
            strength: strength,
            vibrato: vibrato,
            randomness: randomness,
            fadeOut: false
        ).SetLoops(-1, LoopType.Restart);
    }
    public void JitterDispose()
    {
        transform.SetParent(null);
        _jitterObjectTweener?.Kill();
        _jitterObjectTweener = null;
    }

    public async UniTaskVoid Lift(Transform upPosition, float offset, float speed, float duration,
        Telekinesis telekinesis)
    {
        bool canceled = false;
        try
        {
            Vector3 targetPosition = transform.position + new Vector3(0f, offset, 0f);
            upPosition.position = targetPosition;
            Vector3 initialPosition = upPosition.position;
            Vector3 direction = initialPosition - transform.position;
            float elapsed = 0f;
            while (duration > elapsed)
            {
                elapsed += Time.deltaTime;

                _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, direction, Time.deltaTime * 5f);
                direction = initialPosition - transform.position;
                _rb.AddForce(direction * speed, ForceMode.Acceleration);
                await UniTask.Yield(cancellationToken: _cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            canceled = true;
            telekinesis.ChangeState(new TelekinesisThrowState());
        }
        finally
        {
            if (!canceled)
                telekinesis.ChangeState(new TelekinesisOutHoldingState());
        }
    }

    public void Holding(Transform holdingPosition, float speed, Camera mainCamera)
    {
        Vector3 direction = holdingPosition.position - transform.position;
        _rb.AddForce(direction * speed, ForceMode.Acceleration);
        _rb.angularVelocity = Vector3.Slerp(_rb.angularVelocity, Vector3.zero, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(mainCamera.transform.forward, Vector3.up), Time.deltaTime * 5f);
    }


    public void Throw(float throwForce, Camera mainCamera, float maxDistance)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.origin + ray.direction * maxDistance;
        Vector3 direction = (targetPoint - transform.position).normalized;
        _rb.linearVelocity = Vector3.zero;
        _rb.linearDamping = _initialDamping;
        _rb.useGravity = true;
        _rb.AddForce(direction * throwForce, ForceMode.Impulse);
    }

    public void CancellationToken()
    {
        _cts?.Cancel();
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}