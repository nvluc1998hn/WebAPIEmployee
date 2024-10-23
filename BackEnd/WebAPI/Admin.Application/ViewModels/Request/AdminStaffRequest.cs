namespace Admin.Application.ViewModels.Request
{
    public class AdminStaffRequest
    {
        public int Id { get; set; }

        public string StaffCode { get; set; }

        public string StaffName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Image { get; set; }

        public int Part { get; set; }

        public int Sex { get; set; }

        public Guid CreateByUser { get; set; }

        public Guid UpdateByUser { get; set; }
    }
}
