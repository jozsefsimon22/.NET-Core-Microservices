using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, AppDbContext db, IJwtGenerator jwtGenerator)
    {
        _db = db;
        _jwtGenerator = jwtGenerator;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new()
        {
            UserName = registrationRequestDto.Email,
            Email = registrationRequestDto.Email,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            Name = registrationRequestDto.Name,
            PhoneNumber = registrationRequestDto.PhoneNumber
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (result.Succeeded)
            {
                var userToReturn = _db.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);

                UserDto userDto = new()
                {
                    ID = userToReturn.Id,
                    Email = userToReturn.Email,
                    Name = userToReturn.Name,
                    PhoneNumber = userToReturn.PhoneNumber
                };

                return "";
            }
            else
            {
                return result.Errors.FirstOrDefault()!.Description;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        } 

        return "ErrorEncountered";
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        // var user = _db.ApplicationUsers.FirstOrDefault(u =>
        //     string.Equals(u.UserName, loginRequestDto.UserName, StringComparison.OrdinalIgnoreCase));

        var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (user == null || isValid == false)
        {
            return new LoginResponseDto() { User = null, Token = "" };
        }

        //if user was found generate JWT

        var token = _jwtGenerator.GenerateToken(user);

        UserDto userDto = new()
        {
            Email = user.Email,
            ID = user.Id,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber
        };

        LoginResponseDto loginResponseDto = new()
        {
            User = userDto,
            Token = token
        };

        return loginResponseDto;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        if (user != null)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }

            await _userManager.AddToRoleAsync(user, roleName);

            return true;
        }

        return false;

    }
}