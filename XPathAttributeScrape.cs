using HtmlAgilityPack;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RO.XPathScrape
{
    [DisplayName("XPath Attribute Scrape")]
    public class XPathAttributeScrape : CodeActivity
    {
        [DisplayName("Html")]
        [Category("Input")]
        [RequiredArgument]
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

        [DisplayName("Attribute")]
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> AttributeToQuery
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
            string attribute = AttributeToQuery.Get(context);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var val = htmlDoc.DocumentNode.SelectNodes(xpath);
            if (val == null)
            {
                string[] source = new string[1]
                {
                "[ERROR] No nodes found"
                };
                NodeValues.Set(context, source.ToList());
                return;
            }

            var list = new List<string>(val.Count);
            for (int i = 0; i < val.Count; i++)
            {
                if (attribute.ToUpper().Equals("INNERHTML"))
                {
                    if (val[i].InnerHtml == null)
                    {
                        list.Add("[ERROR] InnerHtml");
                    }
                    else
                    {
                        list.Add(val[i].InnerHtml.ToString());
                    }
                }
                else if (val[i].Attributes[attribute] == null)
                {
                    list.Add("[ERROR] No such attribute");
                }
                else
                {
                    list.Add(val[i].Attributes[attribute].Value);
                }
            }
            NodeValues.Set(context, list);
        }
    }

}
