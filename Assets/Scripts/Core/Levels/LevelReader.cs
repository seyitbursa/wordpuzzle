using UnityEngine;

namespace Core.Levels
{
    public class LevelReader : MonoBehaviour
    {
        [SerializeField] private TextAsset levelFile;
        [SerializeField] private LevelInformation levelInformation;
        private ILevelReader levelReader;

        private void Awake()
        {
            levelReader = GetComponent<ILevelReader>();
            levelInformation.LevelDetails = levelReader.ReadLevels(levelFile.text);
        }
    }
}