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
		private readonly SignInManager<User> _signInManager;

		public AccountService(SignInManager<User> signInManager)
		{
			_signInManager = signInManager;
		}

		public async Task<BaseResponse> RegisterUser(RegisterUserRequest request)
		{
			User user = new User
			{
				UserName = request.Email,
				Email = request.Email,
				AccountConfirmed = false
			};

			string password = Constants.DEFAULT_PASSWORD;

			var result = await _signInManager.UserManager.CreateAsync(user, password);

			return new BaseResponse
			{
				IsSuccess = result.Succeeded
			};
		}

		public async Task<BaseResponse<string>> VerifyUser(string email, string password)
		{
			BaseResponse<string> response = new();

			var user = await _signInManager.UserManager.FindByEmailAsync(email);
			if(user is null)
			{
				response.ErrorMessage = "User is not found!";
				response.IsSuccess = false;
				return response;
			}

			var result = await _signInManager.UserManager.CheckPasswordAsync(user, password);
			response.IsSuccess = result;
			if (!result)
			{
				response.ErrorMessage = "Invalid Email or Password";
			} else
			{
				response.Value = user.UserName;
			}

			return response;
		}
	}
}
