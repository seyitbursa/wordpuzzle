using System;
using System.Collections.Generic;
using System.Linq;
using Core.Animation;
using Core.Levels;
using Core.LineDrawing;
using Core.Pooling;
using UnityEngine;

namespace PuzzleSolving
{
    public class Letters : MonoBehaviour
    {
        [SerializeField] private LevelInformation levelInformation;
        [SerializeField] private ObjectPoolController objectPoolController;
        [SerializeField] private MainCircle mainCircle;

        private RectTransform rectTransform;
        private List<Letter> letters;
        private IAnimator animator;
        private ILineDrawer lineDrawer;
        private bool isPointerDown;
        private bool isPointerEntered;
        private string mainWord;
        private float circleRadius;
        private float xPosition;
        private float yPosition;

        public WordPreviewController wordPreviewController;
        public List<Letter> enteredLetters;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            animator = GetComponent<IAnimator>();
            lineDrawer = GetComponent<ILineDrawer>();
            enteredLetters = new List<Letter>();
            letters = new List<Letter>();
            circleRadius = rectTransform.rect.height / 2;
        }

        private void OnEnable()
        {
            levelInformation.NextLevelStarted += OnNextLevelStarted;
            levelInformation.WordCheckStarted += OnWordCheckStarted;
        }

        private void Start()
        {
            Initialize();
            animator.DoAnimation();
            xPosition = transform.position.x;
            yPosition = transform.position.y;
        }

        private void Update()
        {
            if (isPointerDown && !isPointerEntered && enteredLetters.Count > 0 && enteredLetters.Count < levelInformation.CurrentLevelDetail.Letters.Length)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (IsPointInsideCircle(mousePosition))
                {
                    lineDrawer.AddLinePoint(new Vector3(mousePosition.x, mousePosition.y, 0),true);
                }
            }
        }

        private void Initialize()
        {
            mainWord = levelInformation.CurrentLevelDetail.Letters;
            float distanceFromMainCircleCenter = 0;
            for (int i = 0; i < mainWord.Length; i++)
            {
                GameObject letterObject = objectPoolController.GetObject(ObjectType.Letter);
                if (distanceFromMainCircleCenter == 0)
                    distanceFromMainCircleCenter = (rectTransform.rect.height - letterObject.GetComponent<RectTransform>().rect.height) / 2;

                int posx = (int)(distanceFromMainCircleCenter * Math.Cos(2 * Math.PI * i / (mainWord.Length)));
                int posy = (int)(distanceFromMainCircleCenter * Math.Sin(2 * Math.PI * i / (mainWord.Length)));
                Vector3 localPosition = new Vector3(posx, posy, 0);
                letterObject.transform.localPosition = localPosition;
                letterObject.transform.localScale = Vector3.zero;

                Letter letter = letterObject.GetComponent<Letter>();
                letter.SetLetter(mainWord[i].ToString(), "Letter-" + i);
                letter.PointerIsUp += OnPointerUp;
                letter.PointerIsDown += OnPointerDown;
                letter.PointerIsEntered += OnPointerEnter;
                letter.PointerIsExited += OnPointerExit;
                letters.Add(letter);
            }
        }

        private void OnPointerExit(Letter letter)
        {
            isPointerEntered = false;
            if(false == isPointerDown)
                letter.UndoEnterAnimation();
        }

        private void OnPointerEnter(Letter letter, string letterText, string letterTextMeshName)
        {
            isPointerEntered = true;
            if (isPointerDown)
            {
                if (false == enteredLetters.Contains(letter))
                { 
                    if (wordPreviewController.word.Length < levelInformation.CurrentLevelDetail.Letters.Length)
                    {
                        AddEnteredLetter(letter, letterText, letterTextMeshName);
                        lineDrawer.AddLinePoint(letter.transform.position,false);
                    }
                }
                else
                {
                    lineDrawer.RemoveTempLine();
                    if (enteredLetters.Count>1 && enteredLetters[enteredLetters.Count-2] == letter)
                    {
                        enteredLetters.Last().UndoEnterAnimation();
                        DeleteLastEnteredLetter();
                        lineDrawer.RemoveLastLine();
                    }
                }
            }
        }

        private void OnPointerDown(Letter letter,string letterText,string letterTextMeshName)
        {
            if (!isPointerDown &&  !string.IsNullOrWhiteSpace(letterText) && string.IsNullOrWhiteSpace(wordPreviewController.word))
            {
                AddEnteredLetter(letter, letterText, letterTextMeshName);
                lineDrawer.AddLinePoint(letter.transform.position,false);
            }

            isPointerDown = true;
        }

        private void OnPointerUp()
        {
            if (isPointerDown && !string.IsNullOrWhiteSpace(wordPreviewController.word))
                levelInformation.OnWordCheckStarted(wordPreviewController.word);

            isPointerDown = false;
        }

        private void OnNextLevelStarted()
        {
            ResetObjects();
            Initialize();
            animator.DoAnimation();
        }

        private void OnWordCheckStarted(string word)
        {
            UndoLetterAnimations();
            enteredLetters.Clear();
            lineDrawer.ClearLinePoints();
        }

        private void ResetObjects()
        {
            foreach (Letter letter in letters)
            {
                objectPoolController.ResetObject(letter.gameObject);
            }
            letters.Clear();
        }

        private void UndoLetterAnimations()
        {
            foreach (var item in enteredLetters)
            {
                item.UndoEnterAnimation();
            }
        }

        private void AddEnteredLetter(Letter letter, string letterText, string letterTextMeshName)
        {
            wordPreviewController.AddLetter(letterText, letterTextMeshName);
            enteredLetters.Add(letter);
        }

        private void DeleteLastEnteredLetter()
        {
            wordPreviewController.DeleteLastLetter();
            enteredLetters.RemoveAt(enteredLetters.Count-1);
        }

        private bool IsPointInsideCircle(Vector3 point)
        {
            float xDistance = Math.Abs(point.x - xPosition);
            float yDistance = Math.Abs(point.y - yPosition);
            if (xDistance * xDistance + yDistance * yDistance <= circleRadius * circleRadius)
                return true;
            else
                return false;
        }
    }
}