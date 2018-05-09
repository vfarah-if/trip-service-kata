using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TripServiceKata.Trips;

namespace TripServiceKata.Users
{
    public class User
    {
        private readonly List<Trip> trips = new List<Trip>();
        private readonly List<User> friends = new List<User>();


        public ReadOnlyCollection<User> GetFriends()
        {
            return friends.AsReadOnly();
        }

        public ReadOnlyCollection<Trip> Trips()
        {
            return trips.AsReadOnly();
        }


        public void AddFriend(User user)
        {
            friends.Add(user);
        }

        public void AddTrip(Trip trip)
        {
            trips.Add(trip);
        }


        public bool IsAFriendOf(User user)
        {
            return user != null && friends.Any(each => each.Equals(user));
        }
    }
}
