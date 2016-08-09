using UnityEngine;
using System.Collections;
using System;

namespace Infection
{
    
    public class Green : GameElement, IPlayable
    {
        [SerializeField]
        private float minWalkDuration = 3;
        [SerializeField]
        private float maxWalkDuration = 10;
        [SerializeField]
        private float minIdleDuration = 1;
        [SerializeField]
        private float maxIdleDuration = 3;

        private float subStateCount;
        private float subStateTime;
        public enum SubState {Walk, Idle, Fight};
        public SubState subState;

        public override void Init(Vector2 position)
        {
            base.Init(position);
            this.subState = SubState.Walk;
            this.live = Constants.GREEN_LIVE;
            this.subStateCount = 0;
            this.subStateTime = UnityEngine.Random.Range(minWalkDuration, maxWalkDuration);
        }

        protected override void Update()
        {
            base.Update();
            if(state == State.Run)
            {
                subStateCount += Time.deltaTime;
                switch(subState)
                {
                    case SubState.Idle:
                        if(subStateCount > subStateTime)
                        {
                            subStateCount = 0;
                            subStateTime = UnityEngine.Random.Range(minWalkDuration, maxWalkDuration);
                            subState = SubState.Walk;
                        }
                        break;
                    case SubState.Walk:
                        if (!IsInGameArea(transform.position, Constants.GAME_AREA_MARGIN_ACTION_ZONE))
                        {
                            ChangeDirection(40);
                        }
                        lastPosition = transform.position;
                        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                        transform.Translate(direction * speed * Time.deltaTime, Space.World);
                        if (subStateCount > subStateTime)
                        {
                            subStateCount = 0;
                            subStateTime = UnityEngine.Random.Range(minIdleDuration, maxIdleDuration);
                            subState = SubState.Idle;
                        }
                        break;
                    case SubState.Fight:
                        transform.position = lastPosition;
                        transform.position = new Vector2(transform.position.x + UnityEngine.Random.Range(-0.02f, 0.02f), transform.position.y + UnityEngine.Random.Range(-0.02f, 0.02f));
                        if(opponent != null && opponent.state != State.Death)
                        {
                            opponent.DoHealth(Constants.GREEN_ATACK);
                        }
                        else
                        {
                            subState = SubState.Walk;
                            opponent = null;
                        }
                        break;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //TODO: Es una mierda, pero de momento sirve
            if (state == State.Run)
            {
                if(subState == SubState.Walk)
                {
                    if (other.GetComponent<Green>())
                    {
                        ChangeDirection(0);
                    }
                }
            }
        }

        public void Fight(IPlayable other)
        {
            if (state == State.Run)
            {
                this.subState = SubState.Fight;
                SetOponent(other);
            }
        }

        public void SetOponent(IPlayable other)
        {
            this.opponent = (GameElement)other;
        }

        public override void DoDestroy()
        {
            if(state != State.Death)
            {
                state = State.Death;
                StartCoroutine(Scale(this.transform, 0.5f, new Vector2(0,0), () => Destroy()));
            }
        }

        
    }
}
