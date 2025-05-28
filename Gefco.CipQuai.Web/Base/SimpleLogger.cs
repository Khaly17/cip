using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Gefco.CipQuai.Web.Models;
using log4net;
using log4net.Config;

namespace Gefco.CipQuai.Web
{
    /// <summary>
    /// This class is a simple wrapper around the log tool
    /// </summary>
    public sealed class SimpleLogger
    {
        /// <summary>
        /// This enum maps to the log tool levels
        /// </summary>
        public enum ErrorLevel
        {
            /// <summary>
            /// For debugging purpose
            /// </summary>
            Debug,
            /// <summary>
            /// Informations about processing - but more than just debug
            /// </summary>
            Info,
            /// <summary>
            /// A non blocking error
            /// </summary>
            Warning,
            /// <summary>
            /// A blocking error - an exception
            /// </summary>
            Error
        }

        public SimpleLogger()
            : this("Gefco")
        {
        }

        public SimpleLogger(string logName)
        {
            if (!_isConfigured)
            {
                // Make log4net read the config file
                XmlConfigurator.Configure();
                _isConfigured = true;
            }


            _log = LogManager.GetLogger(logName);
        }

        public static void ConfigureConsoleLogger()
        {
            //MONO
            var appender = new log4net.Appender.ConsoleAppender();
            log4net.Layout.ILayout fallbackLayout = new log4net.Layout.PatternLayout("%m%n");
            appender.Layout = fallbackLayout;

            log4net.Config.BasicConfigurator.Configure(appender);
        }

        private static bool _isConfigured;
        private ILog _log;

        //private readonly ILog _log;

        public bool IsDebugEnabled
        {
            get
            {
                return true;
            }
        }
        public bool IsInfoEnabled
        {
            get
            {
                return true;
            }
        }

        public void LogMessage(string message, ErrorLevel level)
        {
            switch (level)
            {
                case ErrorLevel.Error:
                case ErrorLevel.Warning:
                case ErrorLevel.Info:
                case ErrorLevel.Debug:
                    break;
            }
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            _log.Error(message);
        }

        public void FormatError(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message, args);
            Console.ResetColor();
            _log.Error(message);
        }

        public void Error(string message, Exception e)
        {
            _log.Error(message);
            _log.Error(e);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine(e.ToString());
            Console.ResetColor();
        }

        public void Error(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.ToString());
            Console.ResetColor();
            _log.Error(e.Message, e);
        }

        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ResetColor();
            _log.Warn(message);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
            _log.Info(message);
        }

        public void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ResetColor();
            _log.Debug(message);
        }
        public static SimpleLogger GetOne()
        {
            return new SimpleLogger();
        }

        public static SimpleLogger GetOne(string logName)
        {
            return new SimpleLogger(logName);
        }
    }
}