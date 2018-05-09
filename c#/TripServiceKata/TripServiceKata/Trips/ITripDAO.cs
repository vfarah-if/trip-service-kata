using System.Collections.Generic;
using TripServiceKata.Users;

namespace TripServiceKata.Trips
{
    public interface ITripDAO
    {
        List<Trip> FindTripsByUser(User user);
    }
}