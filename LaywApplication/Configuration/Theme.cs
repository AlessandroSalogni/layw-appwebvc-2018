using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LaywApplication.Configuration
{
    public class Theme
    {
        public enum Skins
        {
            [Description("skin-black")] SkinBlack,
            [Description("skin-black-light")] SkinBlackLight,
            [Description("skin-blue")] SkinBlue,
            [Description("skin-blue-light")] SkinBlueLight,
            [Description("skin-green")] SkinGreen,
            [Description("skin-green-light")] SkinGreenLight,
            [Description("skin-purple")] SkinPurple,
            [Description("skin-purple-light")] SkinPurpleLight,
            [Description("skin-red")] SkinRed,
            [Description("skin-red-light")] SkinRedLight,
            [Description("skin-yellow")] SkinYellow,
            [Description("skin-yellow-light")] SkinYellowLight
        };

        public enum Layouts
        {
            [Description("layout-boxed")] LayoutBoxed,
            [Description("sidebar-collapsed")] SidebarCollapsed,
            [Description("sidebar-mini")] SidebarMini,
            [Description("fixed")] Fixed,
            [Description("layout-top-nav ")] LayoutTopNav
        };

        public Skins Skin
        {
            get; set;
        }

        public Layouts Layout
        {
            get; set;
        }

        public string GetStringLayout()
        {
            return GetEnumDescription(Layout);
        }

        public string GetStringSkin()
        {
            return GetEnumDescription(Skin);
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}
