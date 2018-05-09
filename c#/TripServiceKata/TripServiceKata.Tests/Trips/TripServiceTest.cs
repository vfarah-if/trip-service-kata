using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TripServiceKata.Exceptions;
using TripServiceKata.Trips;
using TripServiceKata.Users;

namespace TripServiceKata.Tests.Trips
{
    [TestFixture]
    public class WhenWorkingWithTheTripService
    {
        private readonly User guest = null;
        private User loggedInUser;
        private User friend;
        private User stranger;
        private List<Trip> trips;

        private IUserSession userSessionMock;
        private ITripDAO tripDAOMock;        

        [SetUp]
        public void SetUpUserSession()
        {
            userSessionMock = Substitute.For<IUserSession>();
            tripDAOMock = Substitute.For<ITripDAO>();
        }

        [TearDown]
        public void TearDownUserSession()
        {
            userSessionMock = null;
            tripDAOMock = null;
        }

        private User CreateUser()
        {
            return new User();
        }

        private Trip CreateTrip()
        {
            return new Trip();
        }

        private IEnumerable<Trip> CreateTrips()
        {
            yield return CreateTrip();
            yield return CreateTrip();
            yield return CreateTrip();
        }

        private void SetupFriends()
        {
            friend = CreateUser();
            loggedInUser = CreateUser();
            loggedInUser.AddFriend(friend);
            friend.AddFriend(loggedInUser);
        }


        [Test]
        public void ShouldThrowANullArgumentExceptionWhenANullUserSessionIsAssigned()
        {
            TestDelegate action = () => new TripService(null, tripDAOMock);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Test]
        public void ShouldThrowANullArgumentExceptionWhenANullTripDAOIsAssigned()
        {
            TestDelegate action = () => new TripService(null, tripDAOMock);

            Assert.Throws<ArgumentNullException>(action);
        }


        [TestFixture]
        public class AndGettingTripsByUser : WhenWorkingWithTheTripService
        {            
            [SetUp]
            public void SetUpDifferentUsersTripsAndMocks()
            {
                SetupFriends();
                stranger = CreateUser();
                userSessionMock.GetLoggedUser().Returns(x => loggedInUser);

                trips = CreateTrips().ToList();
                tripDAOMock.FindTripsByUser(Arg.Any<User>()).Returns(x => trips);
            }

            [Test]
            public void ShouldThrowAUserNotLoggedInExceptionWhenUserIsNotLoggedInOrAGuest()
            {
                loggedInUser = guest;
                var subject = new TripService(userSessionMock, tripDAOMock);


                TestDelegate action = () => subject.GetTripsByUser(friend);


                Assert.Throws<UserNotLoggedInException>(action);
            }

            [Test]
            public void ShouldAlwaysCheckIfTheUserIsLoggedIn()
            {
                var subject = new TripService(userSessionMock, tripDAOMock);
                userSessionMock.Received(1);
            }

            [Test]
            public void ShouldFindTripsByUserWhenTheUserIsAFriend()
            {
                var subject = new TripService(userSessionMock, tripDAOMock);

                subject.GetTripsByUser(friend);

                tripDAOMock.Received(1);
            }

            [Test]
            public void ShouldNotFindTripsByUserWhenTheUserIsAStrangerAndShouldReturnAnEmptyTripsList()
            {
                var subject = new TripService(userSessionMock, tripDAOMock);

                var actualTrips = subject.GetTripsByUser(stranger);

                tripDAOMock.DidNotReceive();
                Assert.That(actualTrips, Is.Not.Null);
                CollectionAssert.IsEmpty(actualTrips);
            }
        }
    }
}