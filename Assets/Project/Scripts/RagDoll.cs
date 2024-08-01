using UnityEditor;
using UnityEngine;

namespace Project.Scripts
{
    public class RagDoll : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] _addForceRb;
        [SerializeField] private Rigidbody[] _rigidbodies;
        [SerializeField] private Collider _playerCollider;
        [SerializeField] private Collider[] _colliders;

        private void Awake()
        {
            Disable();
        }

        [ContextMenu("GetRB")]
        private void GetRigidBodies()
        {
            _rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
            _colliders = transform.GetComponentsInChildren<Collider>();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }


        public void Enable()
        {
            _playerCollider.enabled = false;

            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
            }

            foreach (var collider in _colliders)
            {
                collider.enabled = true;
            }
        }

        public void Disable()
        {
            _playerCollider.enabled = true;

            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
            }

            foreach (var collider in _colliders)
            {
                collider.enabled = false;
            }
        }

        public void AddForce(Vector3 force)
        {
            _addForceRb.Do(x => x.AddForce(force, ForceMode.Impulse));
        }
    }
}