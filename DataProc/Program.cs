using System;

namespace DataProc
{
    class Program
	{
		static void Main(string[] args)
		{
			IService svc = null;
			switch (args[0])
			{
				case "SMS":
					svc = new SmsService();
					break;
				case "Prompt":
					svc = new PromptSMSService();
					break;
				case "Email":
					svc = new EmailService();
					break;
				case "Export":
					svc = new DataExport();
					break;
				case "Import":
					svc = new DataImport();
					break;
			}

			if (svc != null)
				svc.Run();
			else
			{
				Console.WriteLine("找不到相关的服务。");
			}

			Console.WriteLine("按任意键退出...");
			Console.ReadLine();
		}
	}
}
