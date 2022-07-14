using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Domain.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RealEstate.Api.Controllers
{
    /*[Authorize]*/
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostService _postService;
        private IAuthService _authService;
        public PostsController(IPostService postService, IAuthService authService)
        {
            _postService = postService;
            _authService = authService;
        }
        // GET: api/<PostsController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilterParams filterParams) //test lai
        {
            var user = await _authService.ExistUser(filterParams.UserID);

            if (!string.IsNullOrEmpty(filterParams.UserID) && user == null)
                return NotFound();

            var result= await _postService.GetAllPosts(filterParams);
            return Ok(result);
        }


        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetPostsByUserCurrent()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");
            string userID = null;
            if (user != null)
                userID = user.Value;

            if (userID == null)
                return NotFound();
            var result = await _postService.GetPostsByUserCurrent(userID);
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetPostsWithReport() //lấy ra danh sách những bài post bị báo cáo
        {
            var result = await _postService.GetPostsWithReport();
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetPostsDueToExplain() //lấy ra danh sách những bài post đến hạn giải trình
        {
            var result = await _postService.GetPostsDueToExplain();
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPostsByUser(string id, string userCurrentID) //-------------------------test lại
        {
            var user = await _authService.ExistUser(id);
            var userCurrent = await _authService.ExistUser(userCurrentID);
            if (user == null)
                return NotFound();
            if(!string.IsNullOrEmpty(userCurrentID) && userCurrent==null)
                return NotFound();

            var result = await _postService.GetPostsByUser(id, userCurrentID);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id,string? userID) //-------------------------test lại
        {
            var user = await _authService.ExistUser(userID);
            if (!string.IsNullOrEmpty(userID) && user == null)
                return NotFound();

            var result = await _postService.GetPostById(id, userID);
            return Ok(result);
        }

        // GET api/<PostsController>/5
        [HttpGet("[action]")]
        public async Task<IActionResult> Search([FromQuery] FilterParams filterParams) //------------------------test lai
        {
            var user = await _authService.ExistUser(filterParams.UserID);

            if (!string.IsNullOrEmpty(filterParams.UserID) && user == null)
                return NotFound();

            var result = await _postService.Search(filterParams);
            return Ok(result);
        }

        // POST api/<PostsController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromForm] PostDto post)
        {
            var creatorID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            if (string.IsNullOrEmpty(post.CreatorID) || string.IsNullOrEmpty(creatorID) || !creatorID.Equals(post.CreatorID))
                return StatusCode(StatusCodes.Status404NotFound);

            if (ModelState.IsValid)
            {
                var result = await _postService.AddPost(post);
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPostTypeNumber()
        {
            var result = await _postService.GetPostTypeNumber();
            return Ok(result);
        }

        // PUT api/<PostsController>/5
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromForm] PostDto post)
        {
            var creatorID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            if (string.IsNullOrEmpty(post.CreatorID) || string.IsNullOrEmpty(creatorID) || !creatorID.Equals(post.CreatorID))
                return StatusCode(StatusCodes.Status404NotFound);

            if (ModelState.IsValid)
            {
                var result = await _postService.UpdatePost(post);
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status404NotFound);
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)                      //--------------------------------Can kiem tra nguoi xoa co phai la nguoi tao ra bai dang hay khong
        {
            var creatorID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            /*if (string.IsNullOrEmpty(creatorID))
                return StatusCode(StatusCodes.Status404NotFound);*/

            var result = _postService.DeletePost(id);
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Sold(int id)
        {
            var creatorID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            if (string.IsNullOrEmpty(creatorID))
                return StatusCode(StatusCodes.Status404NotFound);

            var result = await _postService.Sold(id, creatorID);
            return Ok(result);
        }
    }
}
