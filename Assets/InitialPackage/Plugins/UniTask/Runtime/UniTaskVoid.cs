#pragma warning disable CS1591
#pragma warning disable CS0436

#region

using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks.CompilerServices;

#endregion

namespace Cysharp.Threading.Tasks
{
    [AsyncMethodBuilder(typeof(AsyncUniTaskVoidMethodBuilder))]
    public readonly struct UniTaskVoid
    {
        public void Forget()
        {
        }
    }
}

