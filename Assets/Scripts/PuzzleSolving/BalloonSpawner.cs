using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Core.Levels;
using PuzzleBuilding;
using Core.Pooling;

namespace PuzzleSolving
{

    public class BalloonSpawner : MonoBehaviour
    {
        [SerializeField] private LevelInformation levelInformation;
        [SerializeField] private ObjectPoolController objectPoolController;
        [SerializeField] private Puzzle puzzle;

        private List<GameObject> balloons;

        private void OnEnable()
        {
            levelInformation.LevelFinished += OnLevelFinished;
            levelInformation.NextLevelStarted += OnNextLevelStarted;
        }

        private void Start()
        {
            SpawnBalloons(3);
        }

        private void SpawnBalloons(int duration)
        {
            Canvas.ForceUpdateCanvases();
            balloons = new List<GameObject>();
            for (int i = 0; i < puzzle.randomBalloonItems.Count; i++)
            {
                GameObject balloonObject = objectPoolController.GetObject(ObjectType.Balloon);
                balloons.Add(balloonObject);
                balloonObject.transform.DOMove(puzzle.randomBalloonItems[i].transform.position, duration).OnComplete(() => OnDoMoveComplete());
            }
        }

        private void OnDoMoveComplete()
        {
            for (int i = 0; i < balloons.Count; i++)
            {
                objectPoolController.ResetObject(balloons[i]);
                puzzle.randomBalloonItems[i].ShowLetterWithHint();
            }
        }

        private IEnumerator SpawnBalloonsCoroutine()
        {
            yield return new WaitForSeconds(1);
            SpawnBalloons(2);
        }

        private IEnumerator OnLevelFinished(LevelDetail levelDetail)
        {
            yield return null;
        }

        private void OnNextLevelStarted()
        {
            StartCoroutine(SpawnBalloonsCoroutine());
        }
    }
}
