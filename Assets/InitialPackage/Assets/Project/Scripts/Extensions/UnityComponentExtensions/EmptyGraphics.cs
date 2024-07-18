#region

using UnityEngine.UI;

#endregion

namespace Project.UI
{
    public class EmptyGraphics : MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }
}