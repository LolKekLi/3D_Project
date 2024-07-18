#region

using System;

#endregion

namespace Project.Meta
{
    public interface ICommand
    {
        event Action<ICommand, bool> Completed;
        
        void Execute();
    }
}