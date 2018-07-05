using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkManagement
{
    public delegate void NetworkHandler(NetworkState state);

    public enum NetworkState
    {
        Disconnected = 0,
        Connected,
        FailedToConnect,
        LostConnection,
        CreatedRoom,
        JoinedToRoom,
        LeftRoom,
        RoomCreateFailed,
        JoinRoomFailed,
        OpponentReadyToPlay
    }

    public abstract class NetworkEngine : MonoBehaviour
    {
        public event NetworkHandler OnNetwork;

        public NetworkGameAdapter adapter { get; private set; }

        public int sendRate { get; protected set; }

        public bool opponentWaitingForYourTurn { get; protected set; }

        public abstract void SendRemoteMessage(string message, params object[] args);

        public abstract void OnGoToPlayWithPlayer(PlayerProfile player);


        public void SetAdapter(NetworkGameAdapter adapter)
        {
            this.adapter = adapter;
        }

        protected void CallNetworkState(NetworkState state)
        {
            this.state = state;
            if (OnNetwork != null)
            {
                OnNetwork(state);
            }
        }

        public virtual void Initialize()
        {
            opponentWaitingForYourTurn = false;
        }

        protected virtual void Awake()
        {
            //DontDestroyOnLoad(gameObject);
            state = NetworkState.Disconnected;
        }

        protected bool reachable { get { return (Application.internetReachability != NetworkReachability.NotReachable); } }
        protected bool oldReachable = false;

        protected virtual void Update()
        {
            if (oldReachable != reachable)
            {
                oldReachable = reachable;
                if (!reachable)
                {
                    CallNetworkState(NetworkState.LostConnection);
                }
            }
        }

        IEnumerator Start()
        {
            if (reachable)
            {
                Connect();
            }

            while (true)
            {
                yield return new WaitForSeconds(3.0f);
                if ((state == NetworkState.Disconnected || state == NetworkState.FailedToConnect || state == NetworkState.LostConnection))
                {
                    if (reachable)
                    {
                        Debug.Log("IEnumerator Start Connect ");
                        Connect();
                    }
                }
            }
        }

        public virtual void Disable()
        {
            OnNetwork = null;
        }

        public NetworkState state
        {
            get;
            private set;
        }

        public abstract void Reset();

        public abstract void Disconnect();

        public abstract bool CheckIsFriend(string id);

        public abstract void CreateRoom();

        public abstract void LeftRoom();

        public abstract void Connect();

        public abstract void OnOpponentReadyToPlay(string playerData);

        public abstract void OnOpponentStartToPlay(int turnId);

        public abstract void OnSendTime(float time01);

        public void OnMadeTurn()
        {
            opponentWaitingForYourTurn = false;
        }

        public abstract void StartSimulate(string ballsState);

        public abstract void EndSimulate(string ballsState);

        public virtual void OnOpponentWaitingForYourTurn()
        {
            opponentWaitingForYourTurn = true;
        }

        public abstract void OnOpponentInGameScene();
        public abstract void OnOpponentForceGoHome();
        public abstract void StartUpdatePlayers();

        public abstract void LoadPlayers(ref NetworkManagement.PlayerProfile[] players);
    }
}
