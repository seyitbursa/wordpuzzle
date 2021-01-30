using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Levels;
using Core.Pooling;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleBuilding
{
    public class Puzzle : MonoBehaviour
    {
        [SerializeField] private LevelInformation levelInformation;
        [SerializeField] private ObjectPoolController objectPoolController;
        [SerializeField] private AudioClips audioClips;
        [SerializeField] private int cellSpace;
        [SerializeField] private int maxCellSize;

        private AudioSource audioSource;
        private Dictionary<WordDetail, List<GridItem>> wordDetailGridItems;
        private GridLayoutGroup gridLayoutGroup;
        private List<GridItem> letterGridItemsForRandomSelection;

        public GridItem[,] gridItems;
        public List<GridItem> randomBalloonItems;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
            Initialize();
        }

        private void OnEnable()
        {
            levelInformation.WordFound += OnWordFound;
            levelInformation.WordNotFound += OnWordNotFound;
            levelInformation.ExtraWordFound += OnExtraWordFound;
            levelInformation.LevelFinished += OnLevelFinished;
            levelInformation.WordCheckStarted += OnWordCheckStarted;
            levelInformation.NextLevelStarted += OnNextLevelStarted;
            levelInformation.FoundWordFound += OnFoundWordFound;
        }

        private void Initialize()
        {
            levelInformation.FoundWords = new List<WordDetail>();
            levelInformation.FoundExtraWords = new List<string>();
            letterGridItemsForRandomSelection = new List<GridItem>();

            RectTransform rectTransform = transform.parent.GetComponent<RectTransform>();
            float cellSize = CalculateGridCellSize(rectTransform);

            wordDetailGridItems = new Dictionary<WordDetail, List<GridItem>>();
            gridItems = new GridItem[levelInformation.CurrentLevelDetail.Row, levelInformation.CurrentLevelDetail.Column];

            float startingPositionX = cellSize / 2 + (rectTransform.rect.width - (cellSize * levelInformation.CurrentLevelDetail.Column + ((levelInformation.CurrentLevelDetail.Column + 2) * cellSpace))) / 2;
            float startingPositionY = transform.localPosition.y - ((rectTransform.rect.height - (cellSize * levelInformation.CurrentLevelDetail.Row + ((levelInformation.CurrentLevelDetail.Row + 2) * 5))) + cellSize) / 2;

            for (int i = 0; i < levelInformation.CurrentLevelDetail.Row; i++)
            {
                for (int j = 0; j < levelInformation.CurrentLevelDetail.Column; j++)
                {
                    GameObject gridItemObject = objectPoolController.GetObject(ObjectType.GridItem);
                    gridItemObject.name = "Letter[" + j + "," + i + "]";
                    RectTransform rectTransformGridItem = gridItemObject.GetComponent<RectTransform>();
                    rectTransformGridItem.sizeDelta = new Vector2(cellSize, cellSize);
                    gridItemObject.transform.localPosition = new Vector2(startingPositionX + (cellSpace + (j * (cellSize + cellSpace))), startingPositionY - (cellSpace + (i * (cellSize + cellSpace))));
                    gridItemObject.transform.SetParent(transform);
                    GridItem gridItem = gridItemObject.GetComponent<GridItem>();
                    gridItem.ParentWordDetails = new List<WordDetail>();
                    gridItems[i, j] = gridItem;
                    gridItem.MakeInVisible();
                }
            }

            /*
            gridLayoutGroup.constraintCount = levelInformation.CurrentLevelDetail.Column;
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
            */

            AddWordsOnGrid();
            SetRandomBalloons(3);
        }

        private float CalculateGridCellSize(RectTransform rectTransform)
        {
            float cellSizeWidth = Mathf.Min(maxCellSize, (rectTransform.rect.width - ((levelInformation.CurrentLevelDetail.Column + 2) * cellSpace)) / levelInformation.CurrentLevelDetail.Column);
            float cellSizeHeight = Mathf.Min(maxCellSize, (rectTransform.rect.height - ((levelInformation.CurrentLevelDetail.Row + 2) * cellSpace)) / levelInformation.CurrentLevelDetail.Row);
            float cellSize = Mathf.Min(cellSizeHeight, cellSizeWidth);
            return cellSize;
        }

        private void ClearGridItems()
        {
            foreach (var item in gridItems)
            {
                item.LetterShown -= OnLetterShown;
                item.LetterShownWithHint -= OnLetterShown;
                item.ClearLetter();
                item.ParentWordDetails.Clear();
                objectPoolController.ResetObject(item.gameObject);
            }
        }

        private void OnNextLevelStarted()
        {
            ClearGridItems();
            Initialize();
        }

        private void OnWordCheckStarted(string word)
        {
            CheckWord(word);
        }

        private IEnumerator OnLevelFinished(LevelDetail levelDetail)
        {
            print("Level " + levelDetail.LevelId + " Finished");
            randomBalloonItems = null;
            yield return null;
        }

        private void OnExtraWordFound(string word)
        {
            levelInformation.FoundExtraWords.Add(word);
            print("Found Extra Word: " + word);
        }

        private void OnWordNotFound(string word)
        {
            print("Word Not Found: " + word);
        }

        private void OnWordFound(string word)
        {
            PlayAudio(audioClips.wordFoundAudioClip);
            WordDetail wordDetail = levelInformation.CurrentLevelDetail.Words.Find(p => p.Word == word);
            ShowFoundWord(wordDetail);
        }

        private void OnFoundWordFound(string word)
        {
            WordDetail wordDetail = levelInformation.CurrentLevelDetail.Words.Find(p => p.Word == word);
            foreach (var item in wordDetailGridItems[wordDetail])
            {
                item.DoAnimation();
            }
        }

        private void AddWordsOnGrid()
        {
            foreach (var wordDetail in levelInformation.CurrentLevelDetail.Words)
            {
                AddLettersOnGrid(wordDetail);
            }
        }

        private void AddLettersOnGrid(WordDetail wordDetail)
        {
            List<GridItem> wordDetailGridItemList = new List<GridItem>();
            for (int i = 0; i < wordDetail.Word.Length; i++)
            {
                int row = GetNextRow(wordDetail, i);
                int column = GetNextColumn(wordDetail, i);
                gridItems[row, column].ParentWordDetails.Add(wordDetail);
                gridItems[row, column].AddLetter(wordDetail.Word[i].ToString());
                wordDetailGridItemList.Add(gridItems[row, column]);

                gridItems[row, column].LetterShown += OnLetterShown;
                gridItems[row, column].LetterShownWithHint += OnLetterShown;
            }
            wordDetailGridItems.Add(wordDetail, wordDetailGridItemList);
            letterGridItemsForRandomSelection.AddRange(wordDetailGridItemList);
        }

        private void OnLetterShown(GridItem gridItem)
        {
            foreach (var wordDetail in gridItem.ParentWordDetails)
            {
                if (CheckWordFound(wordDetail) && false == levelInformation.FoundWords.Contains(wordDetail))
                {
                    levelInformation.FoundWords.Add(wordDetail);
                    print("Word Found: " + wordDetail.Word);
                }
            }
            StartCoroutine(levelInformation.CheckLevelFinished());
        }

        private int GetNextColumn(WordDetail wordDetail, int index)
        {
            return wordDetail.Column + (wordDetail.Orientation == "H" ? index : 0);
        }

        private int GetNextRow(WordDetail wordDetail, int index)
        {
            return wordDetail.Row + (wordDetail.Orientation == "V" ? index : 0);
        }

        private void ShowFoundWord(WordDetail wordDetail)
        {
            foreach (var item in wordDetailGridItems[wordDetail])
            {
                item.ShowLetter();
            }
        }

        private bool CheckWordFound(WordDetail wordDetail)
        {
            foreach (var item in wordDetailGridItems[wordDetail])
            {
                if (false == item.IsLetterVisible)
                    return false;
            }

            return true;
        }

        private void SetRandomBalloons(int count)
        {
            randomBalloonItems = new List<GridItem>();
            while (randomBalloonItems.Count < count)
            {
                int i = Random.Range(0, letterGridItemsForRandomSelection.Count - 1);

                if (false == randomBalloonItems.Contains(letterGridItemsForRandomSelection.ElementAt(i)))
                {
                    randomBalloonItems.Add(letterGridItemsForRandomSelection.ElementAt(i));
                    letterGridItemsForRandomSelection.RemoveAt(i);
                }
            }
        }

        private void PlayAudio(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        private void CheckWord(string word)
        {
            if (levelInformation.FoundWords.Any(p => p.Word == word))
            {
                levelInformation.OnFoundWordFound(word);
            }
            else if (levelInformation.CurrentLevelDetail.Words.Any(p => p.Word == word))
            {
                levelInformation.OnWordFound(word);
            }
            else if (levelInformation.CurrentLevelDetail.OtherWords.Contains(word))
            {
                if (false == levelInformation.FoundExtraWords.Contains(word))
                {
                    levelInformation.OnExtraWordFound(word);
                }
                else
                {
                    levelInformation.OnFoundExtraWordFound(word);
                }
            }
            else
            {
                levelInformation.OnWordNotFound(word);
            }

            levelInformation.OnWordCheckCompleted(word);
        }

        public List<GridItem> GetGridItemsOfWord(WordDetail wordDetail)
        {
            return wordDetailGridItems[wordDetail];
        }
    }
}