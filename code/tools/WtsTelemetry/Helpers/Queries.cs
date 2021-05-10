namespace WtsTelemetry.Helpers
{
    public class Queries
    {
        private readonly string DataByEventQuery = @"
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
| where customDimensions.WtsCategory == '{4}'
| where customDimensions['WtsGenerationProperties.appmodel'] in ('{5}');
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
        private readonly string appModel;

        public Queries(string platform, int year, int month, string appModel = null)
        {
            this.platform = platform;
            this.year = year;
            this.month = month;
            this.appModel = appModel ?? string.Empty;
        }

        public string Projects => GetDataByPlatformQuery("WtsProjectGen", "WtsProjectType");

        // TODO: Remove WtsFramework and use WtsFrontendFramework
        public string Frameworks => GetDataByPlatformQuery("WtsProjectGen", "WtsFramework");

        public string FrontendFrameworks => GetDataByPlatformQuery("WtsProjectGen", "WtsFrontendFramework");

        public string BackendFrameworks => GetDataByPlatformQuery("WtsProjectGen", "WtsBackendFramework");

        public string Pages => GetDataByPlatformQuery("WtsPageGen", "WtsTemplateName");

        public string Features => GetDataByPlatformQuery("WtsFeatureGen", "WtsTemplateName");

        public string Services => GetDataByPlatformQuery("WtsServiceGen", "WtsTemplateName");

        public string Testing => GetDataByPlatformQuery("WtsTestingGen", "WtsTemplateName");

        public string EntryPoints => GetDataByEventQuery("WtsWizard", "WtsWizardType");

        public string Languages => GetDataByEventQuery("WtsProjectGen", "WtsLanguage");

        public string Platforms => GetDataByEventQuery("WtsProjectGen", "WtsCategory");

        public string AppModels(string[] appModels) => GetDataByPlatformQuery("WtsProjectGen", "WtsGenerationProperties.appmodel", appModels);

        private string GetDataByPlatformQuery(string eventName, string itemName)
        {
            return string.Format(DataByCategoryQuery, year, month, eventName, itemName, platform, appModel);
        }

        private string GetDataByPlatformQuery(string eventName, string itemName, string[] appModels)
        {
            string appModel = string.Join("', '", appModels);
            return string.Format(DataByCategoryQuery, year, month, eventName, itemName, platform, appModel);
        }

        private string GetDataByEventQuery(string eventName, string itemName)
        {
            return string.Format(DataByEventQuery, year, month, eventName, itemName);
        }
    }
}
