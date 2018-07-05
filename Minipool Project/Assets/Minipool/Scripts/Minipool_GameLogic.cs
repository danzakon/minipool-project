using UnityEngine;
using System.Collections;
using BallPool.Mechanics;


namespace BallPool
{
        public class MinipoolGameState : GameState
        {

            public bool tableIsOpened = true;
            public bool playersHasBallType = false;
            public int ballsHitBoardCount = 0;
            public bool cueBallHasHitRightBall = false;
            public bool cueBallHasHitSomeBall = false;
            public bool hasRightBallInPocket = false;
            public bool cueBallInHand = true;
            public bool cueBallInPocket = false;

            public MinipoolGameState()
                : base()
            {
                tableIsOpened = true;
                playersHasBallType = false;
                ballsHitBoardCount = 0;
                cueBallHasHitRightBall = false;
                cueBallHasHitSomeBall = false;
                hasRightBallInPocket = false;
                cueBallInHand = true;
                cueBallInPocket = false;
            }
        }


        public class MinipoolGameLogic : BallPoolGameLogic
        {
            

            private static MinipoolGameState _gameState;

            public static MinipoolGameState gameState
            {
                get; set;
            }

            public void ResetState()
            {
            Debug.Log("Minipool game logic: reset state");
            }

            public void OnBallHitBoard(int ballId)
            {

            }

            public void OnCueBallHitBall(int cueBallId, int ballId)
            {

            }

            public void OnBallInPocket(int ballId, ref bool cueBallInPocket)
            {

            }

            public void OnEndShot(bool blackBallInPocket, out bool gameIsEnd)
            {
                gameIsEnd = false;
            }

            public override void OnEndTime()
            {

            }

            public override void Deactivate()
            {

            }

            public static Ball GetCueBall(Ball[] balls)
            {
                return null;
            }


            public static bool ballInPocket(Ball ball)
            {
                return ball.inPocket;
            }

            public static Ball GetBlackBall(Ball[] balls)
            {
                return null;
            }

            public static bool isCueBall(int id)
            {
                return id == 0 || id == 1;
            }

            public static bool isBlackBall(int id)
            {
                return id == 9;
            }

            public static bool isStripesBall(int id)
            {
                return id > 9 && id < 17;
            }

            public static bool isSolidsBall(int id)
            {
                return id > 1 && id < 9;
            }
        }

}
