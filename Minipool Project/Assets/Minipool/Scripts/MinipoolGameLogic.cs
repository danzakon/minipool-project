using System.Collections;
using System.Collections.Generic;
using BallPool.Mechanics;

namespace BallPool
{
    public class MinipoolGameState : GameState
    {
        /// <summary>
        /// The table will close after first shot.
        /// </summary>
        public bool tableIsOpened = true;
        /// <summary>
        /// The type of the players is e stripes or solids.
        /// </summary>
        public bool playersHasBallType = false;
        /// <summary>
        /// Balls hit board count in first shot.
        /// </summary>
        public int ballsHitBoardCount = 0;
        /// <summary>
        /// The cue ball has hit right ball, stripes, solids or black.
        /// </summary>
        public bool cueBallHasHitRightBall = false;
        /// <summary>
        /// The cue ball has hit some ball at first hit.
        /// </summary>
        public bool cueBallHasHitSomeBall = false;
        /// <summary>
        /// The cue ball has right ball in pocket, stripes, solids or black.
        /// </summary>
        public bool hasRightBallInPocket = false;
        /// <summary>
        /// The current player has cue ball in his hand
        /// </summary>
        public bool cueBallInHand = true;
        /// <summary>
        /// The cue ball in pocket.
        /// </summary>
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
            get
            {
                if (_gameState == null)
                {
                    _gameState = new MinipoolGameState();
                }
                return _gameState;
            }
        }

        public void RessetState()
        {
            gameState.gameIsComplete = false;
            gameState.needToChangeTurn = false;
            gameState.cueBallHasHitRightBall = false;
            gameState.cueBallHasHitSomeBall = false;
            gameState.hasRightBallInPocket = false;
            gameState.ballsHitBoardCount = 0;
            gameState.cueBallInHand = false;
            gameState.cueBallInPocket = false;

            MinipoolPlayer.mainPlayer.isCueBall = false;
            MinipoolPlayer.otherPlayer.isCueBall = false;
        }

        public void OnBallHitBoard(int ballId)
        {
            if (gameState.tableIsOpened)
            {
                gameState.ballsHitBoardCount++;
            }
        }

        public void OnCueBallHitBall(int cueBallId, int ballId)
        {
            if (gameState.cueBallHasHitSomeBall)
            {
                return;
            }
            gameState.cueBallHasHitSomeBall = true;

            if (isBlackBall(ballId))
            {
                if (((MinipoolPlayer)BallPoolPlayer.currentPlayer).isBlack)
                {
                    gameState.cueBallHasHitRightBall = true;
                }
            }
            else if (gameState.tableIsOpened)
            {
                gameState.cueBallHasHitRightBall = true;
            }
            else if (!gameState.playersHasBallType)
            {
                gameState.cueBallHasHitRightBall = true;
            }
            else if (MinipoolPlayer.PlayerHasSomeBallType((MinipoolPlayer)BallPoolPlayer.currentPlayer, ballId))
            {
                gameState.cueBallHasHitRightBall = true;
            }
            if (!gameState.cueBallHasHitRightBall)
            {
                gameState.cueBallInHand = true;
                gameState.needToChangeTurn = true;
            }
        }

