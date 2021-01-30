using System.Collections;
using Core.Levels;
using UnityEngine;

namespace LevelPassing
{
    public class LevelHud : MonoBehaviour
    {
        [SerializeField] private LevelInformation levelInformation;
        private TMPro.TMP_Text tmpText;

        private void Awake()
        {
            tmpText = GetComponent<TMPro.TMP_Text>();

        }

        private void Start()
        {
            levelInformation.LevelFinished += OnLevelFinished;
            levelInformation.NextLevelStarted += OnNextLevelStarted;
            UpdateLevelText();
        }

        private void OnNextLevelStarted()
        {
            UpdateLevelText();
        }

        private IEnumerator OnLevelFinished(LevelDetail levelDetail)
        {
            yield return null;
            tmpText.text = string.Empty;
        }

        private void UpdateLevelText()
        {
            tmpText.text = "LEVEL " + levelInformation.CurrentLevelDetail.LevelId;
        }
    }
}