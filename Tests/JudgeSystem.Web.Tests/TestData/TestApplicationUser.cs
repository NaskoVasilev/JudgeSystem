using System;

using JudgeSystem.Data.Models;

using MyTested.AspNetCore.Mvc;

namespace JudgeSystem.Web.Tests.TestData
{
    public static class TestApplicationUser
    {
        public const string Id = TestUser.Identifier;

        public const string Username = TestUser.Username;

        public const string Email = "test@email.com";

        public const string Name = "test_name";

        public const string Surname = "test_surname";

        public const string Password = "T3st_P@ss";

        public const string PasswordHash = "AQAAAAEAACcQAAAAEMdWe4GUIMaegFzcl0AtykG1JxIZ98PuZJUbikPQ62KD7I5luuGmv1rgE5s6sdsX6g==";

        public static ApplicationUser GetDefaultUser() => new ApplicationUser
        {
            Id = Id,
            UserName = Username,
            Email = Email,
            Name = Name,
            Surname = Surname,
            PasswordHash = PasswordHash,
            EmailConfirmed = true,
            NormalizedUserName = Username.ToUpper(),
            SecurityStamp = Guid.NewGuid().ToString()
        };
    }
}
