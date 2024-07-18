#region

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

#endregion

namespace Project.UI
{
    public interface IWindow
    {
        Transform Transform
        {
            get;
        }

        bool IsPopup
        {
            get;
        }
        
        void Show();
       
        void Preload();
        
        UniTask Hide(bool isNeedAnimation, CancellationToken cancellationToken);
    }
}