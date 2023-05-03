using UnityEngine;

namespace Enemies.Core
{
    public sealed class EnemyCombatTargetDetection : MonoBehaviour, Common.Interfaces.IHearable
    {
        #region Vision
        [SerializeField] float _visionConeRadius;
        [Range(0, 360)] [SerializeField] float _visionConeAngle;

        [SerializeField] LayerMask _targetMask;
        [SerializeField] LayerMask _obstructionMask;
        public Transform _targetReferance { get; private set; }

        public bool _targetInSight { get; private set; }
        public float minNoiseLevelDetection { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        #endregion
        public void Tick()
        {
            Vision();
        }
        void Vision()
        {
            Collider[] collidersInRangeCheck = Physics.OverlapSphere(transform.position, _visionConeRadius, _targetMask);

            if (collidersInRangeCheck.Length > 0)
            {
                Transform target = collidersInRangeCheck[0].transform;
                Vector3 dir = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dir) < _visionConeAngle / 2)
                {
                    float distaceToTarget = Vector3.Distance(target.position, transform.position);

                    if (!Physics.Raycast(transform.position, dir, distaceToTarget, _obstructionMask))
                    {
                        if (target.GetComponent<Common.Interfaces.IEnemyTarget>() != null)
                        {
                            _targetReferance = target;
                            _targetInSight = true;
                        }
                    }
                }
                else
                {
                    _targetInSight = false;
                    _targetReferance = null;
                }
            }
            else if (_targetInSight)
            {
                _targetInSight = false;
                _targetReferance = null;
            }
        }

#if UNITY_EDITOR
        public float visionConeRadius = 100;
        public float visionConeAngle = 45;
        public void Construct()
        {
            Debug.Log("Construktor");
            visionConeRadius = _visionConeRadius;
            visionConeAngle = _visionConeAngle;
        }
        public void IsTargetDetected(float noiseLevel, Transform target)
        {
            if (noiseLevel > minNoiseLevelDetection)
            {
                _targetInSight = target;
            }
            else
            {
                return;
            }
        }
#endif
    }
}