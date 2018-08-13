using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BallPool.Mechanics;

namespace BallPool
{
    public class BallListener : MonoBehaviour
    {
		public int id;

        public dans_GameManager gameManager;

		private Ball[] balls;
		private Ball attachedBall;

        public Rigidbody body;

        public PocketListener pocket{ get; set; }

        public PhysicsManager physicsManager;

        public float radius{ get; private set; }

        public int pocketId{ get; set; }

        public int hitShapeId{ get; set; }

        public Vector3 normalizedVelocity{ get { return body.velocity / physicsManager.ballMaxVelocity; } }

        private bool firstHit = false;
        private bool inMove;

        void OnCollisionEnter(Collision other)
        {
    //        if (physicsManager.inMove)
    //        {
    //            firstHit = true;
    //        }
    //        if (!firstHit && other.gameObject.layer == LayerMask.NameToLayer("Cloth"))
    //        {
    //            firstHit = true;
				//if (id != 0 && id != 1)
            //    {
            //        //body.Sleep();
            //    }
            //}
        }

        public void OnTriggerEnter(Collider other)
        {
            if (BallPoolGameLogic.playMode == GamePlayMode.Replay || BallPoolGameLogic.controlFromNetwork)
            {
                return;
            }
            PocketListener pocket = other.GetComponent<PocketListener>();
            if (pocket)
            {
                OnEnterPocket(pocket);
            }
        }

        public void OnEnterPocket(PocketListener pocket)
        {
            if (!body.isKinematic)
            {
                //body.isKinematic = true;
				//gameObject.SetActive(false);
				//attachedBall.gameObject.SetActive (false);
                pocketId = pocket.id;
                hitShapeId = -2;
                Debug.Log(id + " OnEnterPocket");
                physicsManager.CallOnBallHitPocket(this, pocket, true);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            //if (BallPoolGameLogic.playMode == GamePlayMode.Replay || BallPoolGameLogic.controlFromNetwork)
            //{
            //    return;
            //}
            //BallListener ball = other.GetComponent<BallListener>();

            //if (ball)
            //{
            //    OnHitBall(ball);
            //}
            //else if (other.gameObject.layer == LayerMask.NameToLayer("Board"))
            //{
            //    OnHitBoard();
            //}
        }

        public void OnHitBall(BallListener ball)
        {
            //Debug.Log("BallListener OnHitBall");
            //pocketId = -1;
            //hitShapeId = ball.id;
            //physicsManager.CallBallHitBall(this, ball, true);
        }

        public void OnHitBoard()
        {
            //Debug.Log("BallListener OnHitBoard");
            //pocketId = -1;
            //hitShapeId = -1;
            //physicsManager.CallBallHitBoard(this, true);
        }

        void Awake()
        {
            radius = body.GetComponent<SphereCollider>().radius;
			balls = gameManager.balls;
			attachedBall = balls [id];
            transform.position = attachedBall.transform.position;
            pocketId = -1;
            hitShapeId = -2;
            inMove = physicsManager.inMove;
        }

        void FixedUpdate()
        {
			//if (!body.isKinematic && !body.IsSleeping () && physicsManager.inMove) {
			//	physicsManager.CallBallMove (id, body.position, body.velocity, body.angularVelocity);
			//} else {
			//	body.Sleep ();
			//}
            //if (inMove != physicsManager.inMove)
            //{
            //    inMove = physicsManager.inMove;
            //    if (!inMove && !body.isKinematic)
            //    {
            //        body.Sleep();
            //    }
            //}

        }
    }
}
