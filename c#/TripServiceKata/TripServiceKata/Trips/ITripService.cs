using System.Collections.Generic;
using TripServiceKata.Users;

namespace TripServiceKata.Trips
{
    public interface ITripService
    {
        List<Trip> GetTripsByUser(User user);
    }
}