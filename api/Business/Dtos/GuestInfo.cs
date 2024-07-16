namespace api.Business.Dtos
{
    public class GuestInfo
    {
        public string Id { get; set; } = string.Empty;
        public string NameGiven { get; set; } = string.Empty;
        public string NameFamily { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;

        public static string GetSortString(string sort)
        {
            string rs = string.Empty;
            switch (sort)
            {
                default:
                case "newest":
                    rs = "CreatedTS ASC";
                    break;
                case "oldest":
                    rs = "CreatedTS DESC";
                    break;
            }
            return rs;
        }
    }
}
