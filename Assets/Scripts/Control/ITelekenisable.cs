using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Control
{
    public interface ITelekenisable
    {
        void Initialize(Telekinesis telekinesis);
        void Holding(Transform holdingPosition, float speed, Camera mainCamera){}
        void JitterObject(Telekinesis telekinesis,float duration, float strength, int vibrato, float randomness);
        void JitterDispose();
        async UniTaskVoid Lift(Transform upPosition,float offset,float speed,float duration,Telekinesis telekinesis) { }
        void Throw(float throwForce, Camera mainCamera,float maxDistance);
        void CancellationToken();
    }
}

