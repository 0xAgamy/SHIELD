using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shield.Data;
using Shield.IRepos;
using System.Reflection.Metadata.Ecma335;

namespace Shield.Controllers
{
	[Route("api/[controller]")]
    [Authorize(Roles ="admin")]
    [ApiController]
	public class RoleController : ControllerBase
	{
		private readonly IRoleRepo _roleRepo;
		public RoleController(IRoleRepo roleRepo)
		{
			_roleRepo = roleRepo;
		}


		[HttpGet]


        public async Task<IActionResult> GetAllRole()
		{

			try
			{
				return Ok(await _roleRepo.GetRoles());
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("{rolename}")]


        public async Task<IActionResult> CreateRole(string rolename)
		{
			try
			{
				return Ok(await _roleRepo.CreateRole(rolename));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpDelete("{roleId}")]


        public async Task<IActionResult> DeleteRole(string roleId)
		{
			try
			{
				return Ok(await _roleRepo.DeleteRole(roleId));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);


			}
		}

        // User-Roles

        [HttpGet("user")]
		public async Task<IActionResult>GetUserRoles(string userId)
		{
			try
			{
				return Ok(await _roleRepo.GetUserRole(userId));

			}catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}


		}


        [HttpPost("user")]
		public async Task<IActionResult> AddUserToRole(string userid, string role)
		{
			try
			{
				return Ok(await _roleRepo.AddUserToRole(userid, role));
			}
			catch(Exception ex) { return BadRequest(ex.Message); }
		}

		[HttpDelete("user")]

        public async Task<IActionResult> DeleteUserFromRole(string userid, string role)
		{
			try
			{
				return Ok(await _roleRepo.RemoveUserFromRole(userid, role));

			}catch (Exception ex)
			
				{
				return BadRequest(ex.Message);
				}

		}
		[HttpPut("user")]

        public async Task<IActionResult> ChangeUserRole(string userId, string OldRole,string NewRole)
		{
			try
			{
				return Ok(await _roleRepo.ChangeUserRole(userId, OldRole,NewRole));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
