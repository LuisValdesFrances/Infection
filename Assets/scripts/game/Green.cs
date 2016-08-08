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




                        if (!IsInGameArea(transform.position))
                        {
                            ChangeDirection();
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
                        transform.position = new Vector2(transform.position.x + UnityEngine.Random.Range(-0.05f, 0.05f), transform.position.x + UnityEngine.Random.Range(-0.05f, 0.05f));
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
                        ChangeDirection();
                    }
                }
            }
        }

        public void Fight(IPlayable other)
        {
            if (state == State.Run)
            {
                this.subState = SubState.Fight;
            }
        }
    }
}
