using Project;
using Project.Meta;
using Zenject;

public class PickableLootCoin : AbstarctPickableLootObject
{
    [Inject] private IUser _user;

    protected override void Collect()
    {
        _user.SetCurrency(CurrencyType.Coin, 1);
    }
}