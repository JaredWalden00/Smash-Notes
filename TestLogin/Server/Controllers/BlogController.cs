using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestLogin.Server.Data;
using TestLogin.Shared;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using Azure.Core;
using TestLogin.Shared.Dto;

namespace TestLogin.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogControlService _blogControlService;
        public BlogController(IBlogControlService blogControlService)
        {
            _blogControlService = blogControlService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<BlogPost>>> CreateNewBlogPost(AddBlogPostDto request)
        {
            return Ok(await _blogControlService.CreateNewBlogPost(request));
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetBlogPostDto>>>> GimmeAllTheBlogPosts()
        {
            return Ok(await _blogControlService.GetAllBlogPost());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetBlogPostDto>>> GetBlogPostById(int id)
        {
            return Ok(await _blogControlService.GetBlogPostById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(UpdateBlogPostDto updatedPost)
        {
            return Ok(await _blogControlService.UpdateBlogPost(updatedPost));
        }
    }
}
