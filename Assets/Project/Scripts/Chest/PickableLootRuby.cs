using Project;
using Project.Meta;
using Zenject;

public class PickableLootRuby : AbstarctPickableLootObject
{
    [Inject] private IUser _user;

    protected override void Collect()
    {
        _user.SetCurrency(CurrencyType.Ruby, 1);
    }
}
