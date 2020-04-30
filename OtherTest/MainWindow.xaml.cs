using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OtherTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var i = new TestDto
            {
                HdrId = 1,
                HdrNo = "001",
                HdrName = "主单",
                DetailDto = new TestDetailDto
                {
                    DtlId=1,
                    DtlNo = "001",
                    DtlName="张三"
                }
            };

            var origDetail = i.DetailDto;
            var j = (TestBaseDto)i;
            var detail = j.DetailDto;
        }
    }

    public class TestDto:TestBaseDto
    {
        private TestDetailDto _detailDto;
        public int HdrId { get; set; }
        public new TestDetailDto DetailDto
        {
            get { return _detailDto;}
            set
            {
                base.DetailDto = value;
                _detailDto = value;
            }
        }
    }

    public class TestDetailDto: TestDetailBaseDto
    {
        public int DtlId { get; set; }
    }

    public class TestBaseDto
    {
        public int HdrId { get; set; }
        public string HdrNo { get; set; }
        public string HdrName { get; set; }
        public TestDetailBaseDto DetailDto { get; set; }


        public void SetDetailDto(TestDetailBaseDto detailDto)
        {
            DetailDto = detailDto;
        }
    }

    public class TestDetailBaseDto
    {
        public string DtlNo { get; set; }
        public string DtlName { get; set; }
    }
}
