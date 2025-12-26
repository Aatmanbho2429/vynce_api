namespace vynce_api.Model.Response
{
    public class MenuItemListResponse
    {
        public List<MenuItem> list { get; set; }
    }

    public class MenuItem
    {
        public string path { get; set; }
        public string icon { get; set; }
        public string title { get; set; }
        public int order { get; set; }
    }
}
