namespace AppDev.ViewModels
{
    public class SearchViewModel
    {
        public string? Type { get; set; }
        public string Action { get; private set; } = "";

        public string KeyWord { get; set; } = null!;


        public static SearchViewModel BookSearchViewModel { get; } = new()
        {
            Action = "/Books"
        };

        public static SearchViewModel ProfileSearchViewModel { get; } = new()
        {
            Action = "/Profile"
        };

        public static SearchViewModel OrderSearchViewModel { get; } = new()
        {
            Action = "/StoreOwner/Orders"
        };
    }
}
