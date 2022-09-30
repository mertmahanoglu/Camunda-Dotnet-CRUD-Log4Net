using log4net;

namespace Camunda.WHttpClient.Logging
{
    public class CustomLogging
    {
        private ILog _logger = null;
        private string _logFile = null;


        public void Initialize(string ApplicationPath)
        {
            _logFile = Path.Combine(ApplicationPath, "App_Data", "Demo.Camunda.HttpClient.Log");
            GlobalContext.Properties["LogFileName"] = _logFile;

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(ApplicationPath, "log4net.config")));
            _logger = LogManager.GetLogger("Demo.Camunda");
        }


        public string LogFile
        {
            get { return _logFile; }
        }

        public enum TracingLevel
        {
            ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF
        }

        public void LogMessage(TracingLevel tracingLevel, string Message)
        {
            switch (tracingLevel)
            {
                case TracingLevel.DEBUG:
                    _logger.Debug(Message);
                    break;

                case TracingLevel.INFO:
                    _logger.Info(Message);
                    break;

                case TracingLevel.WARN:
                    _logger.Warn(Message);
                    break;

                case TracingLevel.ERROR:
                    _logger.Error(Message);
                    break;

                case TracingLevel.FATAL:
                    _logger.Fatal(Message);
                    break;


            }
        }
    }
}
