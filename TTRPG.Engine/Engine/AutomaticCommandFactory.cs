using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.CommandParsing;

namespace TTRPG.Engine.Engine
{
	public class AutomaticCommandFactory : IAutomaticCommandFactory
	{
		private readonly AutomaticCommandFactoryOptions _options;
		private readonly GameObject _data;

		public AutomaticCommandFactory(AutomaticCommandFactoryOptions options, GameObject data)
		{
			_options = options;
			_data = data;
		}

		public IEnumerable<EngineCommand> GetAutomaticCommands(ProcessedCommand processed)
		{
			if (!processed.Valid || processed.Failed)
				return new EngineCommand[0];

			var commands = new List<EngineCommand>();
			var matches = _options.AutomaticCommands
				.Where(auto => processed.CommandCategories.Contains(auto.SequenceCategory)
					&& (!auto.CompletedOnly || processed.Completed));
			foreach (var match in matches)
			{
				var entitiesMatching = _data.Entities.Where(x => match.Filter(x));
				foreach (var entityMatching in entitiesMatching)
				{
					var command = new EngineCommand();
					command.MainCommand = match.Command;
					command.Inputs = match.DefaultInputs;
					if (processed.CategoryParams.TryGetValue(match.SequenceCategory, out var extraInputs))
					{
						foreach (var kvp in extraInputs)
						{
							command.Inputs[kvp.Key] = kvp.Value;
						}
					}
					if (!string.IsNullOrWhiteSpace(match.AliasEntitiesAs))
						command.Entities.Add(entityMatching.CloneAs(match.AliasEntitiesAs));
					else
						command.Entities.Add(entityMatching);
					commands.Add(command);
				}
			}
			return commands;
		}
	}
}
