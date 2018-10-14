using System;

namespace DockerNoSQL.WebAPI.Models
{
    public class PersonModel
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
    }
}