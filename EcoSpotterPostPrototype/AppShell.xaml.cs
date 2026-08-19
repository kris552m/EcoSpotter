namespace EcoSpotterPostPrototype
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

#if !WINDOWS
            var mapTab = new Tab
            {
                Icon = "map.png",
                Route = "MapView",
            };
            mapTab.Items.Add(new ShellContent
            {
                ContentTemplate = new DataTemplate(typeof(View.MapView))
            });
            MainTabBar.Items.Insert(0, mapTab);
#endif
        }
    }
}
