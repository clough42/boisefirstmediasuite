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

        private string presentationFile = null;
        private string copyFile = null;
        private string presentationDir = null;
        string oldHash = null;
        string processName = POWERPOINT_VIEWER_PROCESS_NAME;
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
            while (true)
            {
                EvaluateFile(this.presentationFile);

                // wait and then check again
                Thread.Sleep(30 * 1000);
            }
        }

        void EvaluateFile(string filename)
        {
            try
            {
                lock (this.sync)
                {
                    if (filename != null && filename != this.presentationFile)
                    {
                        // not the file we care about
                        return;
                    }

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
            catch (Exception e)
            {
                System.Console.Out.WriteLine("EXCEPTION: " + e.Message);
            }
        }

        private void StartPowerPointViewer(string p)
        {
            this.copyFile = makeTempCopy(this.presentationFile);
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = this.copyFile;
            proc.Start();
            this.processName = proc.ProcessName;
            proc.Dispose();
        }

        private static string makeTempCopy(string presentationFile)
        {
            String copyFile = makeTempFileName(presentationFile);
            System.Console.Out.WriteLine("Making copy: " + copyFile);
            File.Copy(presentationFile, copyFile);
            return copyFile;
        }

        private static string makeTempFileName(String presentationFile)
        {
            String extension = Path.GetExtension(presentationFile);
            String path = Path.GetDirectoryName(presentationFile);
            return Path.Combine(path, Guid.NewGuid().ToString() + extension);
        }

        private void KillPowerPointViewer()
        {
            Process[] procs = Process.GetProcessesByName(processName);
            foreach (Process proc in procs)
            {
                proc.Kill();
                proc.WaitForExit();
            }
            if (this.copyFile != null && File.Exists(this.copyFile))
            {
                File.Delete(this.copyFile);
            }
            this.copyFile = null;
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
