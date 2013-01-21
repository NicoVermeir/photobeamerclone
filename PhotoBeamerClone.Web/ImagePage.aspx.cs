using System;
using System.Collections.Specialized;
using System.Web;

namespace PhotoBeamerClone.Web
{
    public partial class ImagePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection postedValues = Request.Form;

            var base64String = postedValues["hiddenByteArray"];
            HttpContext.Current.Session["ByteArray"] = base64String;
        }
    }
}