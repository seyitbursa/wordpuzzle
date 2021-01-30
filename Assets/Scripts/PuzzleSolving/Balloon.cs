using Core.Pooling;

namespace PuzzleSolving
{
    public class Balloon : PoolObject
    {
        private void OnEnable()
        {
            objectType = ObjectType.Balloon;
        }
    }
}