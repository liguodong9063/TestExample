using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml;
using DevExpress.Xpf.Grid;
using TestExample.Model;
using TestExample.UserControls;

namespace TestExample.Utilities
{
    public class GridControlColumnInformationHelper
    {
        private static readonly string CurrentApplicationDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\GridSettings.xml";

        public static bool ExistCustomerSetting(CustomGridControl grid)
        {
            //IContextService context = new ContextService();
            //var userAccount = context.GetUser().Account;
            //todo:临时处理
            var userAccount = "admin";
            var doc = new XmlDocument();
            if (!File.Exists(CurrentApplicationDirectory)) return false;
            doc.Load(CurrentApplicationDirectory);
            var existUser = doc.DocumentElement.SelectSingleNode(string.Format("//User[@Account='{0}']", userAccount));
            if (existUser == null) return false;
            var existGrid = existUser.SelectSingleNode(string.Format("./Grid[@UniqueName='{0}']", grid.UniqueName));
            if (existGrid == null) return false;
            return true;
        }

        /// <summary>
        /// 加载用户自定义设置（如未加载则返回false）
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static bool LoadGridSettingsByXml(CustomGridControl grid)
        {
            if (!File.Exists(CurrentApplicationDirectory)) return false;
            //IContextService context = new ContextService();
            //var userAccount = context.GetUser().Account;
            //todo:临时处理
            var userAccount = "admin";
            var doc = new XmlDocument();
            doc.Load(CurrentApplicationDirectory);
            var exist = ExistAccount(userAccount, grid, doc) ?? ExistAccount("mainDefalut", grid, doc);
            if (exist == null) return false;
            var columns = grid.Columns;
            //加上BeginInit,EndInit为了修复自定义列顺序无效问题
            grid.BeginInit();
            foreach (var column in columns)
            {
                var eleCol = (XmlElement)exist.SelectSingleNode(string.Format("./Column[@Name='{0}']", column.FieldName));
                if (eleCol != null)
                {
                    column.Width = double.Parse(eleCol.GetAttribute("Width"));
                    column.VisibleIndex = string.IsNullOrEmpty(eleCol.GetAttribute("VisibleIndex")) ? column.VisibleIndex : int.Parse(eleCol.GetAttribute("VisibleIndex"));
                }
            }
            grid.EndInit();
            return true;
        }

        private static XmlNode ExistAccount(string account, CustomGridControl grid, XmlDocument doc)
        {
            if (!File.Exists(CurrentApplicationDirectory)) return null;
            doc.Load(CurrentApplicationDirectory);
            var existUser = doc.DocumentElement.SelectSingleNode(string.Format("//User[@Account='{0}']", account));
            if (existUser == null) return null;
            var existGrid = existUser.SelectSingleNode(string.Format("./Grid[@UniqueName='{0}']", grid.UniqueName));
            if (existGrid == null) return null;
            return existGrid;
        }

        public static ObservableCollection<CustomColumnDisplayDetail> GetCustomColumnDisplayDetails(GridControl gridControl)
        {
            var columnDetailConfigs = new ObservableCollection<CustomColumnDisplayDetail>();
            if (gridControl != null)
            {
                foreach (CustomGridColumn column in gridControl.Columns)
                {
                    column.Index += 1;
                    if (column.IsColumnEnabled == false)
                    {
                        continue;
                    }
                    if (column.CustomIsVisible == false)
                    {
                        continue;
                    }
                    if (column.Header == null || column.FieldName == null) continue; //去掉自定义字段没有起用的
                    string header = string.Empty;
                    bool isSelected = column.Visible;
                    if (column.Header.GetType() == typeof(DockPanel))
                    {
                        var dockPanel = (DockPanel)column.Header;
                        UIElementCollection childs = dockPanel.Children;
                        foreach (object child in childs)
                        {
                            if (child.GetType() == typeof(TextBlock))
                            {
                                header += ((TextBlock)child).Text;
                            }
                        }
                    }
                    else if (column.Header.GetType() == typeof(StackPanel))
                    {
                        var stackPanel = (StackPanel)column.Header;
                        UIElementCollection childs = stackPanel.Children;
                        foreach (object child in childs)
                        {
                            if (child.GetType() == typeof(TextBlock))
                            {
                                header += ((TextBlock)child).Text;
                            }
                        }
                    }
                    else
                    {
                        header = column.Header.ToString();
                    }
                    var columnDetailConfig = new CustomColumnDisplayDetail { ColumnChinaName = header, ColumnEnglishName = column.FieldName, IsSelected = isSelected, DisplayIndex = column.Index };
                    columnDetailConfigs.Add(columnDetailConfig);
                }
            }
            return new ObservableCollection<CustomColumnDisplayDetail>(columnDetailConfigs.OrderBy(o => o.DisplayIndex));
        }
    }
}
