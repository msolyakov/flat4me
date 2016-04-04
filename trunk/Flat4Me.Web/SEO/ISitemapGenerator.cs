using System.Collections.Generic;
using System.Xml.Linq;

namespace Flat4Me.Web.SEO
{
    public interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}
