using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MailTest.Models;
using MailTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace MailTest.Controllers
{
    [ApiController]
    [Route("/api/v1/mail")]
    public class MailController : Controller
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        
     
        
        [HttpPost]
        public async Task<ActionResult> SendMail([FromForm]Signin signin)
        {
            try
            {
                MailRequest request = new MailRequest();
                request.Subject = "User verification";
                request.ToEmail = signin.username;
                CreatePasswordHash(signin.password, out byte[] passwordHash, out byte[] passwordSalt);
                var url =
                    $"https://localhost:5001/api/v1/mail/signin-link?username={signin.username}&password={passwordHash}";

                request.Body = $"<h1>Da bi ste aktivirali račun <a href='{url}'>kliknite ovdje</a>.</h1>";
                await _mailService.SendEmailAsync(request);
                return Ok();
            }catch (Exception ex)
            {
                throw;
            }
            return Ok("ok");
        }
        
        [HttpGet("signin-link")]
        public ActionResult SigninLink([FromQuery]Signin signin)
        {
            //uraditi dodavanje racuna u bazu
            return Ok("ok link signin " + signin.username + " " + signin.password);
        }
        
        
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}