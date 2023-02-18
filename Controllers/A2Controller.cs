using Microsoft.AspNetCore.Mvc;
using A2.Data;
using A2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using A2.Handler;
using A2.Dtos;
using System.Security.Claims;

namespace A2.Controllers
{
    [Route("api")]
    [ApiController]
    public class A2Controller : Controller
    {
        private readonly IA2Repo _repository;
        public A2Controller(IA2Repo repository)
        {
            _repository = repository;
        }
        [HttpPost("Register")]
        public ActionResult Register(User user)
        {
            if(_repository.AddUser(user)){
                return Ok("User successfully registered.");
            }
            else
            {
                return Ok("Username not availiable");
            }
        }
        [Authorize(AuthenticationSchemes = "A2Auth")]
        [Authorize(Policy = "BasicAuth")]
        [HttpGet("GetVersionA")]
        public ActionResult GetVersionA()
        {
            return Ok("1.0.0 (auth)");
        }
        [Authorize(AuthenticationSchemes = "A2Auth")]
        [Authorize(Policy = "BasicAuth")]
        [HttpGet("PurchaseItem")]
        public ActionResult PurchaseItem(int id)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            string username = c.Value;
            Order o = new Order { productId = id, userName = username };
            return Ok(o);
        }
        [Authorize(AuthenticationSchemes = "A2Auth")]
        [Authorize(Policy = "BasicAuth")]
        [HttpGet("PairMe")]
        public ActionResult<GameRecordOut> PairMe()
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            string username = c.Value;
            GameRecordOut go = _repository.AvailableGame(username);
            return Ok(go);
        }
        [Authorize(AuthenticationSchemes = "A2Auth")]
        [Authorize(Policy = "BasicAuth")]
        [HttpGet("TheirMove")]
        public ActionResult TheirMove(string gameid)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            string username = c.Value;
            string result = _repository.getMove(gameid, username);
            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = "A2Auth")]
        [Authorize(Policy = "BasicAuth")]
        [HttpGet("MyMove")]
        public ActionResult MyMove(string gameid, string move)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            string username = c.Value;
            string result = _repository.MakeMove(gameid, username, move);
            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = "A2Auth")]
        [Authorize(Policy = "BasicAuth")]
        [HttpGet("QuitGame")]
        public ActionResult QuitGame(string gameid)
        {
            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("userName");
            string username = c.Value;
            string result = _repository.removeGame(gameid, username);
            return Ok(result);
        }
    }
}