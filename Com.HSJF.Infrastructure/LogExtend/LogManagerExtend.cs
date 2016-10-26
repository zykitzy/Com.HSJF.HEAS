using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.LogExtend
{
    public class LogManagerExtend
    {
        private static Logger logger;
        private bool isWriterDB = false;
        public LogManagerExtend()
        {
            if (LogManager.IsLoggingEnabled())
            {
                logger = LogManager.GetCurrentClassLogger();
            }
            else
            {
                throw new Exception("NLog Can not be work!");
            }
        }

        public LogManagerExtend(bool isWriterDB) : base()
        {
            this.isWriterDB = isWriterDB;
        }

        public void WriteToAll(string msg)
        {
            //       logger.Trace(msg);
            logger.Info(msg);
            logger.Debug(msg);
        }

        public void WriteInfo(string msg)
        {
            logger.Info(msg);
        }

        public void WriteException(string msg, Exception ex)
        {
            logger.Debug(string.Format(@"{3} /r/n  
                                            Exception Message:{0} /r/n 
                                            Exception Source:{1} /r/n 
                                            Exception Trace:{2}", ex.Message, ex.Source, ex.StackTrace, msg));
        }

        public virtual void WriterToDB(string ms)
        {

        }

        public virtual void WriterExceptionToDB(string ms)
        {

        }
    }
}
