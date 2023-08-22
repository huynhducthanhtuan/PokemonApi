using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        ///<summary>Sign Up</summary>
        [HttpPost("signup")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register(SignupDTO signup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(signup.Email);
            if (user != null)
            {
                ModelState.AddModelError("error", "Email is existed");
                return Conflict(ModelState);
            }

            var newUser = new AppUser()
            {
                UserRole = signup.UserRole,
                Email = signup.Email,
                UserName = signup.Email.Split('@')[0]
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, signup.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, signup.UserRole);
                return NoContent();
            }
            else
            {
                var errorMessages = newUserResponse.Errors.Select(error => error.Description);
                ModelState.AddModelError("error", "Signup error: " + string.Join(", ", errorMessages));
                return BadRequest(ModelState);
            }
        }

        ///<summary>Sign In</summary>
        [HttpPost("signin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Signin(SigninDTO signin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(signin.Email);

            // User is found
            if (user != null)
            {
                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, signin.Password);

                if (isCorrectPassword)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, signin.Password, false, false);
                    if (result.Succeeded)
                    {
                        return NoContent();
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Signin error");
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Incorrect password");
                    return Unauthorized();
                }
            }
            else
            {
                ModelState.AddModelError("error", "User not found");
                return NotFound();
            }

        }

        ///<summary>Sign Out</summary>
        [HttpPost("signout")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [Authorize(Policy = "OwnerPolicy,ReviewerPolicy,AdminPolicy")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
