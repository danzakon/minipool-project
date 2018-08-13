using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BallPool.Mechanics;

namespace BallPool.AI
{
    public class AightBallPoolAIManager : BallPoolAIManager
    {
        public override bool FindException(int ballId)
        {
            if (MinipoolGameLogic.isCueBall(ballId))
            {
                return true;
            }
            bool isBlackBall = MinipoolGameLogic.isBlackBall(ballId);
            if (!MinipoolGameLogic.gameState.playersHasBallType && isBlackBall)
            {
                return true;
            }
            if (!MinipoolGameLogic.gameState.playersHasBallType && !isBlackBall)
            {
                return false;
            }

            //bool mainPlayerIsBlack = MinipoolPlayer.mainPlayer.isBlack;
            //bool otherPlayerIsBlack = MinipoolPlayer.otherPlayer.isBlack;
            //bool ballIsStripes = MinipoolGameLogic.isStripesBall(ballId);
            //bool ballIsSolids = MinipoolGameLogic.isSolidsBall(ballId);

            //if (MinipoolPlayer.mainPlayer.myTurn)
            //{
            //    if (mainPlayerIsBlack)
            //    {
            //        return !isBlackBall;
            //    }
            //    else if(isBlackBall)
            //    {
            //        return true;
            //    }

            //    bool mainPlayerIsStripes = MinipoolPlayer.mainPlayer.isStripes;
            //    bool mainPlayerIsSolids = MinipoolPlayer.mainPlayer.isSolids;
            //    if (ballIsStripes)
            //    {
            //        return !mainPlayerIsStripes;
            //    }
            //    else if (ballIsSolids)
            //    {
            //        return !mainPlayerIsSolids;
            //    }
            //}
            //else if (MinipoolPlayer.otherPlayer.myTurn)
            //{
            //    if (otherPlayerIsBlack)
            //    {
            //        return !isBlackBall;
            //    }
            //    else if(isBlackBall)
            //    {
            //        return true;
            //    }

            //    bool otherPlayerIsStripes = MinipoolPlayer.otherPlayer.isStripes;
            //    bool otherPlayerIsSolids = MinipoolPlayer.otherPlayer.isSolids;

            //    if (ballIsStripes)
            //    {
            //        return !otherPlayerIsStripes;
            //    }
            //    else if (ballIsSolids)
            //    {
            //        return !otherPlayerIsSolids;
            //    }
            //}
            return false;
        }
    }
}
