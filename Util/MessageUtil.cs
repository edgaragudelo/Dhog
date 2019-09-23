using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DHOG_WPF.Util
{
    public class MessageUtil
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MessageUtil));
        private static ResourceManager resourceManager = new ResourceManager("DHOG_WPF.Resources.Messages", Assembly.GetExecutingAssembly());

        private static string GetMessageString(string messageKey)
        {
            string message = resourceManager.GetString(messageKey);
            if (message != null)
                return message;
            else
            {
                log.Error(FormatMessage("ERROR.MessageKeyNotFound", messageKey));
                throw new Exception(FormatMessage("ERROR.ApplicationConfigurationError"));
            }
        }

        public static string FormatMessage(string messageKey)
        {
            try
            {
                return String.Format(GetMessageString(messageKey));
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw new Exception(FormatMessage("ERROR.ApplicationConfigurationError"));
            }
        }

        public static String FormatMessage(String messageKey, Object arg0)
        {
            try
            {
                return String.Format(GetMessageString(messageKey), arg0);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw new Exception(FormatMessage("ERROR.ApplicationConfigurationError"));
            }
        }

        public static String FormatMessage(String messageKey, Object arg0, Object arg1)
        {
            Object[] args = new Object[2];
            args[0] = arg0;
            args[1] = arg1;
            try
            {
                return String.Format(GetMessageString(messageKey), args);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw new Exception(FormatMessage("ERROR.ApplicationConfigurationError"));
            }
        }

        public static String FormatMessage(String messageKey, Object arg0, Object arg1, Object arg2)
        {
            Object[] args = new Object[3];
            args[0] = arg0;
            args[1] = arg1;
            args[2] = arg2;
            try
            {
                return String.Format(GetMessageString(messageKey), args);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw new Exception(FormatMessage("ERROR.ApplicationConfigurationError"));
            }
        }

        public static String FormatMessage(String messageKey, Object arg0, Object arg1, Object arg2, Object arg3)
        {
            Object[] args = new Object[4];
            args[0] = arg0;
            args[1] = arg1;
            args[2] = arg2;
            args[3] = arg3;
            try
            {
                return String.Format(GetMessageString(messageKey), args);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw new Exception(FormatMessage("ERROR.ApplicationConfigurationError"));
            }
        }

        public static String FormatMessage(String messageKey, Object arg0, Object arg1, Object arg2, Object arg3, Object arg4)
        {
            Object[] args = new Object[5];
            args[0] = arg0;
            args[1] = arg1;
            args[2] = arg2;
            args[3] = arg3;
            args[4] = arg4;
            try
            {
                return String.Format(GetMessageString(messageKey), args);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw new Exception(FormatMessage("ERROR.ApplicationConfigurationError"));
            }
        }

        public static String FormatMessage(String messageKey, Object arg0, Object arg1, Object arg2, Object arg3, Object arg4, Object arg5)
        {
            Object[] args = new Object[6];
            args[0] = arg0;
            args[1] = arg1;
            args[2] = arg2;
            args[3] = arg3;
            args[4] = arg4;
            args[5] = arg5;
            try
            {
                return String.Format(GetMessageString(messageKey), args);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                throw new Exception(FormatMessage("ERROR.ApplicationConfigurationError"));
            }
        }
    }
}
