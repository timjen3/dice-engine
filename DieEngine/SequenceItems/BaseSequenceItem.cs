﻿using DieEngine.Equations;
using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	public abstract class BaseSequenceItem : ISequenceItem
	{
		public string Name { get; set; }
		public string Equation { get; set; }
		public bool PublishResult { get; set; } = true;

		protected SequenceItemResult GetResult(int order, IEquationResolver equationResolver, IDictionary<string, string> inputs)
		{
			var result = new SequenceItemResult();
			result.Order = order;
			result.Inputs = inputs;
			result.ResolvedItem = this;
			result.Result = equationResolver.Process(Equation, inputs).ToString();

			return result;
		}

		public abstract SequenceItemResult GetResult(int order, IEquationResolver equationResolver, ref Dictionary<string, string> mappedInputs, IDictionary<string, string> sharedInputs);
	}
}
