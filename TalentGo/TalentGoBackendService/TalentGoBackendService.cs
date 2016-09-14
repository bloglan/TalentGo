using System.ServiceProcess;

namespace TalentGo.BackendService
{
    public partial class TalentGoBackendService : ServiceBase
	{
		public TalentGoBackendService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
		}

		protected override void OnStop()
		{
		}
	}
}
