﻿using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#pragma warning disable S125 // Sections of code should not be commented out
namespace IntuneAppBuilder.Domain
{
    /// <summary>
    /// A statically typed manifest for an MSI app. Gets converted to a byte array and included on MobileAppContentFile.
    /// </summary>
    [XmlRoot("MobileMsiData")]
    public class MobileMsiManifest
    {
        [XmlAttribute]
        public string MsiExecutionContext { get; set; }
        [XmlAttribute]
        public bool MsiRequiresReboot { get; set; }
        [XmlAttribute]
        public bool MsiRequiresLogon { get; set; }
        [XmlAttribute]
        public string MsiPublisher { get; set; }
        [XmlAttribute]
        public string MsiProductCode { get; set; }
        [XmlAttribute]
        public string MsiPackageCode { get; set; }
        [XmlAttribute]
        public string MsiUpgradeCode { get; set; }
        [XmlAttribute]
        public string MsiProductVersion { get; set; }
        [XmlAttribute]
        public bool MsiIsMachineInstall { get; set; }
        [XmlAttribute]
        public bool MsiIsUserInstall { get; set; }
        [XmlAttribute]
        public bool MsiIncludesServices { get; set; }
        [XmlAttribute]
#pragma warning disable S100 // Methods and properties should be named in PascalCase
        public bool MsiIncludesODBCDataSource { get; set; }
#pragma warning restore S100 // Methods and properties should be named in PascalCase
        [XmlAttribute]
        public bool MsiContainsSystemRegistryKeys { get; set; }
        [XmlAttribute]
        public bool MsiContainsSystemFolders { get; set; }

        public byte[] ToByteArray()
        {
            var serializer = new XmlSerializer(typeof(MobileMsiManifest));

            using var ms = new MemoryStream();
            using var writer = XmlWriter.Create(ms, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Encoding = Encoding.ASCII // important - Graph can't serialize if UTF
            });
            serializer.Serialize(writer, this, new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName(string.Empty, string.Empty)
            }));
            return ms.ToArray();
        }

        public static MobileMsiManifest FromByteArray(byte[] data)
        {
            var serializer = new XmlSerializer(typeof(MobileMsiManifest));

            if (data == null) return default;
            using var ms = new MemoryStream(data);
            return (MobileMsiManifest)serializer.Deserialize(ms);
        }
    }
}