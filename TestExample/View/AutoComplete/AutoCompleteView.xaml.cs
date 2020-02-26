using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TestExample.Common;
using TestExample.Model.AutoCompleteTextBox;
using TestExample.ViewModel.AutoComplete;
using WpfControls.Editors;

namespace TestExample.View.AutoComplete
{
    /// <summary>
    /// AutoCompleteView.xaml 的交互逻辑
    /// </summary>
    public partial class AutoCompleteView : Window
    {
        private bool _hasAutoCompleteSelectionChanged = false;

        public AutoCompleteView()
        {
            InitializeComponent();

            //AutoComplete.CanTrigger = () =>
            //{
            //    var filter = AutoComplete.Editor.Text.Trim() ?? "";
            //    if (filter.Length >= 2)
            //    {
            //        var regex = new Regex("^(([\u4e00-\u9fa5]{2,30})|([\u4e00-\u9fa5]{2,26}（+[\u4e00-\u9fa5]{2,26}）[\u4e00-\u9fa5]{0,24})|([\u4e00-\u9fa5]{2,26}\\(+[\u4e00-\u9fa5]{2,26}\\)[\u4e00-\u9fa5]{0,24}))$");
            //        if (regex.IsMatch(filter))
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //};
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as AutoComplete2ViewModel;
            dataContext.TestObject = new AutoCompleteIdNameDto();
            MessageBox.Show("id:" + dataContext.TestObject?.Id + ";name:" + dataContext.TestObject?.Name);
        }

        private void AutoComplete_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            _hasAutoCompleteSelectionChanged = true;
            var dataContext = DataContext as AutoComplete2ViewModel;
            if (dataContext == null) return;
            var autoCompleteTextBox = sender as AutoCompleteTextBox;
            if (autoCompleteTextBox == null) return;
            var selectedItem = autoCompleteTextBox.SelectedItem as IdNameObject;
            //清除对象ID
            dataContext.TestObject.Id = selectedItem?.Id;
            Console.WriteLine("当前选中项：" + selectedItem?.Name + "(" + DateTime.Now + ")");
        }

