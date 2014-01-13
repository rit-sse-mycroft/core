using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Mycroft.Cmd;

namespace Mycroft
{
    /// <summary>
    /// Log messages to log files
    /// by default logs are in the log folder, and labeled by day.
    /// Log messages are automatically time stamped
    /// </summary>
    class Logger
    {
        private string path = System.IO.Path.Combine("logs");
        private string filename;
        private DateTime date;
        private StreamWriter os;
        private FileStream fs;

        public Logger()
        {
            checkFile();
        }

        /// <summary>
        /// Checks file to confirm correct log file.
        /// </summary>
        private void checkFile()
        {
            if (!DateTime.Today.Equals(this.date))
            {
                if (os != null)
                {
                    os.Flush();
                    os.Close();
                    fs.Flush();
                    fs.Close();
                }
                this.date = DateTime.Today;
                this.filename = System.IO.Path.Combine(path, "log-", DateTime.Now.ToString("[yyyy-MM-dd]"));
                fs = new FileStream(filename, FileMode.Append);
                os = new StreamWriter(fs);
            }
        }

        /// <summary>
        /// Log given message. 
        /// Applies timestamp, and records in file.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool log(string message)
        {
            checkFile();
            try
            {
                string line = DateTime.Now.ToString("[yyyy-MM-dd-HH-mm-ss-fff]");
                line += message;
                os.WriteLine(line);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Log String message or any format
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool logMessage(string message)
        {
            log(message);
            return true;
        }

        /// <summary>
        /// Log a command object
        /// Currently just using toString
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool logCommand(Command command)
        {
            ///TODO - format appropriatly
            log(command.ToString());
            return true;
        }
    }
}