        public void OnBallInPocket(int ballId, ref bool cueBallInPocket)
        {
            if (isCueBall(ballId))
            {
                if (MinipoolPlayer.mainPlayer.myTurn)
                {
                    MinipoolPlayer.mainPlayer.isCueBall = true;
                }
                else if (MinipoolPlayer.otherPlayer.myTurn)
                {
                    MinipoolPlayer.otherPlayer.isCueBall = true;
                }

                gameState.needToChangeTurn = true;
                //gameState.cueBallInHand = true;
                cueBallInPocket = true;
                gameState.cueBallInPocket = true;
                return;
            }
            if (gameState.tableIsOpened)
            {
                if (!isBlackBall(ballId))
                {
                    gameState.hasRightBallInPocket = true;
                }
            }
            else
            {
                if (!gameState.playersHasBallType)
                {
                    gameState.playersHasBallType = true;
                    if (!isBlackBall(ballId))
                    {
                        gameState.hasRightBallInPocket = true;
                    }
                    if (MinipoolPlayer.mainPlayer.myTurn)
                    {
                        if (MinipoolGameLogic.isStripesBall(ballId))
                        {
                            MinipoolPlayer.mainPlayer.isStripes = true;
                            MinipoolPlayer.otherPlayer.isSolids = true;
                        }
                        else if (MinipoolGameLogic.isSolidsBall(ballId))
                        {
                            MinipoolPlayer.mainPlayer.isSolids = true;
                            MinipoolPlayer.otherPlayer.isStripes = true;
                        }
                    }
                    else if (MinipoolPlayer.otherPlayer.myTurn)
                    {
                        if (MinipoolGameLogic.isStripesBall(ballId))
                        {
                            MinipoolPlayer.otherPlayer.isStripes = true;
                            MinipoolPlayer.mainPlayer.isSolids = true;
                        }
                        else if (MinipoolGameLogic.isSolidsBall(ballId))
                        {
                            MinipoolPlayer.otherPlayer.isSolids = true;
                            MinipoolPlayer.mainPlayer.isStripes = true;
                        }
                    }
                }
                else if (MinipoolPlayer.PlayerHasSomeBallType((MinipoolPlayer)BallPoolPlayer.currentPlayer, ballId))
                {
                    gameState.hasRightBallInPocket = true;
                }
            }
        }

