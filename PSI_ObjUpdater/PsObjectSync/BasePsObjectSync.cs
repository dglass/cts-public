using System;
using System.Threading;
using System.ServiceModel;

namespace PsObjectSync
{
    public class BasePsObjectSync
    {
		public string PwaUri { get; set; }
		public string OrgConnStr { get; set; }
		public string OrgNavConnStr { get; set; }
		protected static int lcid = Thread.CurrentThread.CurrentCulture.LCID;
		const int MAXSIZE = 500000000;
		public EndpointAddress a;
		public BasicHttpBinding b;

		protected BasePsObjectSync(string pwaUri)
		{
			PwaUri = pwaUri;
			a = new EndpointAddress(PwaUri);
			b = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
			{
				Name = "basicHttpConf",
				SendTimeout = TimeSpan.MaxValue,
				MaxReceivedMessageSize = MAXSIZE,
				MessageEncoding = WSMessageEncoding.Text,
			};
			b.ReaderQuotas.MaxNameTableCharCount = MAXSIZE;
			b.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
		}

		public void SetCredentials(System.ServiceModel.Description.ClientCredentials creds)
		{
			if (creds != null)
			{
				creds.Windows.AllowedImpersonationLevel
					= System.Security.Principal.TokenImpersonationLevel.Impersonation;
			}
			else
			{
				throw new NullReferenceException("missing ChannelFactory Credentials");
			}
		}
    }
}
