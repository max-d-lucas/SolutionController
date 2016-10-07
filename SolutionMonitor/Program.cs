namespace SolutionMonitor
{
    using System;
    using System.Windows.Forms;
    using Autofac;
    using Common.Logging;
    using IoC;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += Program.CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             var container = IoCConfig.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                Application.Run(container.Resolve<SolutionManager>());
            }
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject != null)
            {
                Program.Log(e.ExceptionObject as Exception);
            }
        }

        private static readonly ILog _Logger = LogManager.GetLogger("SolutionMonitor");
        private static readonly ILogManager _LogManager = new LogManager();

        public static void Log(Exception ex)
        {
            var logger = ex.TargetSite == null || ex.TargetSite.DeclaringType == null
                ? _Logger
                : _LogManager.GetLogger(ex.TargetSite.DeclaringType);
            var hasInnerException = ex.InnerException != null;
            logger.ErrorFormat("EXCEPTION", ex);
            if (hasInnerException)
            {
                logger.ErrorFormat("BASE EXCEPTION", ex.GetBaseException());
            }
        }
    }
}
