using UnityEngine;
using System.Collections;
using System;

namespace Infection
{

    public class Red : GameElement, IPlayable
    {
        public enum SubState { Walk, Fight };
        public SubState subState;

        public override void Init(Vector2 position)
        {
            base.Init(position);
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
                        lastPosition = transform.position;
                        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                        transform.Translate(direction * speed * Time.deltaTime, Space.World);
                        break;
                    case SubState.Fight:
                        transform.position = lastPosition;
                        transform.position = new Vector2(transform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f), transform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f));
                        break;
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
                        ChangeDirection();
                    }
                    else if(other.GetComponent<Green>())
                    {
                        IPlayable p = other.GetComponent<IPlayable>();
                        Fight(p);
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
