using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public PhotonView PV;
        bool isConnecting;
        #region Private Serializable Fields
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        private Player[] playerList;

        #endregion


        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";


        #endregion




        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        private void Start()
        {
   
            PV = GetComponent<PhotonView>();
            Connect();
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        private void Update()
        {
            Debug.Log(PhotonNetwork.PlayerList);
        }


        #endregion


        #region Public Fields


       


        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }


        #endregion


        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            

            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }


        #endregion

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            int randomRoom = Random.Range(0, 9000);
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom("Room#" + randomRoom, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
            
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
               Debug.Log("We load the scene1 ");

              // #Critical
               // Load the Room Level.
               PhotonNetwork.LoadLevel("scene1");
            }
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }
        [PunRPC]
        private void RPC_CreatePlayer()
        {
         GameObject player =   PhotonNetwork.Instantiate("Player", new Vector3(-1.01f, 1.82f, -12.63f), Quaternion.identity);
         PhotonView photonView = player.GetComponent<PhotonView>();


        }

        //public void SpawnPlayer()
        //{
        //    GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(-1.01f, 1.82f, -12.63f), Quaternion.identity);
        //    PhotonView photonView = player.GetComponent<PhotonView>();

        //    if (PhotonNetwork.AllocateViewID(photonView))
        //    {
        //        object[] data = new object[]
        //        {
        //    player.transform.position, player.transform.rotation, photonView.ViewID
        //        };

        //        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        //        {
        //            Receivers = ReceiverGroup.Others,
        //            CachingOption = EventCaching.AddToRoomCache
        //        };

        //        SendOptions sendOptions = new SendOptions
        //        {
        //            Reliability = true
        //        };

        //        PhotonNetwork.RaiseEvent(CustomManualInstantiationEventCode, data, raiseEventOptions, sendOptions);
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to allocate a ViewId.");

        //        Destroy(player);
        //    }
        //}

    }
}