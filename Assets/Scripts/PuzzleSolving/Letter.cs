using System.Collections.Generic;
using Core.Animation;
using Core.Pooling;
using UnityEngine;

namespace PuzzleSolving
{
    public class Letter : PoolObject
    {
        private TMPro.TMP_Text letterTextMesh;
        private string letterText;
        private IAnimator enterAnimator;

        public delegate void PointerUpHandler();
        public event PointerUpHandler PointerIsUp;

        public delegate void PointerDownHandler(Letter letter, string letterText, string letterTextMeshName);
        public event PointerDownHandler PointerIsDown;

        public delegate void PointerEnterHandler(Letter letter, string letterText, string letterTextMeshName);
        public event PointerEnterHandler PointerIsEntered;

        public delegate void PointerExitHandler(Letter letter);
        public event PointerExitHandler PointerIsExited;

        private void OnEnable()
        {
            objectType = ObjectType.Letter;
            enterAnimator = GetComponent<IAnimator>();
        }

        public void UndoEnterAnimation()
        {
            enterAnimator.UndoAnimation();
        }

        public void SetLetter(string letter, string name)
        {
            letterTextMesh = GetComponentInChildren<TMPro.TMP_Text>();
            letterTextMesh.name = name;
            letterTextMesh.text = letter;
            letterText = letter;
        }

        public void OnPointerUp()
        {
            UndoEnterAnimation();
            PointerIsUp?.Invoke();
        }

        public void OnPointerDown()
        {
            enterAnimator.DoAnimation();
            PointerIsDown?.Invoke(this,letterText,letterTextMesh.name);            
        }

        public void OnPointerEnter()
        {
            enterAnimator.DoAnimation();
            PointerIsEntered?.Invoke(this, letterText, letterTextMesh.name);
        }

        public void OnPointerExit()
        {
            PointerIsExited?.Invoke(this);
        }
    }
}