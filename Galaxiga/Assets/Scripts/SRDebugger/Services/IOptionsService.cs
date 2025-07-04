using System;
using System.Collections.Generic;
using System.ComponentModel;
using SRDebugger.Internal;

namespace SRDebugger.Services
{
	public interface IOptionsService
	{
		event EventHandler OptionsUpdated;

		event EventHandler<PropertyChangedEventArgs> OptionsValueUpdated;

		ICollection<OptionDefinition> Options { get; }

		[Obsolete("Use IOptionsService.AddContainer instead.")]
		void Scan(object obj);

		void AddContainer(object obj);

		void RemoveContainer(object obj);
	}
}
