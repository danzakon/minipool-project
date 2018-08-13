using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkManagement;
using BallPool.Mechanics;

namespace BallPool
{
    public class MinipoolPlayer : BallPoolPlayer
    {
        public new static MinipoolPlayer mainPlayer
        {
            get { return (players == null || players.Length < 1) ? null : (MinipoolPlayer)players[0]; }
        }

        public static MinipoolPlayer otherPlayer
        {
            get { return (players == null || players.Length < 2) ? null : (MinipoolPlayer)players[1]; }
        }

        public bool checkIsBlackInEnd
        {
            get;
            private set;
        }

        /// <summary>
        /// The Player need to put the black ball
        /// </summary>
        public bool isBlack
        {
            get;
            set;
        }

        public bool isCueBall
        {
            get;
            set;
        }

        private bool _isStripes;
        private bool _isSolids;

        public bool isStripes
        {
            get { return _isStripes; }
            set
            {
                _isStripes = value;
                _isSolids = !_isStripes;
            }
        }

        public bool isSolids
        {
            get { return _isSolids; }
            set
            {
                _isSolids = value;
                _isStripes = !_isSolids;
            }
        }

        public override void OnDeactivate()
        {
            checkIsBlackInEnd = false;
            isBlack = false;
            isCueBall = false;
            _isSolids = false;
            _isStripes = false;
        }

        public override void SetActiveBalls(Ball[] balls)
        {
            if (balls == null || balls.Length == 0)
            {
                return;
            }
            this.balls = new List<Ball>(0);
            foreach (Ball ball in balls)
            {
                if (!ball.inPocket && ball.gameObject.activeInHierarchy)
                {
                    if (isStripes && MinipoolGameLogic.isStripesBall(ball.id))
                    {
                        this.balls.Add(ball);
                    }
                    else if (isSolids && MinipoolGameLogic.isSolidsBall(ball.id))
                    {
                        this.balls.Add(ball);
                    }
                }
            }

            if (MinipoolGameLogic.gameState.playersHasBallType)
            {
                if (this.balls.Count == 0)
                {
                    Ball blackBall = MinipoolGameLogic.GetBlackBall(BallPoolGameManager.instance.balls);
                    if (!blackBall.inPocket)
                    {
                        checkIsBlackInEnd = true;
                        this.balls.Add(blackBall);
                    }
                }
            }
        }

        public static bool PlayerHasSomeBallType(MinipoolPlayer player, int ballId)
        {
            return (player.isSolids && MinipoolGameLogic.isSolidsBall(ballId)) || (player.isStripes && MinipoolGameLogic.isStripesBall(ballId));
        }

        public MinipoolPlayer(int playerId, string name, int coins, object avatar, string avatarURL) : base(playerId, name, coins, avatar, avatarURL)
        {
            _isStripes = false;
            _isSolids = false;
            isBlack = false;
            isCueBall = false;
            checkIsBlackInEnd = false;
        }


    }
}
