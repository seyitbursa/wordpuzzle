using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Animation
{
    public class WordPreviewShakeAnimator : MonoBehaviour, IAnimator
    {
        public event EventHandler AnimationCompleted;

        public void DoAnimation()
        {
            DOTween.Sequence().Append(transform.DOShakeScale(0.1f).SetEase(Ease.Linear))
                                  .Append(transform.DOScale(new Vector3(1f, 1f, 0), 0.1f))
                                  .OnComplete(()=>AnimationCompleted?.Invoke(this,null));
                      
        }

        public void UndoAnimation()
        {
        }
    }
}