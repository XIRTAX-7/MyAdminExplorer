using System;
using MyAdminExplorer.Models;

namespace MyAdminExplorer.Services
{
    public class AccessPolicyService
    {
        public AccessResult Evaluate(User user, DateTime now)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (IsWrongDay(user, now))
            {
                return user.Even ? AccessResult.DeniedOddDay : AccessResult.DeniedEvenDay;
            }

            var accessStart = user.From.Date.AddHours(user.FromTime);
            var accessEnd = user.To.Date.AddHours(user.ToTime);

            if (now < accessStart)
            {
                return AccessResult.DeniedNotYetActive;
            }

            if (now > accessEnd)
            {
                return AccessResult.DeniedExpired;
            }

            return AccessResult.Allowed;
        }

        private static bool IsWrongDay(User user, DateTime now)
        {
            var isEvenDay = now.Day % 2 == 0;
            return (isEvenDay && !user.Even) || (!isEvenDay && user.Even);
        }

        public string GetDeniedMessage(User user, AccessResult result)
        {
            switch (result)
            {
                case AccessResult.DeniedOddDay:
                    return "Today is an odd day.\nYour day is tomorrow";
                case AccessResult.DeniedEvenDay:
                    return "Today is an even day.\nYour day is tomorrow";
                case AccessResult.DeniedExpired:
                case AccessResult.DeniedNotYetActive:
                    return string.Format(
                        "Access is denied.\nYour terms from {0}/{1}/{2} {3}:00 to {4}/{5}/{6} {7}:00",
                        user.From.Day,
                        user.From.Month,
                        user.From.Year,
                        user.FromTime,
                        user.To.Day,
                        user.To.Month,
                        user.To.Year,
                        user.ToTime);
                default:
                    return "Access is denied.";
            }
        }
    }
}
