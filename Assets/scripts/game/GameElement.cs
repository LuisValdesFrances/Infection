using UnityEngine;
using System.Collections;
using System;

namespace Infection
{
    public class GameElement : MonoBehaviour
    {
        [SerializeField]
        protected float speed;

        public enum State { Init, Run, Dead};
        public State state;//{ get;  protected set; }

        protected Vector2 lastPosition;
        protected float angle;

        public virtual void Awake()
        {
        }

        public virtual void Init(Vector2 position)
        {
            this.transform.position = position;
            this.lastPosition = transform.position;
            this.state = State.Init;
            this.transform.localScale = new Vector2(0, 0);
            this.StartInitAnimation(transform, 0.5f);
            
        }

        protected virtual void Update()
        {
            switch (state)
            {
                case State.Init:
                    break;
                case State.Run:
                    break;
            }
        }

        private void StartInitAnimation(Transform transform, float time)
        {
            StartCoroutine(Scale(transform, time*0.5f, new Vector2(1.2f, 1.2f), 
                ()=> StartCoroutine(Scale(transform, time*0.5f, new Vector2(1f, 1f), 
                    ()=> state = State.Run))));
        }

        protected IEnumerator Scale(Transform transform, float time, Vector2 targetScale, Action callback)
        {
            float interpolationTime = 0;
            Vector2 startScale = transform.localScale;
            while (interpolationTime < 1f)
            {
                float rate = 1f / time;
                interpolationTime += Time.deltaTime * rate;
                transform.localScale = Vector2.Lerp(startScale, targetScale, interpolationTime);
                yield return null;
            }
            transform.localScale = targetScale;
            if (callback != null)
            {
                callback();
            }
        }

        protected bool IsInGameArea(Vector2 position)
        {
            return GameManager.GetInstance.
                GetRectGameDimension(Constants.GAME_AREA_MARGIN).Contains(position);
        }

        protected void ChangeDirection()
        {
            transform.position = lastPosition;
            angle += (180 + UnityEngine.Random.Range(-45, 45)) % 360;
        }

    }
}

