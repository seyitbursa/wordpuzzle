using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Levels
{

    [CreateAssetMenu]
    public class LevelInformation : ScriptableObject
    {
        [SerializeField] private int levelId = 1;
        [SerializeField] private List<Sprite> backgroundImages;

        public Color DefaultColor = Color.white;
        public Color ImageColor = Color.gray;
        public Color TextColor = Color.white;

        public List<LevelDetail> LevelDetails { get; set; }
        public LevelDetail CurrentLevelDetail { get { return LevelDetails.Find(p => p.LevelId == levelId); } private set { } }
        public List<WordDetail> FoundWords { get; set; }
        public List<string> FoundExtraWords { get; set; }
        public Sprite CurrentLevelBackroundImage => backgroundImages[CurrentLevelDetail.SetId - 1];


        public delegate void WordFoundHandler(string word);
        public event WordFoundHandler WordFound;

        public delegate void WordNotFoundHandler(string word);
        public event WordNotFoundHandler WordNotFound;

        public delegate void ExtraWordFoundHandler(string word);
        public event ExtraWordFoundHandler ExtraWordFound;

        public delegate void FoundWordFoundHandler(string word);
        public event FoundWordFoundHandler FoundWordFound;

        public delegate void FoundExtraWordFoundHandler(string word);
        public event FoundExtraWordFoundHandler FoundExtraWordFound;

        public delegate IEnumerator LevelFinishedHandler(LevelDetail levelDetail);
        public event LevelFinishedHandler LevelFinished;

        public delegate void WordCheckStartedHandler(string word);
        public event WordCheckStartedHandler WordCheckStarted;

        public delegate void WordCheckCompletedHandler(string word);
        public event WordCheckCompletedHandler WordCheckCompleted;

        public delegate void NextLevelStartedHandler();
        public event NextLevelStartedHandler NextLevelStarted;

        public void OnWordFound(string word)
        {
            WordFound?.Invoke(word);
        }

        public void OnWordNotFound(string word)
        {
            WordNotFound?.Invoke(word);
        }

        public void OnExtraWordFound(string word)
        {
            ExtraWordFound?.Invoke(word);
        }

        public void OnFoundWordFound(string word)
        {
            FoundWordFound?.Invoke(word);
        }

        public void OnFoundExtraWordFound(string word)
        {
            FoundExtraWordFound?.Invoke(word);
        }

        public IEnumerator OnLevelFinished(LevelDetail levelDetail)
        {
            return LevelFinished?.Invoke(levelDetail);
        }

        public void OnWordCheckStarted(string word)
        {
            WordCheckStarted?.Invoke(word);
        }

        public void OnWordCheckCompleted(string word)
        {
            WordCheckCompleted?.Invoke(word);
        }

        public void OnNextLevelStarted()
        {
            levelId += 1;
            NextLevelStarted?.Invoke();
        }

        public IEnumerator CheckLevelFinished()
        {
            if (FoundWords.Count == CurrentLevelDetail.Words.Count)
            {
                yield return OnLevelFinished(CurrentLevelDetail);
            }
            yield return null;
        }
    }
}