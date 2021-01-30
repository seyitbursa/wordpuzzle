using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Core.Animation
{
    public class ExtraWordAnimator : MonoBehaviour, IAnimator
    {
        public event EventHandler AnimationCompleted;

        public void DoAnimation()
        {
            transform.DOShakeRotation(0.5f, 45).OnComplete(() => UndoAnimation());
        }

        public void UndoAnimation()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}