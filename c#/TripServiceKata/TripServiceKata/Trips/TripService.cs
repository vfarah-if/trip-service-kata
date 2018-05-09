using System.Collections.Generic;
using System.Linq;
using TripServiceKata.Exceptions;
using TripServiceKata.Users;

namespace TripServiceKata.Trips
{
    public class TripService : ITripService
    {
        private readonly IUserSession userSession;
        private readonly ITripDAO tripDAO;

        public TripService(IUserSession userSession, ITripDAO tripDAO)
        {
            this.userSession = userSession ?? throw new System.ArgumentNullException(nameof(userSession));
            this.tripDAO = tripDAO ?? throw new System.ArgumentNullException(nameof(tripDAO));
        }

        //REFACTOR: Return readonly collection
        public List<Trip> GetTripsByUser(User user)
        {
            User loggedUser = this.userSession.GetLoggedUser();
            if (loggedUser == null)
            {
                throw new UserNotLoggedInException();
            }
            return user.IsAFriendOf(loggedUser) ?
                tripDAO.FindTripsByUser(user) :
                Enumerable.Empty<Trip>().ToList();
        }
    }
}
