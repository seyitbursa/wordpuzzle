using System.Collections.Generic;
using Core.Animation;
using Core.Levels;
using DG.Tweening;
using UnityEngine;

namespace PuzzleSolving
{
    public class MainCircle : MonoBehaviour
    {
        private IAnimator animator;

        private void Awake()
        {
            animator = GetComponent<IAnimator>();
        }

        private void Start()
        {
            animator.DoAnimation();
        }
    }
}