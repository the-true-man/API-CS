using System;
using System.Net.Sockets;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using registration;
using registration.Classes;

namespace JSONAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("ListAllUsers")]
        public ActionResult<User> GetAllListUsers()
        {
            var users = FileSingleton.deserializeCoolection<User>(FileSingleton.pathToFileWithUsers);
            if (users == null || users.Count == 0)
            {
                return NotFound("Пользователи не найдены");
            }
            return Ok(users);

        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var userByID = FileSingleton.deserializeCoolection<User>(FileSingleton.pathToFileWithUsers).FirstOrDefault(u => id == u.Id);
            return userByID == null ? NotFound() : Ok(userByID);
        }

        [HttpPost("Auth")]
        public ActionResult<User> Auth([FromBody] LoginRequest loginRequest)
        {
            var allUsers = FileSingleton.deserializeCoolection<User>(FileSingleton.pathToFileWithUsers);
            var user_check = allUsers.FirstOrDefault(u => u.Email == loginRequest.Email);
            if(user_check == null )
            {
                return NotFound("Не верный логин или пароль!");
            }
            if (user_check.TimeBan > DateTime.Now)
            {
                return BadRequest("Пользователь заблокирован попробуйте позже");
            }
            user_check = allUsers.FirstOrDefault(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);
            if (user_check == null)
            {
                user_check = allUsers.FirstOrDefault(u => u.Email == loginRequest.Email);
                user_check.failTry += 1;
                userBanAndSerilize(user_check, FileSingleton.pathToFileWithUsers, allUsers);
                return NotFound("Не верный логин или пароль!");
            }

            return Ok(user_check);
        }

        [HttpPost("Registration")]
        public ActionResult<User> Registration([FromBody]User person)
        {
            var users = FileSingleton.deserializeCoolection<User>(FileSingleton.pathToFileWithUsers);
            if(users.FirstOrDefault(u => u.Email == person.Email) != null)
            {
                return BadRequest("Ошибка регистрации: данный email уже зарегистрирован!");
            }
            person.Id = users.Count>0 ? users.Max(u => u.Id)+1 : 1;

            users.Add(person);
            FileSingleton.serializeCollection<User>(FileSingleton.pathToFileWithUsers, users);
            return Ok(person);
        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            var users = FileSingleton.deserializeCoolection<User>(FileSingleton.pathToFileWithUsers);
            var userForDelete = users.FirstOrDefault(u => u.Id == id);
            if(userForDelete == null) {
                return NotFound("Пользователь не найден");
            }
            users.Remove(userForDelete);
            FileSingleton.serializeCollection(FileSingleton.pathToFileWithUsers, users);
            return Ok("Пользователь удален");

        }

        [HttpPut("Edit")]
        public ActionResult<User> Edit(int id, [FromBody] User person) 
        {
            var users = FileSingleton.deserializeCoolection<User>(FileSingleton.pathToFileWithUsers);
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }
            user.Name = person.Name;
            user.Lastname = person.Lastname;
            user.Midname = person.Midname;
            FileSingleton.serializeCollection(FileSingleton.pathToFileWithUsers, users);
            return Ok(user);
        }

        private void userBanAndSerilize(User user, string userFilePath, List<User> allUsers)
        {
            if (user.failTry == 10)
            {
                user.isBanned = true;
                user.TimeBan = DateTime.Now.AddMinutes(30);
                user.failTry = 0;
            }
            FileSingleton.serializeCollection(userFilePath, allUsers);
        }
    }
}
