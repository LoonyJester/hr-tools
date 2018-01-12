using System.ComponentModel.DataAnnotations;

namespace HRTools.AuthorizationServer.Entities
{
    public class Client
    {
        [Key]
        [MaxLength(250)]
        public string Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Secret { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        //public ApplicationTypes ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        [MaxLength(100)]
        public string AllowedOrigin { get; set; }
    }
}