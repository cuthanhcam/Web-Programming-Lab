using System;
using System.Collections.Generic;

namespace Lab05.WebsiteBanHang.Models
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public List<ApplicationUser> RecentUsers { get; set; }
    }
} 