using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Core.Levels;
using Core.Pooling;
using Core.Animation;

namespace PuzzleBuilding
{
    public class GridItem : PoolObject
    {
        [SerializeField] private LevelInformation levelInformation;
        private Image image;
        private TMPro.TMP_Text tmpText;
        private Puzzle puzzle;
        private IAnimator animator;

        public List<WordDetail> ParentWordDetails { get; set; }
        public bool IsLetterVisible { get; private set; }

        public delegate void LetterShownWithHintHandler(GridItem gridItem);
        public event LetterShownWithHintHandler LetterShownWithHint;

        public delegate void LetterShownHandler(GridItem gridItem);
        public event LetterShownHandler LetterShown;

        private void OnEnable()
        {
            objectType = ObjectType.GridItem;
            animator = GetComponent<IAnimator>();
        }

        private void OnLetterShownWithHint(GridItem gridItem)
        {
            LetterShownWithHint?.Invoke(gridItem);
        }

        private void OnLetterShown(GridItem gridItem)
        {
            LetterShown?.Invoke(gridItem);
        }

        public void MakeVisible()
        {
            if (image == null)
                image = GetComponent<Image>();

            image.color = new Color(levelInformation.DefaultColor.r, levelInformation.DefaultColor.g, levelInformation.DefaultColor.b, 100);
        }

        public void MakeInVisible()
        {
            if (image == null)
                image = GetComponent<Image>();

            image.color = new Color(levelInformation.DefaultColor.r, levelInformation.DefaultColor.g, levelInformation.DefaultColor.b, 0);
        }

        public void AddLetter(string letter)
        {
            if (tmpText == null)
                tmpText = GetComponentInChildren<TMPro.TMP_Text>();

            if (puzzle == null)
                puzzle = transform.parent.GetComponent<Puzzle>();

            tmpText.text = letter;
            HideLetter();
            MakeVisible();
        }

        public void ClearLetter()
        {
            if (tmpText == null)
                tmpText = GetComponentInChildren<TMPro.TMP_Text>();

            tmpText.text = string.Empty;
            IsLetterVisible = false;
        }

        public void ShowLetter()
        {
            if (!IsLetterVisible)
            {
                DoAnimation();
                IsLetterVisible = true;
                image.color = levelInformation.ImageColor;
                tmpText.color = levelInformation.TextColor;
                OnLetterShown(this);
            }
        }

        public void DoAnimation()
        {
            animator.DoAnimation();
        }

        public void ShowLetterWithHint()
        {
            if (!IsLetterVisible)
            {
                DoAnimation();
                IsLetterVisible = true;
                image.color = levelInformation.DefaultColor;
                tmpText.color = levelInformation.ImageColor;
                OnLetterShownWithHint(this);
            }
        }

        public void HideLetter()
        {
            tmpText.color = new Color(levelInformation.DefaultColor.r, levelInformation.DefaultColor.g, levelInformation.DefaultColor.b, 0);
        }
    }
}