using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Control
{
    public interface ITelekinesisable
    {
        void Initialize(Telekinesis telekinesis);
        void Holding(Transform holdingPosition, float speed, Camera mainCamera){}
        void JitterObject(Telekinesis telekinesis,float duration, float strength, int vibrato, float randomness);
        void JitterDispose();
        UniTask<GameObject> Lift(Transform upPosition, float offset, float speed, float duration, Telekinesis telekinesis);
        void Throw(float throwForce, Camera mainCamera,float maxDistance);
        void CancellationToken();
    }
}

