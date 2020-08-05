﻿using System.Runtime.InteropServices;
using System.Xml;
using WinSW.Util;

namespace WinSW.Extensions
{
    /// <summary>
    /// Describes WinSW extensions in <see cref="IWinSWExtension"/>
    /// </summary>
    /// <remarks>
    /// Any extension has its own descriptor instance.
    /// </remarks>
    public class WinSWExtensionDescriptor
    {
        /// <summary>
        /// Unique extension ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Exception is enabled
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Extension classname
        /// </summary>
        public string ClassName { get; private set; }

        public WinSWExtensionDescriptor(string id, string className, bool enabled)
        {
            this.Id = id;
            this.Enabled = enabled;
            this.ClassName = className;
        }

        // PLUGIN : Get Type contains enabled, classname and id, instead XmlElement
        public static WinSWExtensionDescriptor FromXml(XmlElement node)
        {
            bool enabled = XmlHelper.SingleAttribute(node, "enabled", true);
            string className = XmlHelper.SingleAttribute<string>(node, "className");
            string id = XmlHelper.SingleAttribute<string>(node, "id");
            return new WinSWExtensionDescriptor(id, className, enabled);
        }
    }
}
