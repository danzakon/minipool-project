using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour {
	
	[SerializeField]
	private GameObject _roomListingPrefab;
	private GameObject RoomListingPrefab
	{
		get { return _roomListingPrefab; }	
	}

    private List<RoomListing> _roomListingButtons = new List<RoomListing> ();
    private List<RoomListing>RoomListingButtons
    {
        get { return _roomListingButtons; }
    }

    private void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();

        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);
        }

        RemoveOldRooms();
    }

    private void RoomReceived(RoomInfo room)
    {
        // searches RoomListingButtons to see if any existing rooms match the room received's Name
        // returns -1 if no existing rooms match, returns another number if they do match
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);

        // if the room received does not yet exist
        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingButtons.Add(roomListing);

                index = (RoomListingButtons.Count - 1);
            }
        }

        // if the room received does exist
        if (index != -1)
        {
            // update the name
            RoomListing roomListing = RoomListingButtons[index];
            roomListing.SetRoomNameText(room.Name);
            roomListing.Updated = true;
        }

    }

    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();

        foreach (RoomListing roomListing in RoomListingButtons)
        {
            if (!roomListing.Updated)
                removeRooms.Add(roomListing);
            else
                roomListing.Updated = false;
        }

        foreach (RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            RoomListingButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }
            
    }
}
