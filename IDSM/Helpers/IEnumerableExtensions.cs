using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.WebPages;
using System.Dynamic;
using System.IO;

namespace IDSM.Helpers
{

    ///<summary>
    /// HTMLHelpers
    /// Presently extension methods for any type object collection
    ///</summary>
    ///<remarks>
    ///Taken directly from http://stackoverflow.com/questions/7820117/elegant-foreach-else-construct-in-razor
    ///     Reason:  wanted a foreach/else (for no records) on the viewplayers index view (chosenplayers list).  
    ///     NOTE:  I DO NOT UNDERSTAND how the Func delegate works for the EACH method & why it MUST have (on the index page input) html tags around the HTML.Partial
    ///            (without these tags, it won't work).  I can't see how the delegate required those tags - it seems to just want a type object.
    ///            However.. they are referred to in the above link as templated delegates - http://haacked.com/archive/2011/02/27/templated-razor-delegates.aspx
    ///            Another link: http://www.prideparrot.com/blog/archive/2012/9/simplifying_html_generation_using_razor_templates
    ///</remarks>
    public static class HtmlHelpers
    {
        public static ElseHelperResult<TItem> Each<TItem>(this IEnumerable<TItem> items,
            //  Func<TItem, HelperResult> eachTemplate)
              Func<TItem, HelperResult> eachTemplate)
        {
            return ElseHelperResult<TItem>.Create(items, eachTemplate);
        }
    }

    public class ElseHelperResult<T> : HelperResult
    {
        private class Data
        {
            public IEnumerable<T> Items { get; set; }
            public Func<T, HelperResult> EachTemplate { get; set; }
            public Func<dynamic, HelperResult> ElseTemplate { get; set; }

            public Data(IEnumerable<T> items, Func<T, HelperResult> eachTemplate)
            {
                Items = items;
                EachTemplate = eachTemplate;
            }

            public void Render(TextWriter writer)
            {
                foreach (var item in Items)
                {
                    var result = EachTemplate(item);
                    result.WriteTo(writer);
                }

                if (!Items.Any() && ElseTemplate != null)
                {
                    var otherResult = ElseTemplate(new ExpandoObject());
                    // var otherResult = other(default(TItem));
                    otherResult.WriteTo(writer);
                }
            }
        }

        public ElseHelperResult<T> Else(Func<dynamic, HelperResult> elseTemplate)
        {
            RenderingData.ElseTemplate = elseTemplate;
            return this;
        }

        public static ElseHelperResult<T> Create(IEnumerable<T> items, Func<T, HelperResult> eachTemplate)
        //public static ElseHelperResult<T> Create(dynamic items, Func<T, HelperResult> eachTemplate)
        {
            var data = new Data(items, eachTemplate);
            return new ElseHelperResult<T>(data);
        }

        private ElseHelperResult(Data data)
            : base(data.Render)
        {
            RenderingData = data;
        }

        private Data RenderingData { get; set; }
    }
}