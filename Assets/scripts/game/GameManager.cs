using UnityEngine;
using System.Collections;

namespace Infection
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer gameArea;
        [SerializeField]
        private LevelSpawner levelSpawner;
        
        public bool isGameRunning { get; private set; }
        public float gameWidth { get; private set; }
        public float gameHeight { get; private set; }

        private static GameManager gameManager;
        public static GameManager GetInstance
        {
            get
            {
                if (gameManager == null)
                {
                    gameManager = GameObject.FindObjectOfType<GameManager>();
                }
                return gameManager;
            }
        }

        void Awake()
        {
            gameWidth = gameArea.bounds.max.x - gameArea.bounds.min.x;
            gameHeight = gameArea.bounds.max.y - gameArea.bounds.min.y;
            StartGame();
        }

        void StartGame()
        {
            this.isGameRunning = true;
            this.gameObject.SetActive(true);
            this.levelSpawner.Init();
        }

        void Update()
        {
            Rect r = GetRectGameDimension(Constants.GAME_AREA_MARGIN);
            Debug.DrawLine(new Vector3(r.xMin, r.yMax, 0), new Vector3(r.xMax, r.yMax, 0), Color.white);//Top
            Debug.DrawLine(new Vector3(r.xMin, r.yMax, 0), new Vector3(r.xMin, r.yMin, 0), Color.white);//Left
            Debug.DrawLine(new Vector3(r.xMax, r.yMax, 0), new Vector3(r.xMax, r.yMin, 0), Color.white);//Right
            Debug.DrawLine(new Vector3(r.xMin, r.yMin, 0), new Vector3(r.xMax, r.yMin, 0), Color.white);//Button

            r = GetRectGameDimension(Constants.GAME_AREA_MARGIN_ACTION_ZONE);
            Debug.DrawLine(new Vector3(r.xMin, r.yMax, 0), new Vector3(r.xMax, r.yMax, 0), Color.magenta);//Top
            Debug.DrawLine(new Vector3(r.xMin, r.yMax, 0), new Vector3(r.xMin, r.yMin, 0), Color.magenta);//Left
            Debug.DrawLine(new Vector3(r.xMax, r.yMax, 0), new Vector3(r.xMax, r.yMin, 0), Color.magenta);//Right
            Debug.DrawLine(new Vector3(r.xMin, r.yMin, 0), new Vector3(r.xMax, r.yMin, 0), Color.magenta);//Button
        }

        public Rect GetRectGameDimension(float margin)
        {
            Vector2 topLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
            /*
            Vector2 bottomRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            Rect r = new Rect(
                topLeft.x, 
                topLeft.y, 
                bottomRight.x - topLeft.x - Constants.MARGIN*2 - distanceMod*2, 
                bottomRight.y - topLeft.y - Constants.MARGIN - Constants.MARGIN_TOP - distanceMod*2);
                float offset = Constants.MARGIN + distanceMod;
            r.position = new Vector2(r.position.x + offset, r.position.y + offset);
            */

            Rect r = new Rect(
                -gameWidth / 2 + margin,
                -gameHeight / 2 + margin,
                gameWidth - margin * 2,
                gameHeight - margin * 2);
            return r;
        }
    }
}