        public void OnEndShot(bool blackBallInPocket, out bool gameIsEnd)
        {
            gameIsEnd = false;
            if (BallPoolGameManager.instance == null)
            {
                return;
            }
            string info = "";

            bool canSetInfo = true;

            if (gameState.cueBallInPocket && !blackBallInPocket)
            {
                if (BallPoolPlayer.mainPlayer.myTurn)
                {
                    info = "You pocket the cue ball, \n" + MinipoolPlayer.otherPlayer.name + " has cue ball in hand";
                }
                else
                {
                    info = MinipoolPlayer.otherPlayer.name + " pocket the cue ball, \n" + " You have cue ball in hand";
                }
                BallPoolGameManager.instance.SetGameInfo(info);
                canSetInfo = false;
            }
            if (gameState.tableIsOpened)
            {
                info = " ";
                if (!(gameState.cueBallHasHitRightBall && gameState.hasRightBallInPocket))
                {
                    gameState.needToChangeTurn = true;
                    if (gameState.ballsHitBoardCount < 4)
                    {
                        gameState.cueBallInHand = true;
                        info = "Break up of balls was weak, \n" + (BallPoolPlayer.mainPlayer.myTurn ? MinipoolPlayer.otherPlayer.name + " has cue ball in hand" : "You have cue ball in hand");
                    }
                }
            }
            gameState.tableIsOpened = false;
            if (blackBallInPocket)
            {
                if (MinipoolPlayer.mainPlayer.myTurn)
                {
                    MinipoolPlayer.mainPlayer.isWinner = MinipoolPlayer.mainPlayer.isBlack && !MinipoolPlayer.mainPlayer.isCueBall;
                    MinipoolPlayer.otherPlayer.isWinner = !MinipoolPlayer.mainPlayer.isWinner;
                    info = MinipoolPlayer.otherPlayer.isWinner ? ("You poked the black ball " + (MinipoolPlayer.mainPlayer.isCueBall ? "with cue ball" : "")) : "";
                }
                else if (MinipoolPlayer.otherPlayer.myTurn)
                {
                    MinipoolPlayer.otherPlayer.isWinner = MinipoolPlayer.otherPlayer.isBlack && !MinipoolPlayer.otherPlayer.isCueBall;
                    MinipoolPlayer.mainPlayer.isWinner = !MinipoolPlayer.otherPlayer.isWinner;
                    info = MinipoolPlayer.mainPlayer.isWinner ? (MinipoolPlayer.otherPlayer.name + " pocket the black ball " + (MinipoolPlayer.otherPlayer.isCueBall ? "with cue ball" : "")) : "";
                }
                gameState.needToChangeTurn = false;
                gameIsEnd = true;
                BallPoolGameManager.instance.SetGameInfo(info);
                return;
            }

            if (MinipoolPlayer.mainPlayer.checkIsBlackInEnd)
            {
                MinipoolPlayer.mainPlayer.isBlack = true;
            }
            if (MinipoolPlayer.otherPlayer.checkIsBlackInEnd)
            {
                MinipoolPlayer.otherPlayer.isBlack = true;
            }

            if (!gameState.cueBallHasHitRightBall)
            {
                gameState.cueBallInHand = true;
                gameState.needToChangeTurn = true;

                if (MinipoolPlayer.mainPlayer.myTurn)
                {
                    if (info == "")
                        info = MinipoolPlayer.mainPlayer.isBlack ? "You need to hit black ball" : (MinipoolPlayer.mainPlayer.isSolids ? "You need to hit solids ball" : (MinipoolPlayer.mainPlayer.isStripes ? "You need to hit stripes ball" : "You need to hit solids or stripes ball")) +
                        "\n" + MinipoolPlayer.otherPlayer.name + " has cue ball in hand";
                }
                else
                {
                    if (info == "")
                        info = MinipoolPlayer.otherPlayer.name + ((MinipoolPlayer.otherPlayer.isBlack ? " need to hit black ball" : (MinipoolPlayer.otherPlayer.isSolids ? " need to hit solids ball" : (AightBallPoolPlayer.otherPlayer.isStripes ? " need to hit stripes ball" : " need to hit solids or stripes ball")))) +
                        ", \nYou have cue ball in hand";
                }
            }
            else if (!gameState.hasRightBallInPocket)
            {
                gameState.needToChangeTurn = true;
                //gameState.cueBallInHand = true;
                if (MinipoolPlayer.mainPlayer.myTurn)
                {
                    if (info == "")
                        info = MinipoolPlayer.mainPlayer.isBlack ? "You need to pocket solids ball" : (MinipoolPlayer.mainPlayer.isSolids ? "You need to pocket solids ball" : (MinipoolPlayer.mainPlayer.isStripes ? "You need to pocket stripes ball" : "You need to pocket solids or stripes ball"));
                }
                else
                {
                    if (info == "")
                        info = MinipoolPlayer.otherPlayer.name + (MinipoolPlayer.otherPlayer.isBlack ? " need to pocket black ball" : ((MinipoolPlayer.otherPlayer.isSolids ? " need to pocket solids ball" : (MinipoolPlayer.otherPlayer.isStripes ? " need to pocket stripes ball" : " need to pocket solids or stripes ball"))));
                }
            }
            if (canSetInfo)
            {
                BallPoolGameManager.instance.SetGameInfo(info);
            }
        }

        public override void OnEndTime()
        {
            UnityEngine.Debug.Log("OnEndPlayTime");
            gameState.cueBallInHand = true;
            string info = MinipoolPlayer.mainPlayer.myTurn ? "You run out of time\n" + MinipoolPlayer.otherPlayer.name + " has cue ball in hand" : MinipoolPlayer.otherPlayer.name + " run out of time, \nYou have cue ball in hand ";
            BallPoolGameManager.instance.SetGameInfo(info);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _gameState = null;
        }

        public static Ball GetCueBall(Ball[] balls)
        {
            foreach (Ball ball in balls)
            {
                if (isCueBall(ball.id))
                {
                    return ball;
                }
            }
            return null;
        }

        /// <summary>
        /// Is the ball in pocket?, not for the cue ball, the cue ball will be resetted
        /// </summary>
        public static bool ballInPocket(Ball ball)
        {
            return ball.inPocket;
        }

        public static Ball GetBlackBall(Ball[] balls)
        {
            foreach (Ball ball in balls)
            {
                if (isBlackBall(ball.id))
                {
                    return ball;
                }
            }
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
