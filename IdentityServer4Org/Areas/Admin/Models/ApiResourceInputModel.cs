using System.ComponentModel.DataAnnotations;

namespace IdentityServer4Org.Areas.Admin.Models
{
    public class ApiResourceInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}
