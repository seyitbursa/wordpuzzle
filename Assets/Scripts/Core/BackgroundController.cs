using Core.Levels;
using UnityEngine;

namespace Core
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] LevelInformation levelInformation;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            SetBackground();
        }

        private void OnEnable()
        {
            levelInformation.NextLevelStarted += LevelInformation_NextLevelStarted; ;
        }

        private void LevelInformation_NextLevelStarted()
        {
            SetBackground();
        }

        private void SetBackground()
        {
            if (spriteRenderer.sprite != levelInformation.CurrentLevelBackroundImage)
            {
                spriteRenderer.sprite = levelInformation.CurrentLevelBackroundImage;

                float cameraHeight = Camera.main.orthographicSize * 2;
                Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
                Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

                transform.localScale = Vector3.one;
                Vector2 scale = transform.localScale;
                scale *= cameraSize.x >= cameraSize.y ? cameraSize.x / spriteSize.x : cameraSize.y / spriteSize.y;
                transform.localScale = scale;
            }
        }

    }
}
