using System;
using System.Collections.Generic;
using System.Threading;
using Facebook.Models;

namespace Facebook.Services.Api
{
	public abstract class GatekeeperProvider
	{
		private readonly FacebookContext _context;

		private readonly string _gatekeeperName;

		private readonly Lazy<bool> _enabled;

		public virtual bool Enabled
		{
			get
			{
				return _enabled.Value;
			}
		}

		protected GatekeeperProvider(FacebookContext context, string gatekeeperName)
		{
			_context = context;
			_gatekeeperName = gatekeeperName;
			_enabled = new Lazy<bool>(Initialize, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		private bool Initialize()
		{
			IReadOnlyDictionary<string, bool> gatekeepers = _context.GetGatekeepers();
			return gatekeepers != null && gatekeepers.ContainsKey(_gatekeeperName) && gatekeepers[_gatekeeperName];
		}
	}
}
