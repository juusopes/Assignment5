using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace teht3.Controllers
{
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IRepository _repository;

        public PlayersController(ILogger<PlayersController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<Player> Get(Guid id)
        {
            return await _repository.Get(id);
        }

        [HttpGet]
        public async Task<Player[]> GetAll()
        {
            return await _repository.GetAll();
        }

        [HttpPost]
        public async Task<Player> Create([FromBody] NewPlayer newplayer)
        {
            Player _player = new Player();
            _player.Id = Guid.NewGuid();
            _player.Name = newplayer.Name;
            _player.CreationTime = DateTime.UtcNow;

            await _repository.Create(_player);

            return _player;
        }

        [HttpPost]
        [Route("modify/{id:Guid}")]
        public async Task<Player> Modify(Guid id, [FromBody] ModifiedPlayer player)
        {
            return await _repository.Modify(id, player);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<Player> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }

        [HttpGet]
        [Route("players?minScore=x")]
        public async Task<List<Player>> Ranges(int points)
        {
            return await _repository.Ranges(points);
        }

        [HttpGet]
        [Route("top10")]
        public async Task<List<Player>> Sorting()
        {
            return await _repository.Sorting();
        }

        [HttpGet]
        [Route("players?name=x")]
        public async Task<Player> SelectorMatching(string playerName)
        {
            return await _repository.SelectorMatching(playerName);
        }

        [HttpGet]
        [Route("players?name=x")]
        public async Task<List<Player>> SubDoc(ItemType itemType)
        {
            return await _repository.SubDoc(itemType);
        }
    }
}