#region

using UniRx;

#endregion

namespace Project.Meta
{
    public interface ILevelData
    {
        IReadOnlyReactiveProperty<int> LevelIndexProperty
        {
            get;
        }

        int LevelIndex
        {
            get;
            set;
        }
    }
}