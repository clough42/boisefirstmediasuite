using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Diagnostics;

namespace AutoPowerPoint
{
    class AutoViewer
    {
        private const string POWERPOINT_VIEWER_PROCESS_NAME = "PPTVIEW";

        private FileSystemWatcher watcher = null;
        private string presentationFile = null;
        private string presentationDir = null;
        string oldHash = null;
        object sync = new object();

        public AutoViewer(string[] args)
        {
            if (args.Length != 1)
            {
                UsageExit("Presentation file name not specified");
            }

            this.presentationFile = Path.GetFullPath(args[0]);
            if (!File.Exists(this.presentationFile))
            {
                UsageExit(string.Format("file not found: {0}", this.presentationFile));
            }

            this.presentationDir = Path.GetDirectoryName(presentationFile);
            // extreme sanity check
            if (!Directory.Exists(this.presentationDir))
            {
                UsageExit(string.Format("directory does not exist: {0}", this.presentationDir));
            }
        }

        private void UsageExit(string message)
        {
            throw new Exception(string.Format("{0}\r\nUsage: AutoPowerPoint <presentation file name>", message));
        }

        public void Run()
        {
            this.watcher = new FileSystemWatcher(this.presentationDir);
            this.watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            this.watcher.Renamed += new RenamedEventHandler(watcher_Renamed);
            this.watcher.Created += new FileSystemEventHandler(watcher_Created);
            this.watcher.EnableRaisingEvents = true;

            EvaluateFile(null);

            while (true)
            {
                Thread.Sleep(60 * 60 * 1000);
            }
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            EvaluateFile(e.FullPath);
        }

        void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            EvaluateFile(e.FullPath);
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            EvaluateFile(e.FullPath);
        }

        void EvaluateFile(string filename)
        {
            lock (this.sync)
            {
                if (filename != null && filename != this.presentationFile)
                {
                    // not the file we care about
                    return;
                }

                // wait ten seconds to make sure things have a chance to quiesce
                Thread.Sleep(10000);

                string newHash = CalculateHash(this.presentationFile);
                if (oldHash != null && newHash == oldHash)
                {
                    // nothing has changed
                    return;
                }
                this.oldHash = newHash;

                // time to change the presentation
                // first, kill any old presentation that's running
                KillPowerPointViewer();

                // and start up the new presentation
                StartPowerPointViewer(this.presentationFile);
            }


        }

        private void StartPowerPointViewer(string p)
        {
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = this.presentationFile;
            proc.Start();
            //proc.Dispose();
        }

        private void KillPowerPointViewer()
        {
            Process[] procs = Process.GetProcessesByName(POWERPOINT_VIEWER_PROCESS_NAME);
            foreach (Process proc in procs)
            {
                proc.Kill();
            }
        }

        string CalculateHash(string filename)
        {
            using (FileStream f = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 8192))
            {
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                sha1.ComputeHash(f);

                byte[] hash = sha1.Hash;
                StringBuilder sb = new StringBuilder();
                foreach (byte hashByte in hash)
                {
                    sb.Append(String.Format("{0:X1}", hashByte));
                }
                return sb.ToString();
            }
        }

    }


}
