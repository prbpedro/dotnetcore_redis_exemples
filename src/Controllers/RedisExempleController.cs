using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace src.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisExempleController : ControllerBase
    {
        private readonly ILogger<RedisExempleController> _logger;

        public RedisExempleController(ILogger<RedisExempleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            ConnectionMultiplexer connectionRedis = ConnectionMultiplexer.Connect("localhost");
            Console.WriteLine(connectionRedis.IsConnected);

            IDatabase clientRedis = connectionRedis.GetDatabase();
            var key = new RedisKey("usuario:1");

            if (!clientRedis.KeyExists(key))
            {
                var HashSet = new List<HashEntry>();
                HashSet.Add(new HashEntry("nome", "Pedro"));
                HashSet.Add(new HashEntry("views", 1));
                clientRedis.HashSet(key, HashSet.ToArray());
            }
            else
            {
                clientRedis.HashIncrement(key, "views");
            }

            var hashUser = clientRedis.HashGetAll("usuario:1");

            return Ok();
        }
    }
}
