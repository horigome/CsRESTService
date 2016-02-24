/**
 *  2016-02-24 M.Horigome
 *  WCF REST WebService Sample
 */
using System;
using System.IO;

using System.ServiceModel;
using System.ServiceModel.Web;

using System.Runtime.Serialization;
using System.Xml;

namespace CsRESTConsole
{
    /// <summary>
    ///  Version response class
    /// </summary>
    [DataContract(Namespace = "", Name = "ImageVersion")]
    public class ImageVersion
    {
        [DataMember]
        public string Version;
        [DataMember]
        public string Description;
    } 

    /// <summary>
    /// IImagingService Interface 
    /// </summary>
    [ServiceContract]
    public interface IImagingService
    {
        [OperationContract]
        [WebInvoke( UriTemplate = "version", 
                    Method="GET", 
                    ResponseFormat = WebMessageFormat.Xml)]
        ImageVersion GetVersion();

        [OperationContract]
        [WebInvoke( UriTemplate = "image?location={locationPath}", 
                    Method="GET",
                    BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream GetXpsImage(string locationPath);
    }

    /// <summary>
    /// ImagingService API Class 
    /// </summary>
    public class ImagingService : IImagingService
    {
        // [GET] /version
        public ImageVersion GetVersion()
        {
            Console.WriteLine("[GET] ==> /api/version" );
            return new ImageVersion { Version="1.1.1.1",Description = "Hello World!" };
        }

        // [GET] /image?location={}
        public Stream GetXpsImage(string locationPath)
        {
            Console.WriteLine("[GET] ==> /api/image?location=" + locationPath);

            string image_file = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), 
                "sample.png");

            byte[] file = File.ReadAllBytes(image_file);
            var m = new MemoryStream(file);
            m.Position = 0;

            WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";
            WebOperationContext.Current.OutgoingResponse.ContentLength =  m.Length;
            return m;
        }
    }
}
