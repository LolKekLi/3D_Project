using System;
using Project;
using Project.Scripts;
using UnityEngine;

public abstract class AbstarctPickableLootObject : PooledBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            Free();
            Collect();
        }
    }

    protected abstract void Collect();
}