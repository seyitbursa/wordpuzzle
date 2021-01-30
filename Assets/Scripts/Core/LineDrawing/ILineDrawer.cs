using UnityEngine;

namespace Core.LineDrawing
{

    public interface ILineDrawer
    {
        void AddLinePoint(Vector3 linePoint,bool isTemp);
        void ClearLinePoints();
        void RemoveLastLine();
        void RemoveTempLine();
    }
}