        //清除自动完成控件显示文本内容
        private void AutoComplete_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var autoCompleteTextBox = sender as AutoCompleteTextBox;
            if (autoCompleteTextBox == null) return;
            var selectedItem = autoCompleteTextBox.SelectedItem;
            if (_hasAutoCompleteSelectionChanged && selectedItem == null)
            {
                //清除显示文本
                autoCompleteTextBox.Text = string.Empty;
            }
            _hasAutoCompleteSelectionChanged = false;
        }
    }

    public class FilesystemSuggestionProvider : ISuggestionProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private List<IdNameObject> _allDataItems = new List<IdNameObject>
        {
            new IdNameObject(1935,"中国中铁七局集团有限公司（三环隧道）"),
            new IdNameObject(1936,"泰富重工制造有限公司"),
            new IdNameObject(1937,"湖南天时利投资置业有限公司"),
            new IdNameObject(1938,"衡阳市联嘉贸易有限公司"),
            new IdNameObject(1939,"湖南中联重科混凝土机械站类设备有限公司"),
            new IdNameObject(1940,"湖南瑞新机械制造有限公司"),
            new IdNameObject(1941,"浙江城建建设集团有限公司（东湖和庭创新城）"),
            new IdNameObject(1942,"湖南华菱湘潭钢铁有限公司"),
            new IdNameObject(1943,"湖南省联创冶金实业有限公司（十四局）"),
            new IdNameObject(1944,"湖南省联创冶金实业有限公司（二十五局）"),
            new IdNameObject(1945,"湘潭电机力源模具有限公司"),
            new IdNameObject(1946,"湖南大兴能源贸易有限公司"),
            new IdNameObject(1947,"中国十五冶金建设集团有限公司（康桥水岸）"),
            new IdNameObject(1948,"中联重科股份有限公司建筑起重机械分公司"),
            new IdNameObject(1949,"永州市华诚贸易有限公司"),
            new IdNameObject(1950,"娄底市鑫德利金属材料有限公司"),
            new IdNameObject(1951,"长沙劲勇能源科技开发有限公司"),
            new IdNameObject(1952,"长沙鑫天置业发展有限公司（格林香山）"),
            new IdNameObject(1953,"中联重科股份有限公司路面机械分公司"),
            new IdNameObject(1954,"湖南金富祥贸易有限公司"),
            new IdNameObject(1955,"湖南中联重科车桥有限公司"),
            new IdNameObject(1956,"湖南尧舜禹经贸有限公司"),
            new IdNameObject(1957,"中联重科股份有限公司江阴分公司"),
            new IdNameObject(1958,"长沙德飞钢铁贸易有限公司"),
            new IdNameObject(1959,"湖南融智信达投资有限公司"),
            new IdNameObject(1960,"中联重科股份有限公司环卫分公司"),
            new IdNameObject(1961,"湖南福聚科技有限公司"),
            new IdNameObject(1962,"湖南省建设物资实业有限公司"),
            new IdNameObject(1963,"湖南大强钢铁贸易有限公司"),
            new IdNameObject(1964,"武汉厚邦商贸有限公司"),
            new IdNameObject(1965,"湖南联欣工贸有限公司"),
            new IdNameObject(1966,"湖南博长建材物流有限公司"),
            new IdNameObject(1967,"湘潭电机股份有限公司"),
            new IdNameObject(1968,"中联重科股份有限公司沅江分公司"),
            new IdNameObject(1969,"湖南键锋钢材贸易有限公司"),
            new IdNameObject(1970,"湖南九龙长沙贸易有限公司"),
            new IdNameObject(1971,"湖南诚亿建材科技有限公司"),
            new IdNameObject(1972,"湖南湘博钢贸有限公司"),
            new IdNameObject(1973,"南阳汉冶特钢有限公司"),
            new IdNameObject(1974,"长沙市丰钛钢材贸易有限公司"),
            new IdNameObject(1975,"湖南恩瑞钢铁有限公司"),
            new IdNameObject(1976,"湖南博长钢铁贸易有限公司"),
            new IdNameObject(1977,"长沙市顶扬钢铁贸易有限公司"),
            new IdNameObject(1978,"湖南金远钢材有限公司"),
            new IdNameObject(1979,"湖南盛安商贸有限公司"),
            new IdNameObject(1980,"湖南首岳钢铁贸易有限公司"),
            new IdNameObject(1981,"五矿钢铁（武汉）有限公司"),
            new IdNameObject(1982,"湖南万程钢铁贸易有限公司"),
            new IdNameObject(1983,"湖南静宁钢铁贸易有限公司"),
            new IdNameObject(1984,"湖南联振钢铁贸易有限公司"),
            new IdNameObject(1985,"湖南润长贸易有限公司"),
            new IdNameObject(1987,"湖南联正仓储有限公司"),
            new IdNameObject(1988,"湖南一力股份有限公司"),
            new IdNameObject(1989,"娄底市四方物流有限公司"),
            new IdNameObject(1990,"长沙百川物流有限公司"),
            new IdNameObject(1991,"柳州市途锐汽车运输有限公司"),
            new IdNameObject(1992,"广西柳钢物流有限责任公司"),
            new IdNameObject(1993,"长沙百川物流有限公司（建材）"),
            new IdNameObject(1994,"中国外运广西公司柳州分公司"),
            new IdNameObject(1995,"柳州钢铁股份有限公司（中板）"),
            new IdNameObject(1996,"柳州钢铁股份有限公司"),
            new IdNameObject(1997,"湖南力邦物流有限公司"),
            new IdNameObject(1998,"湖南国钢工贸有限公司"),
            new IdNameObject(1999,"湖南博信钢铁贸易有限公司"),
            new IdNameObject(2000,"上海正楚工贸有限公司"),
            new IdNameObject(2001,"长沙市灏健物流有限公司"),
            new IdNameObject(2002,"个体司机运费"),
            new IdNameObject(2003,"武汉华冶物资有限公司"),
            new IdNameObject(2004,"广州铁路（集团）公司湘潭东车站-货"),
            new IdNameObject(2005,"湖南涟钢钢材加工配送有限公司"),
            new IdNameObject(2006,"新余市宏跃贸易商行"),
            new IdNameObject(2007,"广西柳州钢铁(集团)公司"),
            new IdNameObject(2008,"湖南众友实业有限公司"),
            new IdNameObject(2011,"湖南弘翔金属材料有限公司"),
            new IdNameObject(2012,"上海钢银电子商务股份有限公司"),
            new IdNameObject(2013,"长沙创凯机电设备有限公司"),
            new IdNameObject(2014,"湖南吉成贸易有限公司"),
            new IdNameObject(2017,"湖南万江达钢材工贸有限公司"),
            new IdNameObject(2018,"湖南盛仕通钢铁贸易有限公司"),
            new IdNameObject(2020,"湖南鑫和佳美钢材贸易有限公司"),
            new IdNameObject(2021,"湖南炜强物资有限公司"),
            new IdNameObject(2022,"长沙市攀升金属材料贸易有限公司"),
            new IdNameObject(2023,"湖南申华贸易有限公司"),
            new IdNameObject(2024,"长沙市志鼎钢铁贸易有限公司"),
            new IdNameObject(2025,"衡山县中山钢管有限公司"),
            new IdNameObject(2026,"湖南隆纳贸易有限公司"),
            new IdNameObject(2027,"湖南国恒钢铁贸易有限公司"),
            new IdNameObject(2028,"武汉市晓俊物资有限公司"),
            new IdNameObject(2029,"长沙市洺顺钢材贸易有限公司"),
            new IdNameObject(2030,"湖南博通经贸有限公司"),
            new IdNameObject(2031,"大汉物流股份有限公司"),
            new IdNameObject(2032,"湖南涟商益源科技发展有限公司"),
            new IdNameObject(2035,"湖南金诚钢铁贸易有限公司"),
            new IdNameObject(2126,"长沙中联重工科技发展股份有限公司汉寿分公司"),
            new IdNameObject(2127,"长沙盈金物流有限公司"),
            new IdNameObject(2128,"长沙市新燃金属材料有限公司"),
            new IdNameObject(2131,"广西柳州外运有限责任公司"),
            new IdNameObject(2241,"湖南永清机械制造有限公司"),
            new IdNameObject(2253,"湖南邦鼎贸易有限公司"),
            new IdNameObject(2256,"南京宇飞金属材料有限公司"),
            new IdNameObject(2260,"湖南方成贸易有限公司"),
            new IdNameObject(2278,"湖南炜大物资贸易有限公司"),
            new IdNameObject(2284,"长沙市飞尔达不锈钢有限公司"),
            new IdNameObject(2295,"湖南中拓建工物流有限公司"),
            new IdNameObject(2296,"湖南联润工贸有限公司"),
            new IdNameObject(2297,"湖南赛恩斯科技发展有限公司"),
            new IdNameObject(2299,"湖南荣创贸易有限公司"),
            new IdNameObject(2301,"武汉市侠义运输有限公司"),
            new IdNameObject(2302,"湖南新大为物流有限责任公司"),
            new IdNameObject(2303,"长沙市新锐金属材料有限公司"),
            new IdNameObject(2317,"长沙市国恒钢铁贸易有限公司"),
            new IdNameObject(2320,"个体"),
            new IdNameObject(2322,"益阳市鑫盛建材贸易有限公司"),
            new IdNameObject(2323,"湖南翌翔物资有限公司"),
            new IdNameObject(2326,"湖南秀会钢铁销售有限公司"),
            new IdNameObject(2329,"中建钢构阳光惠州有限公司"),
            new IdNameObject(2332,"桂林城建金属材料有限公司（东湖和庭创新城）"),
            new IdNameObject(2335,"中铁五局（集团）有限公司贵广铁路工程指挥部物资办事处"),
            new IdNameObject(2336,"湖南省联创冶金实业有限公司"),
            new IdNameObject(2352,"长沙鹏湘金属材料有限公司"),
            new IdNameObject(2353,"湖南湘钢洪盛物流有限公司"),
            new IdNameObject(2372,"湖南顺昌不锈钢有限公司"),
            new IdNameObject(2373,"长沙市地球村钢铁贸易有限公司"),
            new IdNameObject(2375,"长沙物资贸易有限公司"),
            new IdNameObject(2376,"湖南宁钢物流有限公司"),
            new IdNameObject(2377,"长沙市泰能贸易有限公司"),
            new IdNameObject(2379,"湖南志和工贸有限公司"),
            new IdNameObject(2384,"湖南湘钢工贸有限公司一分公司"),
            new IdNameObject(2393,"山西同元实业集团有限公司"),
            new IdNameObject(2394,"长沙天祥物流有限公司"),
            new IdNameObject(2395,"湖南东锋钢铁有限公司"),
            new IdNameObject(2396,"长沙朗锐金属材料贸易有限公司"),
            new IdNameObject(34893,"长沙漆威金属材料贸易有限公司"),
            new IdNameObject(34898,"个体司机运费（建材）"),
            new IdNameObject(34905,"湖南金志杰钢铁贸易有限公司"),
            new IdNameObject(34908,"湖南创进金属材料有限公司"),
            new IdNameObject(34910,"湖南一恒经贸有限公司"),
            new IdNameObject(34913,"湖南荣航钢材贸易有限公司"),
            new IdNameObject(34917,"湖南华测创时检测技术有限公司"),
            new IdNameObject(34918,"湖南联博贸易有限公司"),
            new IdNameObject(34921,"长沙市高晟金属材料有限公司"),
            new IdNameObject(34922,"湖南鑫长顺经贸有限公司"),
            new IdNameObject(34924,"北京龙源昊泰贸易有限公司"),
            new IdNameObject(34925,"长沙盛龙金属材料有限公司"),
            new IdNameObject(34927,"怀化联怀贸易有限公司"),
            new IdNameObject(34932,"长沙泰松金属材料贸易有限公司"),
            new IdNameObject(34940,"湖南本铁商贸有限公司"),
            new IdNameObject(34941,"湖南钢泰商贸有限公司"),
            new IdNameObject(34943,"物产中拓股份有限公司"),
            new IdNameObject(34961,"永州市永信电力设备制造有限公司"),
            new IdNameObject(34966,"长沙泽冶钢铁贸易有限公司"),
            new IdNameObject(34980,"长沙敏功贸易有限公司"),
            new IdNameObject(34981,"湖南嘉威商贸有限公司"),
            new IdNameObject(35416,"长沙市旨新金属材料有限公司"),
            new IdNameObject(35422,"湖南佳阳建设长沙东湖和庭创新城项目部"),
            new IdNameObject(35446,"长沙市天心区建厦钢材经营部"),
            new IdNameObject(35454,"湖南陆国贸易有限公司"),
            new IdNameObject(35472,"湖南省磊鑫贸易有限公司"),
            new IdNameObject(35475,"湖南琳骏经贸有限公司"),
            new IdNameObject(35485,"武汉市正板物资有限公司"),
            new IdNameObject(35493,"湖南北山建设集团股份有限公司"),
            new IdNameObject(35494,"湖南联创矿冶有限公司"),
            new IdNameObject(35498,"湖南五洲长和钢贸有限公司"),
            new IdNameObject(35499,"湖南顺星不锈钢有限公司"),
            new IdNameObject(35551,"中冶京诚(湘潭)重工设备有限公司"),
            new IdNameObject(35565,"湖南双金贸易有限公司"),
            new IdNameObject(35566,"长沙金泽金属材料有限公司"),
            new IdNameObject(35573,"长沙旺利钢铁贸易有限公司"),
            new IdNameObject(35619,"湖南壹佰钢铁贸易有限公司"),
            new IdNameObject(35629,"湖南中钢物资贸易有限公司"),
            new IdNameObject(35634,"湖南金烜建材贸易有限公司"),
            new IdNameObject(35657,"湘潭瑞通球团有限公司"),
            new IdNameObject(35661,"长沙市恒力金属材料有限公司"),
            new IdNameObject(35662,"湖南博通经贸有限公司（工程）"),
            new IdNameObject(35692,"湖南嘉运汇通贸易有限公司"),
            new IdNameObject(35694,"湖南亚旅贸易有限公司"),
            new IdNameObject(35738,"湖南广大工贸有限公司"),
            new IdNameObject(35766,"长沙欣昌钢铁贸易有限公司"),
            new IdNameObject(35838,"湖南金晟工贸有限公司"),
            new IdNameObject(35840,"陕西联旺工贸有限公司"),
            new IdNameObject(35849,"湖南华苏建材有限公司"),
            new IdNameObject(35859,"湖南佩念钢材贸易有限公司"),
            new IdNameObject(35864,"武汉天炙钢物资有限公司"),
            new IdNameObject(35870,"湖南祁阳华泰钢瓶制造有限公司"),
            new IdNameObject(35876,"娄底市鸿大机电设备有限公司"),
            new IdNameObject(35885,"湖南大丰物流有限公司"),
            new IdNameObject(35886,"湖南福云物资贸易有限公司"),
            new IdNameObject(35900,"湖南省湘天建设工程有限公司"),
            new IdNameObject(35903,"湖南创步科技有限公司"),
            new IdNameObject(35908,"湖南大汉型板钢铁贸易有限公司"),
            new IdNameObject(35940,"柳州市津海航运有限公司"),
            new IdNameObject(35943,"湖南全拓贸易有限公司"),
            new IdNameObject(35949,"湖南新旨新经贸有限公司"),
            new IdNameObject(35955,"湖南丰辉金属材料贸易有限公司"),
            new IdNameObject(35960,"湖南和昌钢铁贸易有限公司"),
            new IdNameObject(35968,"湖南鸿云丰投资管理有限公司"),
            new IdNameObject(35972,"长沙耀鸿金属材料贸易有限公司"),
            new IdNameObject(35997,"湖南凯乐建设工程有限公司辰州中央广场工程项目部"),
            new IdNameObject(36003,"湖南诚谊经贸有限公司"),
            new IdNameObject(36004,"湖南鑫昊钢铁贸易有限公司")
        };

        private List<IdNameObject> _grades = new List<IdNameObject>();

        public int? AccountingTypeId { get; set; }

        public System.Collections.IEnumerable GetSuggestions(string filter)
        {


            //if (filter?.Trim().Length >= 2)
            //{
            //    var regex = new Regex("^(([\u4e00-\u9fa5]{2,30})|([\u4e00-\u9fa5]{2,26}（+[\u4e00-\u9fa5]{2,26}）[\u4e00-\u9fa5]{0,24})|([\u4e00-\u9fa5]{2,26}\\(+[\u4e00-\u9fa5]{2,26}\\)[\u4e00-\u9fa5]{0,24}))$");
            //    if (regex.IsMatch(filter))
            //    {
            //        if (_grades.Count(a => a.Name.Contains(filter)) == 0)
            //        {
            //            _grades = _allDataItems.Where(a => a.Name.Contains(filter)).Take(5).ToList();
            //        }
            //        else
            //        {
            //            _grades=new List<IdNameObject>();
            //        }
            //    }
            //}

            _grades = _allDataItems.Where(a => a.Name.Contains(filter)).ToList();
            return _grades;
        }

    }
}
