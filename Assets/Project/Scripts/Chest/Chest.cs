using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project;
using Project.Scripts;
using Project.UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[Serializable]
public class KeyWithDelay
{
    public string key;
    public float delay;
}

public class Chest : MonoBehaviour
{
    [SerializeField] private int _objectCount;
    [SerializeField] private float _force;
    [SerializeField] private float _forceTorgue;
    [SerializeField] private Animator _animator;

    [SerializeField] private KeyWithDelay[] _keyWithDelay;


    private UISystem _uiSystem;
    private Dictionary<string, object> _choosePopupData;
    private PoolManager _poolManager;

    [Inject]
    private void Construct(UISystem uiSystem, PoolManager poolManager)
    {
        _poolManager = poolManager;
        _uiSystem = uiSystem;
    }

    private void Start()
    {
        Action spawnLootAction = SpawnLoot;

        _choosePopupData = new Dictionary<string, object>()
        {
            { ChoosePopup.CHOOSE_BUTTON_TEXT_KEY, "Open chest" },
            { ChoosePopup.CHOOSE_BUTTON_ACTION, spawnLootAction }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            _uiSystem.ShowWindow<ChoosePopup>(_choosePopupData);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            _uiSystem.ShowWindow<GameWindow>();
        }
    }

    private async void SpawnLoot()
    {
        var keyWithDelay = _keyWithDelay.RandomElement();
        _animator.SetTrigger(keyWithDelay.key);

        await UniTask.Delay(TimeSpan.FromSeconds(keyWithDelay.delay));

        var lootObjects = _poolManager.PoolSettings.PickableLootObjects;

        for (int i = 0; i < _objectCount; i++)
        {
            var pickableLootObject = lootObjects.RandomElement();

            var lootObject = SpawnLootObject(pickableLootObject);

            lootObject.Rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * _force);

            lootObject.Rigidbody.AddTorque(new Vector3(
                                               Random.Range(0, 1f),
                                               Random.Range(0, 1f),
                                               Random.Range(0, 1f)) *
                                           _forceTorgue);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        gameObject.SetActive(false);
    }

    private AbstarctPickableLootObject SpawnLootObject(AbstarctPickableLootObject pickableLootObject)
    {
        switch (pickableLootObject)
        {
            case PickableLootCoin coin:
                return _poolManager.Get<PickableLootCoin>(pickableLootObject, transform.position, Quaternion.identity);
            case PickableLootRuby ruby:
                return _poolManager.Get<PickableLootRuby>(pickableLootObject, transform.position, Quaternion.identity);
            default:
                return null;
        }
    }
}