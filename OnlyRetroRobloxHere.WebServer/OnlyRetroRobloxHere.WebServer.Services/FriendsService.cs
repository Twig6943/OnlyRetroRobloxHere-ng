using System.Collections.Generic;
using System.Linq;
using OnlyRetroRobloxHere.WebServer.Enums;
using OnlyRetroRobloxHere.WebServer.Models;

namespace OnlyRetroRobloxHere.WebServer.Services;

internal class FriendsService
{
	public static FriendsService Instance { get; } = new FriendsService();

	public List<FriendRequest> FriendRequests { get; } = new List<FriendRequest>();

	private FriendRequest? FindFriendRequest(int user, int otherUser)
	{
		return FriendRequests.Find((FriendRequest x) => (x.Inviter == user && x.Invitee == otherUser) || (x.Inviter == otherUser && x.Invitee == user));
	}

	private FriendRequest GetFriendRequest(int user, int otherUser)
	{
		FriendRequest friendRequest = FindFriendRequest(user, otherUser);
		if (friendRequest == null)
		{
			friendRequest = new FriendRequest
			{
				Inviter = user,
				Invitee = otherUser,
				Status = FriendStatus.NotFriend
			};
			FriendRequests.Add(friendRequest);
		}
		return friendRequest;
	}

	private IEnumerable<FriendRequest> GetFriendRequests(long user)
	{
		return FriendRequests.Where((FriendRequest x) => x.Inviter == user || x.Invitee == user);
	}

	public bool AreFriend(int user, int otherUser)
	{
		FriendRequest friendRequest = GetFriendRequest(user, otherUser);
		return friendRequest.Status == FriendStatus.Friend;
	}

	public IEnumerable<int> AreFriends(int user, IEnumerable<int> users)
	{
		List<int> list = new List<int>();
		foreach (int user2 in users)
		{
			if (AreFriend(user, user2))
			{
				list.Add(user2);
			}
		}
		return list;
	}

	public void CreateFriend(int inviter, int invitee)
	{
		FriendRequest friendRequest = GetFriendRequest(inviter, invitee);
		friendRequest.Status = FriendStatus.Friend;
	}

	public void BreakFriend(int inviter, int invitee)
	{
		FriendRequest friendRequest = GetFriendRequest(inviter, invitee);
		friendRequest.Status = FriendStatus.NotFriend;
	}

	public IEnumerable<FriendRequest> GetFriends(int user, int skip = 0)
	{
		return (from x in GetFriendRequests(user)
			where x.Status == FriendStatus.Friend
			select x).Skip(skip);
	}

	public IEnumerable<int> GetFriendIDs(int user, int skip = 0)
	{
		return from x in GetFriends(user, skip)
			select (x.Inviter != user) ? x.Inviter : x.Invitee;
	}
}
