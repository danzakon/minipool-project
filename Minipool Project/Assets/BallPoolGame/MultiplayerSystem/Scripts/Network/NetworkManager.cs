using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml;

namespace NetworkManagement
{
    public delegate void UpdateCoinsHandler(int coins);
    public delegate void LoginHandler(LoginedState state);
    public delegate void PurchasedHandler(NetworkManagement.ProductProfile productProfile,PurchasedState state);
    public delegate void LoadPlayersHandler(NetworkManagement.PlayerProfile[] players);
    public delegate void LoadPlayerHandler(NetworkManagement.PlayerProfile player);
    public delegate void SetPlayerHandler(NetworkManagement.PlayerProfile player);
    public delegate bool CheckIsFriend(string id);

    public enum LoginedState
    {
        Successful = 0,
        Unsuccessful
    }

    public enum PurchasedState
    {
        Successful = 0,
        Unsuccessful
    }

    /// <summary>
    /// Player state.
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// The user is online.
        /// </summary>
        Online = 0,
        /// <summary>
        /// The user is online but away from their computer.
        /// </summary>
        Away,
        /// <summary>
        /// The user is online but set their status to busy.
        /// </summary>
        Busy,
        /// <summary>
        /// The user is offline.
        /// </summary>
        Offline,
        /// <summary>
        /// The user is playing a game.
        /// </summary>
        Playing
    }

    /// <summary>
    /// The Network room, who include players.
    /// </summary>
    public class Room
    {

        public int id
        {
            get;
            private set;
        }

        public int prize
        {
            get;
            set;
        }

        public Room(int id, int prize, List<PlayerProfile> players)
        {
            this.id = id;
            this.prize = prize;
            this.players = players;
            mainPlayer = (players == null || players.Count == 0) ? null : players[0];
            foreach (PlayerProfile player in players)
            {
                player.roomId = id;
                Debug.Log("Player " + id + "is in the house");
            }
        }

        public PlayerProfile mainPlayer
        {
            get;
            private set;
        }

        public List<PlayerProfile> players
        {
            get;
            private set;
        }
    }


    [System.Serializable]
    public class ProductType
    {
        public bool updateIcon{ get; set; }

        public string type = "Product type";
        public string nameInВatabase;
        /// <summary>
        /// The product can be bought only with real money.
        /// </summary>
        public bool isRealMoney = false;
        /// <summary>
        /// The product can be bought one time.
        /// </summary>
        public bool oneTimeBought = false;
        /// <summary>
        /// How many time the product can be bought?.
        /// </summary>
        public int maxCount = 0;
        public Sprite icon;
        public DefaultProductProfile[] defaultProducts;

        public static ProductType GetProductTypeByType(string type, ProductType[] productsTypeList)
        {
            foreach (var item in productsTypeList)
            {
                if (item.type == type)
                {
                    return item;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class DefaultProductProfile
    {
        /// <summary>
        /// This product name.
        /// </summary>
        public string name;
        /// <summary>
        /// The price in real money.
        /// </summary>
        public int price;
        /// <summary>
        /// Product price.
        /// </summary>
        public Texture2D icon;
        /// <summary>
        /// All possible sources  of product.
        /// </summary>
        public UnityEngine.Object[] sources;
    }

    /// <summary>
    /// The product.
    /// </summary>
    public class ProductProfile
    {
        /// <summary>
        /// Gets the data of product.
        /// </summary>
        public ProductData data
        {
            get;
            private set;
        }

        /// <summary>
        /// Product icone.
        /// </summary>
        public Texture2D icon
        {
            get;
            private set;
        }

        public ProductProfile(ProductData data, Texture2D icon)
        {
            this.data = data;
            this.icon = icon;
        }

       
        public void SetIcon(Texture2D icon)
        {
            if (icon)
            {
                this.icon = icon;
            }
        }
    }


    public class PlayerProfile
    {
        /// <summary>
        /// This users unique identifier.
        /// </summary>
        public string id
        {
            get;
            private set;
        }

        public int roomId
        {
            get;
            set;
        }

        public bool isMain
        {
            get;
            private set;
        }

        public Texture2D image
        {
            get;
            private set;
        }

        public string imageURL
        {
            get;
            private set;
        }

        public string imageName
        {
            get;
            private set;
        }

        public bool isFriend
        {
            get;
            private set;
        }

        public string userName
        {
            get;
            private set;
        }

        public int coins
        {
            get;
            private set;
        }

        public void UpdateCoinsWithoutSave(int coins)
        {
            this.coins = Mathf.Clamp(coins, NetworkManager.social.minCoinsCount, coins);
        }

        public void UpdateCoins(int coins)
        {
            this.coins = Mathf.Clamp(coins, NetworkManager.social.minCoinsCount, coins);
            NetworkManager.social.SaveMainPlayerCoins(this.coins);
        }

        public PlayerState state
        {
            get;
            set;
        }

        public PlayerProfile(string id, bool isMain, Texture2D image, string imageURL, string imageName, bool isFriend, string userName, PlayerState state, int coins)
        {
            this.id = id;
            this.isMain = isMain;
            this.image = image;
            this.imageURL = imageURL;
            this.imageName = imageName;
            this.isFriend = isFriend;
            this.userName = userName;
            this.state = state;
            this.coins = coins;
        }

        public void SetImage(Texture2D image)
        {
            if (image)
            {
                this.image = image;
            }
        }

        public void UpdateName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                this.userName = userName;
                NetworkManager.social.SaveMainPlayerName(userName);
            }
        }
    }


    public class NetworkManager
    {
        public static bool initialized = false;
        private static bool mainPlayerLoadedInProgress = false;

        public static event UpdateCoinsHandler OnCoinsUpdated;

        public static string absoluteURL
        {
            get{ return Application.absoluteURL.Replace("%20", "").Replace(" ", ""); }
        }

        public static void CallUpdatedCoins()
        {
            if (OnCoinsUpdated != null)
            {
                OnCoinsUpdated(mainPlayer.coins);
            }
        }

        private static NetworkEngine _network;
        public static NetworkEngine network
        {
            get
            {
                if (!_network)
                {
                    #if PUN
//                    _network = GameObject.Find("Dan's Network").GetComponent<dans_Network>();
                        //(new GameObject("Network")).AddComponent<dans_Network>();
                    #elif Local
                    _network = (new GameObject("Network")).AddComponent<LocalNetwork>();
                    #else
                    _network = (new GameObject("Network")).AddComponent<NetworkExample>();
                    #endif

//                    _network.Initialize();
                }
                return _network;
            }
        }

        private static SocialEngine _social;
        public static SocialEngine social
        {
            get
            {
                if (_social == null)
                {
                    _social = new SocialExample();
                }
                return _social;
            }
        }

        private static PurchasingEngine _purchasing;
        public static PurchasingEngine purchasing
        {
            get
            {
                if (_purchasing == null)
                {
                    #if UNITY_PURCHASING
                    _purchasing = new UnityIAP();
                    #else
                    ///Implement PurchasingEngine Instance
                    _purchasing = new PurchasingExample();
                    #endif
                }
                return _purchasing;
            }
        }


        public static event LoadPlayersHandler OnPlayersLoaded;
        public static event LoadPlayerHandler OnRandomPlayerLoaded;
        public static event LoadPlayersHandler OnFriendsAndRandomPlayersLoaded;
        public static event LoadPlayerHandler OnMainPlayerLoaded;
        public static event LoadPlayerHandler OnFriendLoaded;
        public static event SetPlayerHandler OnPlayerSet;

        public static void Disable()
        {
            OnPlayersLoaded = null;
            OnRandomPlayerLoaded = null;
            OnFriendsAndRandomPlayersLoaded = null;

            OnMainPlayerLoaded = null;
            OnFriendLoaded = null;
            OnPlayerSet = null;

            if (_network)
            {
                _network.Disable();
            }
            if (_social != null)
            {
                _social.Disable();
            }
        }


        public static void SignUp(string email, string password)
        {
            social.SignUp(email, password);
        }

        public static void Login(string email, string password)
        {
            social.Login(email, password);
        }

        public static void LoginWithFacebook()
        {
            social.LoginWithFacebok();
        }


        public static void Purchase(int productCount, ProductProfile productProfile)
        {
            purchasing.Purchase(productCount, productProfile);
        }

        public static NetworkManagement.PlayerProfile mainPlayer
        {
            get;
            private set;
        }

        public static NetworkManagement.PlayerProfile opponentPlayer
        {
            get;
            set;
        }

        private static NetworkManagement.PlayerProfile[] _players;
        public static NetworkManagement.PlayerProfile[] players
        {
            get
            {
                if (_players == null)
                {
                    LoadPlayers();
                }
                return _players;
            }
        }

        private  static NetworkManagement.PlayerProfile[] _friends;
        public static NetworkManagement.PlayerProfile[] friends
        {
            get
            {
                if (LoginManager.logined || LoginManager.loginedFacebook)
                {
                    if (_friends == null)
                    {
                        LoadPlayers();
                    }
                    return _friends;
                }
                else
                {
                    return null;
                }
            }
        }

        private  static NetworkManagement.PlayerProfile[] _notFriends;
        public static NetworkManagement.PlayerProfile[] notFriends
        {
            get
            {
                if (_notFriends == null)
                {
                    LoadPlayers();
                }
                return _notFriends;
            }
        }

        private  static NetworkManagement.PlayerProfile[] detectedPlayers;

        public static IEnumerator LoadFriendsAndRandomPlayers(int maxCount)
        {
            List<PlayerProfile> _detectedPlayers = new List<PlayerProfile>(0);
            yield return null;
            if (friends != null && friends.Length != 0)
            {
                for (int i = 0; i < friends.Length && i < maxCount; i++)
                {
                    NetworkManagement.PlayerProfile friend = friends[i];
                    _detectedPlayers.Add(friend);
                    Texture2D image = friend.image;

                    if (!image)
                    {
                        WWW imageData = new WWW(friend.imageURL);
                        yield return imageData;
                        if (string.IsNullOrEmpty(imageData.error))
                        {
                            image = imageData.texture;
                            friend.SetImage(image);
                            if (OnPlayerSet != null)
                            {
                                OnPlayerSet(friend);
                            }
                        }
                    }
                }
            }

            yield return null;
            if (notFriends != null && notFriends.Length != 0)
            {
                for (int i = 0; i < notFriends.Length && (friends == null || i < maxCount - friends.Length); i++)
                {
                    NetworkManagement.PlayerProfile player = notFriends[i];
                    _detectedPlayers.Add(player);
                    Texture2D image = player.image;

                    if (!image)
                    {
                        WWW imageData = new WWW(player.imageURL);
                        yield return imageData;
                        if (string.IsNullOrEmpty(imageData.error))
                        {
                            image = imageData.texture;
                            player.SetImage(image);
                            if (OnPlayerSet != null)
                            {
                                OnPlayerSet(player);
                            }
                        }
                    }
                }
            }

            detectedPlayers = _detectedPlayers.ToArray();
            if (OnFriendsAndRandomPlayersLoaded != null)
            {
                OnFriendsAndRandomPlayersLoaded(players);
            }
        }

        public static IEnumerator LoadFriend(string id)
        {
            yield return null;
            NetworkManagement.PlayerProfile friend = null;

            foreach (NetworkManagement.PlayerProfile item in friends)
            {
                if (item.id == id)
                {
                    friend = item;
                    Texture2D image = friend.image;

                    if (!image)
                    {
                        WWW imageData = new WWW(friend.imageURL);
                        yield return imageData;
                        if (string.IsNullOrEmpty(imageData.error))
                        {
                            image = imageData.texture;
                            friend.SetImage(image);
                            if (OnPlayerSet != null)
                            {
                                OnPlayerSet(friend);
                            }
                        }
                    }
                    break;
                }
            } 
            if (friend != null && OnFriendLoaded != null)
            {
                OnFriendLoaded(friend);
            }
        }

        public static IEnumerator SetMainPlayerImage(Texture2D image)
        {
            while (mainPlayerLoadedInProgress)
            {
                yield return null;
            }
            mainPlayerLoadedInProgress = true;
            //mainPlayer = new PlayerProfile(social.mainPlayerId, true, image, "", image.name, false, social.GetMainPlayerName(), PlayerState.Online, social.GetMainPlayerCoins());
            if (OnMainPlayerLoaded != null)
            {
                OnMainPlayerLoaded(mainPlayer);
            }
            mainPlayerLoadedInProgress = false;
        }

        public static IEnumerator SetMainPlayerImage(string imageURL)
        {
            while (mainPlayerLoadedInProgress)
            {
                yield return null;
            }
            mainPlayerLoadedInProgress = true;
            Texture2D image = null;
            if (!string.IsNullOrEmpty(imageURL))
            {
                DownloadManager.DownloadParameters parameters = new DownloadManager.DownloadParameters(imageURL, "Main player", DownloadManager.DownloadType.DownloadOrLoadFromDisc);
                yield return DownloadManager.Download(parameters);
                if (parameters.texture)
                {
                    image = parameters.texture;
                }
            }
            mainPlayer = new PlayerProfile(social.mainPlayerId, true, image, imageURL, "", false, social.GetMainPlayerName(), PlayerState.Online, social.GetMainPlayerCoins());

            if (OnMainPlayerLoaded != null)
            {
                OnMainPlayerLoaded(mainPlayer);
            }
            mainPlayerLoadedInProgress = false;
        }

        public static IEnumerator LoadMainPlayer(Texture2D image)
        {
            if (!social.AvatarDataIsLocal())
            {
                while (mainPlayerLoadedInProgress)
                {
                    yield return null;
                }
                mainPlayerLoadedInProgress = true;
                yield return null;
           
                string mainURL = social.GetAvatarURL();
                if (!string.IsNullOrEmpty(mainURL))
                {
                    DownloadManager.DownloadParameters parameters = new DownloadManager.DownloadParameters(mainURL, "Main player", DownloadManager.DownloadType.Update);
                    yield return DownloadManager.Download(parameters);
                    if (parameters.texture)
                    {
                        image = parameters.texture;
                    }
                }
                mainPlayer = new PlayerProfile(social.mainPlayerId, true, image, mainURL, "", false, social.GetMainPlayerName(), PlayerState.Online, social.GetMainPlayerCoins());
                if (OnMainPlayerLoaded != null)
                {
                    OnMainPlayerLoaded(mainPlayer);
                }
                mainPlayerLoadedInProgress = false;
            }
        }

        public static IEnumerator LoadRandomPlayer()
        {
            yield return null;
            opponentPlayer = null;
            if (players != null && players.Length != 0)
            {
                IEnumerable<NetworkManagement.PlayerProfile> randomPlayersEnum =
                    from player in players
                                   where player.state == PlayerState.Online
                                   select player;

                NetworkManagement.PlayerProfile[] randomPlayers = randomPlayersEnum.ToArray();
                if (randomPlayers != null && randomPlayers.Length > 0)
                {
                    int randomPlayerNumber = Random.Range(0, randomPlayers.Length);
                    opponentPlayer = randomPlayers[randomPlayerNumber];
                    Texture2D image = opponentPlayer.image;

                    if (!image)
                    {
                        WWW imageData = new WWW(opponentPlayer.imageURL);
                        yield return imageData;
                        if (string.IsNullOrEmpty(imageData.error))
                        {
                            image = imageData.texture;
                            opponentPlayer.SetImage(image);
                            if (OnPlayerSet != null)
                            {
                                OnPlayerSet(opponentPlayer);
                            }
                        }
                    }
                }
            }   
            if (OnRandomPlayerLoaded != null)
            {
                OnRandomPlayerLoaded(opponentPlayer);
            }
        }

        public static PlayerProfile FindPlayer(string playerId)
        {
            IEnumerable<NetworkManagement.PlayerProfile> playersEnum =
                from player in players
                            where player.id == playerId
                            select player;

            return playersEnum.ToArray()[0];
        }

        public static NetworkManagement.PlayerProfile[] FindPlayers(string userName, int prize, bool isOnline, bool onlyFriends)
        {
            if (string.IsNullOrEmpty(userName) && prize == 0 && !isOnline && !onlyFriends)
            {
                return detectedPlayers;
            }
            IEnumerable<NetworkManagement.PlayerProfile> playersEnum =
                from player in detectedPlayers
                            where (string.IsNullOrEmpty(userName) || player.userName.Contains(userName)) && (!isOnline || player.state == PlayerState.Online) && (!onlyFriends || player.isFriend == true)
                            select player;

            return playersEnum.ToArray();
        }

        public static void UpdatePlayers()
        {
            _players = null;
            LoadPlayers();
        }

        private static void LoadPlayers()
        {
            NetworkManager.network.LoadPlayers(ref _players);

            if (_players != null)
            {
                if (LoginManager.logined || LoginManager.loginedFacebook)
                {
                    IEnumerable<NetworkManagement.PlayerProfile> newFriends =
                        from player in _players
                                          where player.isFriend == true
                                          select player;

                    _friends = newFriends.ToArray();
                }
                else
                {
                    _friends = null;
                }

                IEnumerable<NetworkManagement.PlayerProfile> newPlayers =
                    from player in _players
                                   where player.isFriend != true
                                   select player;
            
                _notFriends = newPlayers.ToArray();

            }
            if (OnPlayersLoaded != null)
            {
                OnPlayersLoaded(_players);
            }
        }

        public static string mainPlayerEmail
        {
            get { return social.GetMainPlayerEmail(); }
        }

        public static string privacyPolicyURL
        {
            get { return social.GetPrivacyPolicyURL(); }
        }

        public static PlayerProfile PlayerFromString(string playerData, CheckIsFriend friendChecker)
        {
            string str = "";
            int step = 0;

            string id = "-1";
            string imageURL = "";
            string imageName = "";
            bool isFrient = false;
            string userName = "";
            PlayerState state = PlayerState.Offline;
            int coins = 0;
            int prize = 0;

            foreach (char item in playerData)
            {
                if (item != ';')
                {
                    str += item;
                }
                else
                {
                    step++;
                    switch (step)
                    {
                        case 1:
                            id = str;
                            isFrient = friendChecker(id);
                            break;
                        case 2:
                            imageURL = str;
                            break;
                        case 3:
                            imageName = str;
                            break;
                        case 4:
                            userName = str;
                            break;
                        case 5:
                            state = (PlayerState)int.Parse(str);
                            break;
                        case 6:
                            coins = int.Parse(str);
                            break;
                        case 7:
                            prize = int.Parse(str);
                            break;
                        default:
                            break;
                    }
                    str = "";
                }
            }
            return new PlayerProfile(id, false, null, imageURL, imageName, isFrient, userName, state, coins);
        }

        public static string PlayerToString(PlayerProfile playerProfile)
        {
            if (playerProfile == null)
            {
                return "";
            }
            return playerProfile.id + ";" + playerProfile.imageURL + ";" + playerProfile.imageName + ";" + playerProfile.userName + ";" + (int)playerProfile.state + ";" + playerProfile.coins + ";";
        }
    }
}
