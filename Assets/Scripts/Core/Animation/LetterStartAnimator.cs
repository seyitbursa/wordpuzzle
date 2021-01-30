using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Core.Animation
{
    public class LetterStartAnimator : MonoBehaviour, IAnimator
    {
        public event EventHandler AnimationCompleted;

        public void DoAnimation()
        {
            foreach (Transform item in transform)
            {
                item.DOScale(1, 3);
            }
        }

        public void UndoAnimation()
        {
            foreach (Transform item in transform)
            {
                item.DOScale(0, 3);
            }
        }
    }
}