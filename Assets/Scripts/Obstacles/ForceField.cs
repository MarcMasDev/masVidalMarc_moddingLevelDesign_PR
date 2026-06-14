using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    public enum ForceApplicationMode
    {
        Continuous, // Wind zone - applies force every frame (ForceMode.Force)
        Impulse // Jump pad - applies force once on entry (ForceMode.Impulse)
    }

    public class ForceField : MonoBehaviour
    {
        [Header("Force Settings")]
        [SerializeField] private ForceApplicationMode _forceMode = ForceApplicationMode.Continuous;
        [SerializeField] private bool _useLocalSpace = false;
        [SerializeField] private Vector3 _windDirection = Vector3.forward;
        [SerializeField] private float _maxWindStrength = 10f;
        [SerializeField] private AnimationCurve _falloffCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

        private readonly HashSet<Rigidbody> _rigidbodiesInZone = new();
        private readonly HashSet<Rigidbody> _rigidbodiesAlreadyImpulsed = new();
        private BoxCollider _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var rb = other.GetComponent<Rigidbody>();
            if (rb == null) return;

            _rigidbodiesInZone.Add(rb);

            // For impulse mode, apply force immediately on entry
            if (_forceMode == ForceApplicationMode.Impulse && !_rigidbodiesAlreadyImpulsed.Contains(rb))
            {
                float falloff = CalculateFalloff(rb.position);

                // Transform direction based on space mode
                var normalizedWindDirection = _useLocalSpace
                    ? transform.TransformDirection(_windDirection.normalized)
                    : _windDirection.normalized;

                Vector3 impulseForce = normalizedWindDirection * (_maxWindStrength * falloff);
                rb.AddForce(impulseForce, ForceMode.Impulse);
                _rigidbodiesAlreadyImpulsed.Add(rb);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var rb = other.GetComponent<Rigidbody>();
            if (rb == null)
                return;
            _rigidbodiesInZone.Remove(rb);
            _rigidbodiesAlreadyImpulsed.Remove(rb); // Reset impulse tracking on exit
        }

        private void FixedUpdate()
        {
            // Only apply continuous force in Continuous mode
            if (_forceMode != ForceApplicationMode.Continuous)
                return;

            // Transform direction based on space mode
            var normalizedWindDirection = _useLocalSpace
                ? transform.TransformDirection(_windDirection.normalized)
                : _windDirection.normalized;

            foreach (Rigidbody rb in _rigidbodiesInZone)
            {
                if (rb == null) continue;

                // Calculate distance falloff
                float falloff = CalculateFalloff(rb.position);

                // Apply wind force
                Vector3 windForce = normalizedWindDirection * (_maxWindStrength * falloff);
                rb.AddForce(windForce, ForceMode.Force);
            }
        }

        private float CalculateFalloff(Vector3 position)
        {
            // Get the center of the box collider in world space
            Vector3 center = transform.TransformPoint(_boxCollider.center);

            // Calculate distance from center
            float distance = Vector3.Distance(position, center);

            // Get the maximum extent of the box (approximation)
            Vector3 worldExtents = Vector3.Scale(_boxCollider.size, transform.lossyScale) * 0.5f;
            float maxDistance = Mathf.Max(worldExtents.x, worldExtents.y, worldExtents.z);

            // Normalize distance (0 at center, 1 at edge/beyond)
            float normalizedDistance = Mathf.Clamp01(distance / maxDistance);

            // Evaluate falloff curve (input: 0 at center, 1 at edge)
            return _falloffCurve.Evaluate(normalizedDistance);
        }

        private void OnDrawGizmos()
        {
            var boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null) return;

            // Draw the wind zone bounds
            Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);

            // Draw wind direction arrows
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.cyan;

            Vector3 center = transform.TransformPoint(boxCollider.center);

            // Transform direction based on space mode
            Vector3 normalizedWindDir = _useLocalSpace
                ? transform.TransformDirection(_windDirection.normalized)
                : _windDirection.normalized;

            float arrowLength = 2f;

            // Draw main direction arrow
            Gizmos.DrawRay(center, normalizedWindDir * arrowLength);

            // Draw arrowhead
            Vector3 arrowTip = center + normalizedWindDir * arrowLength;
            Vector3 perpendicular = Vector3.Cross(normalizedWindDir, Vector3.up).normalized;
            if (perpendicular == Vector3.zero)
                perpendicular = Vector3.Cross(normalizedWindDir, Vector3.right).normalized;

            Gizmos.DrawLine(arrowTip, arrowTip - normalizedWindDir * 0.3f + perpendicular * 0.15f);
            Gizmos.DrawLine(arrowTip, arrowTip - normalizedWindDir * 0.3f - perpendicular * 0.15f);
        }
    }
}