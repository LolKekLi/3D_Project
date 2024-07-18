#region

using DG.Tweening;

#endregion

namespace Project.UI
{
    public class ChildTweenController : BaseTweenController
    {
        protected override void Awake()
        {
            base.Awake();

            _animations = GetComponentsInChildren<DOTweenAnimation>();
        }
    }
}