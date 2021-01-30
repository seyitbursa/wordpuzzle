using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Animation
{
    public interface IAnimator
    {
        event EventHandler AnimationCompleted;
        void DoAnimation();
        void UndoAnimation();
    }

}
