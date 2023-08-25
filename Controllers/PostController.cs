using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PostController : BaseController
    {
        private readonly DataContextDapper _dapper;

        public PostController(IConfiguration config){
            _dapper = new DataContextDapper(config);
        }    

        [HttpGet]
        public IEnumerable<Post> GetPosts(){
            return _dapper.LoadData<Post>("SELECT * FROM TutorialAppSchema.Posts");
        }

        [HttpGet("{id}")]
        public Post GetOnePost(int id){
            return _dapper.LoadDataSingle<Post>($"SELECT * FROM TutorialAppSchema.Posts WHERE PostId = '{id}'");
        }

        [HttpGet("user/{id}")]
        public IEnumerable<Post> GetAlluserPosts(int id){
            return _dapper.LoadData<Post>($"SELECT * FROM TutorialAppSchema.Posts WHERE UserId = '{id}'");
        }

        [HttpGet("myposts")]
        public IEnumerable<Post> GetMyPost(){
            return _dapper.LoadData<Post>($"SELECT * FROM TutorialAppSchema.Posts WHERE UserId = '{User.FindFirst("userId").Value}'");
        }

        [HttpGet("search/{param}")]
        public IEnumerable<Post> GetBySearch(string param){
            return _dapper.LoadData<Post>($"SELECT * FROM TutorialAppSchema.Posts WHERE PostTitle LIKE '%{param}%' OR PostContent LIKE '%{param}%' ");
        }

        [HttpPost]
        public IActionResult AddPost(AddPostDTO model){
            string sql = $@"INSERT INTO TutorialAppSchema.Posts (UserId, PostTitle, PostContent, PostCreated, PostUpdated)
            VALUES ('{User.FindFirst("userId").Value}', '{model.PostTitle}', '{model.PostContent}', GETDATE(), GETDATE() )";

            if(_dapper.ExecuteSql(sql)){
                return Ok("Post created");
            }
            throw new Exception("Error");
        }

        [HttpPut("{id}")]
        public IActionResult EditPost(EditPostDTO model, int id){
            string sql = $@"UPDATE TutorialAppSchema.Posts SET
            PostContent = '{model.PostContent}', PostTitle = '{model.PostTitle}', PostUpdated = GETDATE() WHERE PostId = '{id}' 
            AND UserId = '{User.FindFirst("userId").Value}' ";

            if(_dapper.ExecuteSql(sql)){
                return Ok("Post updated");
            }
            throw new Exception("Error updating");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id){
            string sql = $@"DELETE FROM TutorialAppSchema.Posts WHERE PostId = '{id}' AND UserId = '{User.FindFirst("userId").Value}'";
            
            if(_dapper.ExecuteSql(sql)){
                return Ok("Post deletd");
            }
            throw new Exception("Error");
        }
    }
}