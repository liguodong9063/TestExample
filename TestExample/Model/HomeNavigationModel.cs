using TestExample.Common;

namespace TestExample.Model
{
    public class HomeNavigationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public TestTypeEnum TestType { get; set; }
    }
}
