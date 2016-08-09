using UnityEngine;
using System.Collections;
using System;

namespace Infection
{
    public abstract class GameElement : MonoBehaviour
    {
        [SerializeField]
        protected float speed;
        [SerializeField]
        public float live{ get;  protected set; }
        [SerializeField]
        public float atack { get;  protected set; }

        public delegate void DestroyEventElement(GameElement element);
        public event DestroyEventElement OnDestroyed;//Event is a delegate type and it will call width a delegate param

        public void DoHealth(float atack)
        {
            if(state == State.Run && live > 0)
            {
                live -= atack;
                if(live <= 0)
                {
                    DoDestroy();
                }
            }
        }

        public GameElement opponent { get; protected set; }

        public enum State { Init, Run, Death};
        public State state;//{ get;  protected set; }

        protected Vector2 lastPosition;
        protected float angle;

        public virtual void Awake()
        {
        }

        public virtual void Init(Vector2 position)
        {
            this.gameObject.SetActive(true);
            this.transform.position = position;
            this.lastPosition = transform.position;
            this.opponent = null;
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

        public abstract void DoDestroy();

        protected void Destroy()
        {
            this.gameObject.SetActive(false);
            if (OnDestroyed != null)
            {
                OnDestroyed(this);
            }
        }

        protected bool IsInGameArea(Vector2 position, float margin)
        {
            return GameManager.GetInstance.
                GetRectGameDimension(margin).Contains(position);
        }

        protected void ChangeDirection(float randomDesviation)
        {
            transform.position = lastPosition;
            angle += (180 + UnityEngine.Random.Range(-randomDesviation, randomDesviation)) % 360;
        }

    }
}

