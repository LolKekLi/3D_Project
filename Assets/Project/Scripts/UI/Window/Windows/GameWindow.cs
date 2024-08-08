using TMPro;
using UnityEngine;

namespace Project.UI
{
    public class GameWindow : Window
    {
        [SerializeField] private TMP_Text _coin;
        [SerializeField] private TMP_Text _ruby;

        protected override void Refresh()
        {
            base.Refresh();

            _coin.text = _user.Coins.ToString();
            _ruby.text = _user.Rubys.ToString();
        }
    }
}