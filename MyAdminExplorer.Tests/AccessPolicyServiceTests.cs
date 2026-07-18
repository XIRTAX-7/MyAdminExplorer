using System;
using FluentAssertions;
using MyAdminExplorer.Models;
using MyAdminExplorer.Services;
using Xunit;

namespace MyAdminExplorer.Tests
{
    public class AccessPolicyServiceTests
    {
        private readonly AccessPolicyService _service = new AccessPolicyService();

        private static User CreateUser(bool even, DateTime from, DateTime to, int fromTime, int toTime)
        {
            return new User
            {
                Even = even,
                From = from,
                To = to,
                FromTime = fromTime,
                ToTime = toTime
            };
        }

        [Fact]
        public void Evaluate_AllowsUserOnMatchingEvenDay()
        {
            var user = CreateUser(true, new DateTime(2024, 6, 1), new DateTime(2024, 6, 30), 0, 24);
            var now = new DateTime(2024, 6, 2, 12, 0, 0);

            _service.Evaluate(user, now).Should().Be(AccessResult.Allowed);
        }

        [Fact]
        public void Evaluate_DeniesUserOnWrongParityDay()
        {
            var user = CreateUser(true, new DateTime(2024, 6, 1), new DateTime(2024, 6, 30), 0, 24);
            var now = new DateTime(2024, 6, 1, 12, 0, 0);

            _service.Evaluate(user, now).Should().Be(AccessResult.DeniedOddDay);
        }

        [Fact]
        public void Evaluate_DeniesExpiredAccess()
        {
            var user = CreateUser(true, new DateTime(2024, 6, 1), new DateTime(2024, 6, 10), 0, 10);
            var now = new DateTime(2024, 6, 10, 11, 0, 0);

            _service.Evaluate(user, now).Should().Be(AccessResult.DeniedExpired);
        }

        [Fact]
        public void Evaluate_DeniesBeforeActiveWindow()
        {
            var user = CreateUser(false, new DateTime(2024, 6, 5), new DateTime(2024, 6, 30), 9, 18);
            var now = new DateTime(2024, 6, 5, 8, 30, 0);

            _service.Evaluate(user, now).Should().Be(AccessResult.DeniedNotYetActive);
        }
    }
}
