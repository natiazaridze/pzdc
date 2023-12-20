using KartuliAPI1.Auth.Model;
using KartuliAPI1.Data.Dtos.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace KartuliAPI1.Auth
{
    public static class AuthEndpoints
    {
 /*       public static void AddAuthApi(this WebApplication app)
        {
            // register
            app.MapPost("api/register", async (UserManager<ForumRestUser> userManager, RegisterUserDto registerUserDto) =>
            {
                // check user exists
                var user = await userManager.FindByNameAsync(registerUserDto.Username);
                if (user != null)
                    return Results.UnprocessableEntity("user name already taken");

                var newUser = new ForumRestUser
                {
                    Email = registerUserDto.Email,
                    UserName = registerUserDto.Username
                };

                var createUserResult = await userManager.CreateAsync(newUser, registerUserDto.Password);
                if (!createUserResult.Succeeded)
                    return Results.UnprocessableEntity();

                await userManager.AddToRoleAsync(newUser, ForumRoles.ForumUser);

                return Results.Created("api/login", new UserDto(newUser.Id, newUser.UserName, newUser.Email));
            });*/
/*
            app.MapPost("api/login", async (UserManager<ForumRestUser> userManager, JwtTokenService jwtTokenService, LoginDto loginDto) =>
            {
                // check user exists
                var user = await userManager.FindByNameAsync(loginDto.Username);
                if (user == null)
                    return Results.UnprocessableEntity("Username or password was incorrect.");

                var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isPasswordValid)
                    return Results.UnprocessableEntity("Username or password was incorrect.");

                user.AdditionalInfo = false;
                await userManager.UpdateAsync(user);

                var roles = await userManager.GetRolesAsync(user);

                var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
                var refreshToken = jwtTokenService.CreateRefreshToken(user.Id);

                return Results.Ok(new SuccessfulLoginDto(accessToken, refreshToken));
            });*/

/*            // accessToken
            app.MapPost("api/accessToken",
                async (UserManager<ForumRestUser> userManager, JwtTokenService jwtTokenService, RefreshAccessTokenDto refreshAccessTokenDto) =>
                {
                    if (!jwtTokenService.TryParseRefreshToken(refreshAccessTokenDto.RefreshToken, out var claims))
                    {
                        return Results.UnprocessableEntity();
                    }

                    var userId = claims.FindFirstValue(JwtRegisteredClaimNames.Sub);

                    var user = await userManager.FindByIdAsync(userId);
                    if (user == null)
                    {
                        return Results.UnprocessableEntity("Invalid token");
                    }

                    if (user.AdditionalInfo)
                    {
                        return Results.UnprocessableEntity();
                    }

                    var roles = await userManager.GetRolesAsync(user);

                    var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
                    var refreshToken = jwtTokenService.CreateRefreshToken(user.Id);

                    return Results.Ok(new SuccessfulLoginDto(accessToken));
                });*/
        }
    }

    public record SuccessfulLoginDto(string AccessToken);

    public record UserDto(string UserId, string UserName, string Email);

    public record RegisterUserDto(string Username, string Email, string Password, string UserName);
    public record RefreshAccessTokenDto(string RefreshToken);


