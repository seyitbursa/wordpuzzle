using System;
using Core.Levels;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Animation
{
    public class LetterEnterAnimator : MonoBehaviour, IAnimator
    {
        private Image image;
        private TMP_Text textMesh;

        public event EventHandler AnimationCompleted;

        private void OnEnable()
        {
            textMesh = GetComponentInChildren<TMP_Text>();
            image = GetComponentInChildren<Image>();
        }

        public void DoAnimation()
        {
            DOTween.Sequence().Append(transform.DOScale(new Vector3(1.1f, 1.1f, 0), 0.5f).SetEase(Ease.Linear)).Append(transform.DOScale(new Vector3(1f, 1f, 0), 0.5f).SetEase(Ease.Linear));
            image.DOColor(Color.gray, 0.5f);
            textMesh.DOColor(Color.white, 0.5f);
        }

        public void UndoAnimation()
        {
            image.DOColor(new Color(1, 1, 1, 0), 0.5f);
            textMesh.DOColor(Color.gray, 0.5f);
        }
    }
}