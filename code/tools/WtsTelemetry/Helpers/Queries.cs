namespace WtsTelemetry.Helpers
{
    public class Queries
    {
        private readonly string DataQuery = @"
let startDatetime = startofmonth(datetime({0}-{1}-01));
let endDatetime = endofmonth(datetime({0}-{1}-01));
let queryTable = customEvents
| where timestamp between(startDatetime .. endDatetime)
| extend eventName = iif(itemType == 'customEvent',name,'')
| where eventName == '{2}';
queryTable
| extend itemName = tostring(customDimensions['{3}'])
| summarize items = sum(itemCount) by itemName
| extend total = toscalar(queryTable | count)
| extend percentage = todouble(items)/toscalar(queryTable | count)*100
| order by percentage desc
";
        private readonly string DataByCategoryQuery = @"
let startDatetime = startofmonth(datetime({0}-{1}-01));
let endDatetime = endofmonth(datetime({0}-{1}-01));
let queryTable = customEvents
| where timestamp between(startDatetime .. endDatetime)
| extend eventName = iif(itemType == 'customEvent',name,'')
| where eventName == '{2}'
| where customDimensions.WtsCategory == '{4}';
queryTable
| extend itemName = tostring(customDimensions['{3}'])
| summarize items = sum(itemCount) by itemName
| extend total = toscalar(queryTable | count)
| extend percentage = todouble(items)/toscalar(queryTable | count)*100
| order by percentage desc
";

        private readonly int year;
        private readonly int month;
        private readonly string platform;

        public Queries(string platform, int year, int month)
        {
            this.platform = platform;
            this.year = year;
            this.month = month;
        }

        public string Projects => string.Format(DataByCategoryQuery, year, month, "WtsProjectGen", "WtsProjectType", platform);

        // TODO: Remove WtsFramework and use WtsFrontendFramework
        public string Frameworks => string.Format(DataByCategoryQuery, year, month, "WtsProjectGen", "WtsFramework", platform);

        public string FrontendFrameworks => string.Format(DataByCategoryQuery, year, month, "WtsProjectGen", "WtsFrontendFramework", platform);

        public string BackendFrameworks => string.Format(DataByCategoryQuery, year, month, "WtsProjectGen", "WtsBackendFramework", platform);

        public string Pages => string.Format(DataByCategoryQuery, year, month, "WtsPageGen", "WtsTemplateName", platform);

        public string Features => string.Format(DataByCategoryQuery, year, month, "WtsFeatureGen", "WtsTemplateName", platform);

        public string Services => string.Format(DataByCategoryQuery, year, month, "WtsServiceGen", "WtsTemplateName", platform);

        public string Testing => string.Format(DataByCategoryQuery, year, month, "WtsTestingGen", "WtsTemplateName", platform);

        public string EntryPoints => string.Format(DataQuery, year, month, "WtsWizard", "WtsWizardType");

        public string Languages => string.Format(DataQuery, year, month, "WtsProjectGen", "WtsLanguage");

        public string Platforms => string.Format(DataQuery, year, month, "WtsProjectGen", "WtsCategory");

    }
}
