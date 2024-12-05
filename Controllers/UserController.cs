using System;
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
        public ActionResult<Person> GetAllListUsers()
        {
            var users = FileSingleton.deserializeCoolection<Person>(FileSingleton.pathToFileWithUsers);
            if (users == null || users.Count == 0)
            {
                return NotFound("Пользователи не найдены");
            }
            return Ok(users);

        }

        [HttpGet("{id}")]
        public ActionResult<Person> GetUserById(int id)
        {
            var userByID = FileSingleton.deserializeCoolection<Person>(FileSingleton.pathToFileWithUsers).FirstOrDefault(u => id == u.Id);
            return userByID == null ? NotFound() : Ok(userByID);
        }

        [HttpPost("Auth")]
        public ActionResult<Person> Auth([FromBody] LoginRequest loginRequest)
        {
            var allUsers = FileSingleton.deserializeCoolection<Person>(FileSingleton.pathToFileWithUsers);
            var user_check = allUsers.FirstOrDefault(u => u.email == loginRequest.Email);
            if(user_check == null )
            {
                return NotFound("Не верный логин или пароль!");
            }
            if (user_check.timeBan > DateTime.Now)
            {
                return BadRequest("Пользователь заблокирован попробуйте позже");
            }
            user_check = allUsers.FirstOrDefault(u => u.email == loginRequest.Email && u.password == loginRequest.Password);
            if (user_check == null)
            {
                user_check = allUsers.FirstOrDefault(u => u.email == loginRequest.Email);
                user_check.failTry += 1;
                userBanAndSerilize(user_check, FileSingleton.pathToFileWithUsers, allUsers);
                return NotFound("Не верный логин или пароль!");
            }

            return Ok(user_check);
        }

        [HttpPost("Registration")]
        public ActionResult<Person> Registration([FromBody]Person person)
        {
            var users = FileSingleton.deserializeCoolection<Person>(FileSingleton.pathToFileWithUsers);
            if(users.FirstOrDefault(u => u.email == person.email) != null)
            {
                return BadRequest("Ошибка регистрации: данный email уже зарегистрирован!");
            }
            person.Id = users.Count>0 ? users.Max(u => u.Id)+1 : 1;

            users.Add(person);
            FileSingleton.serializeCollection<Person>(FileSingleton.pathToFileWithUsers, users);
            return Ok(person);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            
        }

        private void userBanAndSerilize(Person user, string userFilePath, List<Person> allUsers)
        {
            if (user.failTry == 10)
            {
                user.isBanned = true;
                user.timeBan = DateTime.Now.AddMinutes(30);
                user.failTry = 0;
            }
            FileSingleton.serializeCollection(userFilePath, allUsers);

        }
    }
}
