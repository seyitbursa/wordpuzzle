using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace LevelPassing
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private LevelInformation levelInformation;
        [SerializeField] private Image mask;
        [SerializeField] private TMPro.TMP_Text progressText;

        public IEnumerator FillProgressBarCoroutine()
        { //todo: DOTWEEN
            progressText.text = string.Empty;
            mask.fillAmount = 0f;

            List<LevelDetail> levelDetailsInSet = levelInformation.LevelDetails.FindAll(p => p.SetId == levelInformation.CurrentLevelDetail.SetId);
            int maxLevelIdInSet = levelDetailsInSet.Max(p => p.LevelId);
            int minLevelIdInSet = levelDetailsInSet.Min(p => p.LevelId);
            int levelIndexInSet = levelInformation.CurrentLevelDetail.LevelId - minLevelIdInSet + 1;
            int levelCountInSet = maxLevelIdInSet - minLevelIdInSet + 1;
            float fillAmount = (float)levelIndexInSet / (float)levelCountInSet;
            float duration = 2f;
            float elapsedTime = 0;
            while (mask.fillAmount < fillAmount)
            {
                mask.fillAmount = Mathf.Lerp(0f, fillAmount, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            progressText.text = "(" + levelIndexInSet + " / " + levelCountInSet + ")";
        }
    }
}