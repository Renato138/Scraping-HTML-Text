using HtmlAgilityPack;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace RO.XPathScrape
{
    [DisplayName("XPath Text Scrape")]
    public class XPathTextScrape : CodeActivity
    {
        [DisplayName("Html")]
        [Category("Input")]
        [RequiredArgument]
        [Description("HTML text")]
        public InArgument<string> Html
        {
            get;
            set;
        }

        [DisplayName("XPath Selector")]
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> XPath
        {
            get;
            set;
        }

        [DisplayName("Node Values")]
        [Category("Output")]
        public OutArgument<List<string>> NodeValues
        {
            get;
            set;
        }

        protected override void Execute(CodeActivityContext context)
        {
            string html = this.Html.Get(context);
            string xpath = XPath.Get(context);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var val = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (val == null)
            {
               NodeValues.Set(context, new[] { "[ERROR] No nodes found" }.ToList());
                return;
            }
            NodeValues.Set(context, val.Select(v => v.InnerText).ToList());
        }
    }

}
