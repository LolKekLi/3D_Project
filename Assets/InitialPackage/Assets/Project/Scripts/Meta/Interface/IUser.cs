#region

using UniRx;

#endregion

namespace Project.Meta
{
    public interface IUser
    {
        public IReadOnlyReactiveProperty<int> Coins
        {
            get;
        }
        
        public IReadOnlyReactiveProperty<int> Rubys
        {
            get;
        }
        
        bool CanUpgrade(CurrencyType type, int amount);
        void SetCurrency(CurrencyType type, int amount);
    }
}