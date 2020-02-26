using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using TestExample.Utilities.HotKey;

namespace TestExample.Utilities
{
    public class XmlHelper
    {
        public static IList<HotKeyModel> LoadHotKeys()
        {
            var path = Application.StartupPath + "\\HotKeyConfig.xml";
            var doc = XDocument.Load(path);

            var keys= doc.Elements("HotKeys").Elements("HotKey").Select(item =>
            {
                //var name = item.Element("Name");
                var name = item.Value;
                var key = item.Attribute("Key")?.Value;
                var isUsable = item.Attribute("IsUsable")?.Value;
                var isSelectCtrl = item.Attribute("IsSelectCtrl")?.Value;
                var isSelectShift = item.Attribute("IsSelectShift")?.Value;
                var isSelectAlt = item.Attribute("IsSelectAlt")?.Value;

                EKey? eKey = null;
                if (Enum.TryParse(key?.ToString(), out EKey tEKey))
                {
                    eKey = tEKey;
                }
                if (eKey != null)
                {
                    return new HotKeyModel
                    {
                        Name = name,
                        SelectKey = eKey.Value,
                        IsUsable = isUsable?.ToString() == "True",
                        IsSelectCtrl = isSelectCtrl?.ToString() == "True",
                        IsSelectShift = isSelectShift?.ToString() == "True",
                        IsSelectAlt = isSelectAlt?.ToString() == "True",
                    };
                }
                return null;
            }).Where(a=>a!=null).ToList();
            return keys;
        }
    }
}
