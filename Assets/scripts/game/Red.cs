using UnityEngine;
using System.Collections;
using System;

namespace Infection
{

    public class Red : GameElement, IPlayable
    {
        public enum SubState { Walk, Fight };
        public SubState subState;

        private bool freeMovement;

        public override void Init(Vector2 position)
        {
            base.Init(position);
            this.live = Constants.RED_LIVE;
            this.freeMovement = false;
            /*Angle to center*/
            Vector2 diference = Camera.main.transform.position - transform.position;
            float sign = (Camera.main.transform.position.y < transform.position.y) ? -1.0f : 1.0f;
            this.angle = Vector2.Angle(Vector2.right, diference) * sign;

            this.angle += UnityEngine.Random.Range(-20, 20);
            this.subState = SubState.Walk;
        }

        protected override void Update()
        {
            base.Update();
            if (state == State.Run)
            {
                switch (subState)
                {
                    case SubState.Walk:
                        if (freeMovement && !IsInGameArea(transform.position, Constants.GAME_AREA_MARGIN))
                        {
                            ChangeDirection(40);
                        }
                        lastPosition = transform.position;
                        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                        transform.Translate(direction * speed * Time.deltaTime, Space.World);
                        CheckFreeMovement();
                        break;
                    case SubState.Fight:
                        transform.position = lastPosition;
                        transform.position = new Vector2(transform.position.x + UnityEngine.Random.Range(-0.02f, 0.02f), transform.position.y + UnityEngine.Random.Range(-0.02f, 0.02f));
                        if (opponent != null && opponent.state != State.Death)
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

        private void CheckFreeMovement()
        {
            if(!freeMovement)
            {
                if (IsInGameArea(transform.position, Constants.GAME_AREA_MARGIN))
                {
                    freeMovement = true;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //TODO: Es una mierda, pero de momento sirve
            if (state == State.Run)
            {
                if (subState == SubState.Walk)
                {
                    if (other.GetComponent<Red>())
                    {
                        ChangeDirection(0);
                    }
                    else if(other.GetComponent<Green>())
                    {
                        transform.position = lastPosition;
                        IPlayable p = other.GetComponent<IPlayable>();
                        Fight(p);
                        p.Fight(this);
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
                other.Fight(this);
            }
        }

        public void SetOponent(IPlayable other)
        {
            this.opponent = (GameElement)other;
        }

        public override void DoDestroy()
        {
            if (state != State.Death)
            {
                state = State.Death;
                StartCoroutine(Scale(this.transform, 0.5f, new Vector2(0, 0), () => Destroy()));
            }
        }
    }
}
