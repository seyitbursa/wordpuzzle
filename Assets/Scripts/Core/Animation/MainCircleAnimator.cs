using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Animation
{
    public class MainCircleAnimator :MonoBehaviour, IAnimator
    {
        public event EventHandler AnimationCompleted;

        public void DoAnimation()
        {
            transform.DOScale(1, 2);
        }

        public void UndoAnimation()
        {
            transform.DOScale(0, 2);
        }
    }
}