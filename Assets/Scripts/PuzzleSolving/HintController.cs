using System.Collections.Generic;
using Core.Levels;
using PuzzleBuilding;
using UnityEngine;

namespace PuzzleSolving
{
    public class HintController : MonoBehaviour
    {
        [SerializeField] private Puzzle puzzle;
        [SerializeField] private LevelInformation levelInformation;
        [SerializeField] private int hintCount;
        [SerializeField] private TMPro.TMP_Text hitCountText;

        private void Awake()
        {
            hintCount = System.Convert.ToInt32(hitCountText.text);
        }

        public void OnPointerDown()
        {
            if (hintCount > 0)
            {
                List<WordDetail> notFoundWords = levelInformation.CurrentLevelDetail.Words.FindAll(p => false == levelInformation.FoundWords.Contains(p));
                int index = Random.Range(0, notFoundWords.Count - 1);
                WordDetail wordDetail = notFoundWords[index];
                List<GridItem> gridItems = puzzle.GetGridItemsOfWord(wordDetail);
                List<GridItem> invisibleItems = gridItems.FindAll(p => false == p.IsLetterVisible);
                index = Random.Range(0, invisibleItems.Count - 1);
                invisibleItems[index].ShowLetterWithHint();
                hintCount -= 1;
                hitCountText.text = hintCount.ToString();
            }
            else
            {
                print("Hint Count Is 0");
            }
        }
    }
}