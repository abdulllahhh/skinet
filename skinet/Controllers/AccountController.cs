﻿using API.DTOs;
using API.Extentions;
using core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };
            var result = await signInManager.UserManager.CreateAsync(user, registerDto.password);
            if (! result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            return  Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false) return NoContent();

            var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                Address = user.Address?.ToDto()
            });
        }

        [HttpGet("auth-status")]
        public ActionResult GetAuthState()
        {
            return Ok(new
            {
                IsAuthnticated = User.Identity?.IsAuthenticated ?? false
            });
        }
        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
        {
            var user = await signInManager.UserManager.GetUserByEmailWithAddress(User);

            if (user.Address == null)
            {
                user.Address = addressDto.ToEntity();
            }
            else
            {
                user.Address.UpdateFromDto(addressDto);
            }

            var result = await signInManager.UserManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest("Problem updating user address");

            return Ok(user.Address.ToDto());
        }
    }

}
