using AutoFixture;
using Microsoft.AspNetCore.Identity;

namespace Todo.Tests.CustomFixture
{
    public class IdentityUserFixture : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<IdentityUser>(composer =>
                composer.FromFactory(() =>
                {
                    var user = new IdentityUser
                    {
                        UserName = "alice@example.com",
                        Email = "alice@example.com",
                        EmailConfirmed = true
                    };
                    return user;
                }));
        }
    }
}
