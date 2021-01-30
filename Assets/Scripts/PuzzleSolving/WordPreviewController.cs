using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Core.Levels;
using Core.Animation;
using System;

namespace PuzzleSolving
{
    public class WordPreviewController : MonoBehaviour
    {
        [SerializeField] private LevelInformation levelInformation;
        [SerializeField] private GameObject extraWordImage;

        private TMPro.TMP_Text tmpText;
        private bool doShakeRotation;
        private bool doMoveExtraWord;
        private IAnimator shakeAnimator;

        public string word = string.Empty;
        public List<string> letterNames;

        private void Awake()
        {
            tmpText = GetComponentInChildren<TMPro.TMP_Text>();
            shakeAnimator = GetComponent<IAnimator>();
        }

        private void OnEnable()
        {
            levelInformation.WordFound += OnWordFound;
            levelInformation.ExtraWordFound += OnExtraWordFound;
            levelInformation.LevelFinished += OnLevelFinished;
            levelInformation.WordNotFound += OnWordNotFound;
            levelInformation.FoundExtraWordFound += OnFoundExtraWordFound;
            levelInformation.WordCheckCompleted += OnWordCheckCompleted;
            shakeAnimator.AnimationCompleted += new EventHandler(OnAnimationCompleted);
        }

        private void OnAnimationCompleted(object v, object sender)
        {
            ClearText();
        }

        private void OnWordCheckCompleted(string word)
        {
            if (doShakeRotation)
            {
                shakeAnimator.DoAnimation();
                doShakeRotation = false;
            }
            else if (doMoveExtraWord)
            {
                tmpText.transform.DOMove(extraWordImage.transform.position, 0.5f).OnComplete(() =>
                {
                    tmpText.transform.SetParent(transform);
                    tmpText.transform.localPosition = Vector3.zero;
                    ClearText();
                });
                doMoveExtraWord = false;
            }
            else
                ClearText();
        }

        private IEnumerator OnLevelFinished(LevelDetail levelDetail)
        {
            yield return null;
        }

        private void OnExtraWordFound(string word)
        {
            doMoveExtraWord = true;
        }

        private void OnWordFound(string word)
        {
            doShakeRotation = false;
        }

        private void OnWordNotFound(string word)
        {
            doShakeRotation = true;
        }

        private void OnFoundExtraWordFound(string word)
        {
            doShakeRotation = true;
        }

        private void ClearText()
        {
            word = string.Empty;
            tmpText.text = word;
            letterNames.Clear();
        }

        public void AddLetter(string letter, string letterName)
        {
            if (!letterNames.Contains(letterName))
            {
                word += letter;
                tmpText.text = word;
                letterNames.Add(letterName);
            }
        }

        public void DeleteLastLetter()
        {
            if (letterNames.Any())
            {
                word = word.Substring(0,word.Length-1);
                tmpText.text = word;
                letterNames.RemoveAt(letterNames.Count-1);
            }
        }
    }
}