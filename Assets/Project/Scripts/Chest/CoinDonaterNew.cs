using System;
using System.Collections.Generic;
using Project.Meta;
using Project.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Chest
{
    public class CoinDonaterNew : ChooseObject
    {
        [SerializeField] private int _coinCount;

        [Inject] private IUser _user;
        [Inject] private ParticlesManager _particlesManager;
        
        public override Dictionary<string, object> GetPopupData()
        {
            Action freeCoins = GetFreeCoins;
            
            _choosePopupData = new Dictionary<string, object>()
            {
                { ChoosePopup.CHOOSE_BUTTON_TEXT_KEY, "Get free coin" },
                { ChoosePopup.CHOOSE_BUTTON_ACTION, freeCoins }
            };

            return _choosePopupData;
        }

        private void GetFreeCoins()
        {
            _user.SetCurrency(CurrencyType.Coin, _coinCount);
            
            _particlesManager.Emit(ParticleType.MoneyBlast, transform.position);
            
            gameObject.SetActive(false);
        }
    }
}