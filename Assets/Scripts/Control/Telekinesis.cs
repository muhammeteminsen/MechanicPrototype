using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Control
{
    public class Telekinesis : MonoBehaviour
    {
        public AnimationController animationController;
        public AudioController audioController;
        
        public float maxDistance = 20f;
        [Header("Throw")] public float throwForce = 10f;

        [Header("Holding")] public Transform holdingPoint;
        public float holdingSpeed = 10f;
        public float holdingThreshold = 0.15f;
        public float holdingDamping = 10f;

        [Header("Lift")] public Transform liftPoint;
        public float liftOffset = 2f;
        public float liftSpeed = 5f;
        public float liftDuration = 0.5f;

        [Header("Jitter Object")] public Transform jitterPoint;
        public float duration = 0.5f;
        public float strength = 0.2f;
        public int vibrato = 1;
        public float randomness = 90f;
        [Header("Jitter Camera")] 
        [SerializeField] private Transform cameraFollowTarget;
        [SerializeField] private float d = 0.5f;
        [SerializeField] private float s = 0.2f;
        [SerializeField] private int v = 1;
        [SerializeField] private float r = 90f;
        private ITelekenisable _telekinesisable;
        public List<GameObject> VisibleObjects { get; set; } = new List<GameObject>();
        public Camera MainCamera => Camera.main;
        public TelekinesisAbility TelekinesisAbility => GetComponent<TelekinesisAbility>();

        public GameObject Closest { get; set; }

        private StateBase<Telekinesis> _stateBase;

        private void Start()
        {
            _stateBase = new StateBase<Telekinesis>(this);
            _stateBase.ChangeState(new TelekinesisIdleState());
        }

        private void Update()
        {
            _stateBase.Update();
        }

        private void FixedUpdate()
        {
            _stateBase.FixedUpdate();
        }

        public void ChangeState(StateAction<Telekinesis> telekinesis)
        {
            _stateBase.ChangeState(telekinesis);
        }

        public float Direction()
        {
            float directionSqr = (holdingPoint.position - Closest.transform.position).sqrMagnitude;
            return directionSqr;
        }

        private Tweener _jitterCameraTweener;

        public void JitterCamera()
        {
            if (_jitterCameraTweener != null) return;
            _jitterCameraTweener = cameraFollowTarget.DOShakePosition(
                duration: d,
                strength: s,
                vibrato: v,
                randomness: r,
                fadeOut: false
            ).SetLoops(-1, LoopType.Restart);
        }

        public void JitterDisposeCamera()
        {
            _jitterCameraTweener?.Kill();
            _jitterCameraTweener = null;
        }
    }
}