using System.Collections;
using UnityEngine;
using DG.Tweening;
using Core.Levels;

namespace LevelPassing
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelInformation levelInformation;
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private TMPro.TMP_Text congratulationText;
        [SerializeField] private GameObject uiRoot;

        private CanvasGroup uiCanvasGroup;
        private CanvasGroup levelCanvasGroup;

        private void Awake()
        {
            levelCanvasGroup = GetComponent<CanvasGroup>();
            uiCanvasGroup = uiRoot.GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            levelInformation.LevelFinished += OnLevelFinished;
            SetVisibility(false);
        }

        private IEnumerator OnLevelFinished(LevelDetail levelDetail)
        {
            yield return new WaitForSeconds(0.5f);
            SetVisibility(true);
            DoAnimation();
        }

        private void SetVisibility(bool visible)
        {
            levelCanvasGroup.alpha = visible ? 1 : 0;
            uiCanvasGroup.alpha = visible ? 0 : 1;
        }

        private void DoAnimation()
        {
            congratulationText.DOBlendableColor(Color.gray, 3);
            StartCoroutine(progressBar.FillProgressBarCoroutine());
        }

        public void StartNextLevel()
        {
            SetVisibility(false);
            levelInformation.OnNextLevelStarted();
        }
    }
}