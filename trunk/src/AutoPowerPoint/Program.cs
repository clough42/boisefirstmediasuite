using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;


namespace AutoPowerPoint
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AutoViewer viewer = new AutoViewer(args);
                viewer.Run();
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine("EXCEPTION: " + e.ToString());
            }
        }
    }




    //class Program
    //{
    //    private string FILE_NAME = Path.GetFullPath(@"C:\Shows\Test.ppt");
    //    PowerPoint.Application application = null;
    //    PowerPoint.Presentation presentation = null;

    //    static void Main(string[] args)
    //    {
    //        new Program().Run();
    //    }

    //    void Run()
    //    {
    //        application = new PowerPoint.Application();
    //        application.Visible = MsoTriState.msoTrue;

    //        FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(FILE_NAME));
    //        watcher.Changed += new FileSystemEventHandler(watcher_Changed);
    //        watcher.Renamed += new RenamedEventHandler(watcher_Changed);
    //        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite;
    //        watcher.EnableRaisingEvents = true;

    //        EvaluateNewFile();

    //        while(true)
    //        {
    //            Thread.Sleep(1000000);
    //        }
    //    }

    //    void watcher_Changed(object sender, FileSystemEventArgs e)
    //    {
    //        if (e.FullPath == FILE_NAME)
    //        {
    //            EvaluateNewFile();
    //        }
    //    }

    //    private void EvaluateNewFile()
    //    {
    //        Thread.Sleep(2000);
    //        lock (FILE_NAME)
    //        {
    //            Console.Out.WriteLine("Evaluating new file");
    //            try
    //            {
    //                if (presentation != null)
    //                {
    //                    Console.Out.WriteLine("Closing existing presentation");
    //                    presentation.Close();
    //                    presentation = null;
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                System.Console.Out.WriteLine(e.ToString());
    //            }

    //            GC.Collect();

    //            try
    //            {
    //                Console.Out.WriteLine("Loading new presentation");
    //                presentation = application.Presentations.Open(FILE_NAME, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoTrue);
    //                presentation.SlideShowSettings.Run();
    //            }
    //            catch (Exception e)
    //            {
    //                System.Console.Out.WriteLine(e.ToString());
    //                if (presentation != null)
    //                {
    //                    presentation.Close();
    //                }
    //                presentation = null;
    //            }
    //        }
    //    }
    //}
}
