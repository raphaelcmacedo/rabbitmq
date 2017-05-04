using Main.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Helpers
{
    public static class LogFacade
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Add(Log log)
        {
            var logEvent = new LogEventInfo(LogLevel.Info, logger.Name, log.ToString());
            logEvent.Properties.Add("EventID", log.LogId);
            logger.Log(logEvent);

            Repositories.LogRepository logRepository = new Repositories.LogRepository();
            logRepository.Add(log);
        }
    }
}
