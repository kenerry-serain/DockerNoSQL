using System.Threading.Tasks;
using DockerNoSQL.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DockerNoSQL.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        public ClientController(IConfiguration configuration)
        {
            _mongoClient = new MongoClient(configuration.GetConnectionString("Development"));
            _mongoDatabase = _mongoClient.GetDatabase("Database");
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var collection = _mongoDatabase.GetCollection<PersonModel>("Person");
            var @object = await (await collection.FindAsync(filter => filter.Id == id)).FirstOrDefaultAsync();
            if (@object is null)
                return NotFound();
            return Ok(@object);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Insert([FromBody]PersonModel personModel)
        {
            var collection = _mongoDatabase.GetCollection<PersonModel>("Person");
            await collection.InsertOneAsync(personModel);
            return CreatedAtAction(nameof(GetById), new { id = personModel.Id }, personModel);
        }
        
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            var collection = _mongoDatabase.GetCollection<PersonModel>("Person");
            var @object = await (await collection.FindAsync(filter => filter.Id == id)).FirstOrDefaultAsync();
            if (@object is null)
                return NotFound();
            await collection.DeleteOneAsync(filter => filter.Id == id);
            return Ok();
        }
    }
}