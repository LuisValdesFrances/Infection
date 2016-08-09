using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Infection
{
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameElement green;
        [SerializeField]
        private GameElement red;

        private Queue<GameElement> listGreen;
        private Queue<GameElement> listRed;

        void Awake()
        {
            listGreen = new Queue<GameElement>();
            listRed = new Queue<GameElement>();
        }

        public void Init()
        {
            StartCoroutine(SpawnGreens());
            StartCoroutine(SpawnReds());
        }

        private IEnumerator SpawnGreens()
        {
            while (GameManager.GetInstance.isGameRunning)
            {
                InstanceGreen(new Vector2(0, 0));
                yield return new WaitForSeconds(3);
            }
        }

        private IEnumerator SpawnReds()
        {
            while (GameManager.GetInstance.isGameRunning)
            {
                InstanceRed(GetRandomPositionInMargin());
                yield return new WaitForSeconds(3);
            }
        }

        private Vector2 GetRandomPositionInMargin()
        {
            Vector2 p = Vector2.zero;
            
            p.y = UnityEngine.Random.Range(-GameManager.GetInstance.gameHeight/2, GameManager.GetInstance.gameHeight/2);
            if(p.y < -GameManager.GetInstance.gameHeight / 2 + Constants.GAME_AREA_MARGIN || p.y > GameManager.GetInstance.gameHeight/2 - Constants.GAME_AREA_MARGIN)
            {
                p.x = UnityEngine.Random.Range(-GameManager.GetInstance.gameWidth / 2, GameManager.GetInstance.gameWidth/2);
            }
            else
            {
                int r = UnityEngine.Random.Range(0, 2);
                if(r == 0)
                {
                    p.x = -GameManager.GetInstance.gameWidth/2;
                }
                else
                {
                    p.x = GameManager.GetInstance.gameWidth/2;
                }
            }
            return p;
        }

        public void InstanceGreen(Vector2 position)
        {
            GameElement g;
            if (listGreen.Count == 0)
            {
                g = UnityEngine.Object.Instantiate<GameElement>(green);
            }
            else
            {
                g = listGreen.Dequeue();
            }
            g.OnDestroyed += HideGameElement;
            g.Init(position);
        }

        public void InstanceRed(Vector2 position)
        {
            GameElement g;
            if (listRed.Count == 0)
            {
                g = UnityEngine.Object.Instantiate<GameElement>(red);
            }
            else
            {
                g = listRed.Dequeue();
            }
            g.OnDestroyed += HideGameElement;
            g.Init(position);
        }

        public void HideGameElement(GameElement element)
        {
            element.OnDestroyed -= HideGameElement;
            if (element.GetComponent<Green>())
                listGreen.Enqueue(element);
            else if (element.GetComponent<Red>())
                listRed.Enqueue(element);
        }
    }
}
