using Core.Levels;
using DG.Tweening;
using UnityEngine;

namespace PuzzleSolving
{
    public class ExtraWordsController : MonoBehaviour
    {
        [SerializeField] private LevelInformation levelInformation;
        [SerializeField] private TMPro.TMP_Text tmpText;
        [SerializeField] private int extraWordCount;

        private void Awake()
        {
            levelInformation.ExtraWordFound += OnExtraWordFound;
            levelInformation.FoundExtraWordFound += OnFoundExtraWordFound;
        }

        private void OnExtraWordFound(string word)
        {
            extraWordCount += 1;
            tmpText.text = extraWordCount.ToString();
        }

        private void OnFoundExtraWordFound(string word)
        {
            transform.DOShakeRotation(0.5f, 45);
        }
    }
}
