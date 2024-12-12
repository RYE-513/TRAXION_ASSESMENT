using System.IO;
using System.ServiceProcess;
using System;
using NLog;

namespace FileMonitorService
{
    public partial class Service1 : ServiceBase
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Service1()
        {
            InitializeComponent();
        }

        private FileSystemWatcher _fileSystemWatcher;

        protected override void OnStart(string[] args)
        {
            _logger.Info("Service started");

            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.Path = @"C:\Folder1";
            _fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
            _fileSystemWatcher.Created += new FileSystemEventHandler(OnFileCreated);
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            _logger.Info($"File {e.Name} created");

            // Move the file to C:\Folder2
            try
            {
                File.Move(e.FullPath, @"C:\Folder2\" + e.Name);
                _logger.Info($"File {e.Name} moved to C:\\Folder2");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error moving file {e.Name}: {ex.Message}");
            }
        }

        private void LogEvent(string fileName, DateTime timestamp)
        {
            _logger.Info($"File {fileName} created at {timestamp}");
        }

        protected override void OnStop()
        {
            _logger.Info("Service stopped");

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Dispose();
        }
    }
}