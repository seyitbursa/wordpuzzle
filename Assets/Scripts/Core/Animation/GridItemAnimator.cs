using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Animation
{
    public class GridItemAnimator : MonoBehaviour, IAnimator
    {
        public event EventHandler AnimationCompleted;

        public void DoAnimation()
        {
            DOTween.Sequence().Append(transform.DOScale(new Vector3(0.8f, 0.8f, 0), 0.5f).SetEase(Ease.Linear)).Append(transform.DOScale(new Vector3(1f, 1f, 0), 0.5f).SetEase(Ease.Linear));
        }

        public void UndoAnimation()
        {
            transform.localScale = new Vector3(1f, 1f, 0);
        }
    }
}