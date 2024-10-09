using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
	{
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<BaseResponse<string>> VerifyUser(string email, string password)
        {
            var response = new BaseResponse<string>();

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User not found.";
                return response;
            }

            // Check if the account is confirmed
            if (!user.AccountConfirmed)
            {
                response.IsSuccess = true;  // Login attempt was valid, but account isn't confirmed
                response.IsConfirmed = false;
                response.Value = user.Id;  // Return user ID in case we need it for redirection
                return response;
            }

            // Check if the password is correct
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                response.IsSuccess = true;
                response.ErrorMessage = "Login successful.";
                response.IsConfirmed = true;  // Mark the account as confirmed
                response.Value = user.Id;  // Optionally, return the user ID or other data
            }
            else
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Invalid password.";
            }

            return response;
        }



        // RegisterUser method - creates a new user
        public async Task<BaseResponse> RegisterUser(RegisterUserRequest request)
        {
            var response = new BaseResponse();

            // Create a new User entity
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                LicenceNumber = request.LicenceNumber,
                AccountConfirmed = false  // You can adjust this based on your logic
            };

            // Try to create the user
            var result = await _userManager.CreateAsync(user, Constants.DEFAULT_PASSWORD);
            if (result.Succeeded)
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return response;
        }

        public async Task<BaseResponse> ChangePassword(string email, string newPassword)
        {
            var response = new BaseResponse();

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User not found.";
                return response;
            }

            // If the user does not have a password, set a new one
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Failed to remove existing password.";
                    return response;
                }
            }

            // Add the new password
            var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (addPasswordResult.Succeeded)
            {
                // Set AccountConfirmed to true after password change
                user.AccountConfirmed = true;
                user.EmailConfirmed = true;

                // Update the user in the database
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Failed to update account confirmation status.";
                }
            }
            else
            {
                response.IsSuccess = false;
                response.ErrorMessage = string.Join(", ", addPasswordResult.Errors.Select(e => e.Description));
            }

            return response;
        }

    }
}
