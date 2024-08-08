using Cysharp.Threading.Tasks;
using Project;
using Project.Scripts;
using Project.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class CoinDonater : MonoBehaviour
{
    [SerializeField] private int _objectCount;
    [SerializeField] private float _force;
    [SerializeField] private float _forceTorgue;

    [SerializeField] private KeyWithDelay[] _keyWithDelay;


    private UISystem _uiSystem;
    private Dictionary<string, object> _getFreeCoinsData;
    private PoolManager _poolManager;


    [Inject]
    private void Construct(UISystem uiSystem, PoolManager poolManager)
    {
        _poolManager = poolManager;
        _uiSystem = uiSystem;
    }

    private void Start()
    {
        Action spawnCoinsAction = SpawnCoins;

        _getFreeCoinsData = new Dictionary<string, object>()
        {
            { CoinDonaterPopup.GET_FREE_COINS_BUTTON_TEXT_KEY, "Get free coins!"},
            { CoinDonaterPopup.GET_FREE_COINS_BUTTON_ACTION, spawnCoinsAction }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            _uiSystem.ShowWindow<CoinDonaterPopup>(_getFreeCoinsData);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            _uiSystem.ShowWindow<GameWindow>();
        }
    }

    private async void SpawnCoins()
    {
        Debug.Log("Coins spawn");

        var keyWithDelay = _keyWithDelay.RandomElement();

        await UniTask.Delay(TimeSpan.FromSeconds(keyWithDelay.delay));

        var lootObjects = _poolManager.PoolSettings.PickableLootObjects;

        for (int i = 0; i < _objectCount; i++)
        {
            var pickableLootObject = lootObjects[i];

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
        return _poolManager.Get<PickableLootCoin>(pickableLootObject, transform.position, Quaternion.identity);
    }
}
