using System.Collections.Generic;

namespace IdentityServer4Org.Areas.Admin.Models
{
    public class AdminHomeViewModel
    {
        public IList<UserViewModel> Users { get; set; }

        public IList<ApiResourceViewModel> ApiResources { get; set; }

        public AdminHomeViewModel()
        {
            Users = new List<UserViewModel>();
            ApiResources = new List<ApiResourceViewModel>();
        }
    }
}
