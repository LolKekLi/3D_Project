#region

using SRF;
using UnityEngine.UI;

#endregion

namespace SRDebugger.UI.Controls
{
    public class InfoBlock : SRMonoBehaviourEx
    {
        [RequiredField] public Text Content;

        [RequiredField] public Text Title;
    }
}
