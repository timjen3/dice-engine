using System.Collections.Generic;
using NUnit.Framework;
using TTRPG.Engine.Equations;
using TTRPG.Engine.SequenceItems;

namespace TTRPG.Engine.Tests
{
	[TestFixture]
	[TestOf(typeof(ResultItem))]
	public class ResultItemTests
	{
		EquationService EquationService;
		Dictionary<string, string> Inputs;
		List<Entity> Entities;

		[SetUp]
		public void SetupTest()
		{
			EquationService = new EquationService(null);
			Inputs = new Dictionary<string, string>();
			Entities = new List<Entity>();
		}

		[Test]
		public void ProcessResults_KeyFoundInResults_Added()
		{
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			Inputs["c"] = "1";

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Entities);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.That(resultItems[0].Result, Is.EqualTo("1"));
		}

		[Test]
		public void ProcessResults_KeyMissingFromResults_NotAdded()
		{
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Entities);

			Assert.That(resultItems, Is.Empty);
		}

		[Test]
		public void ProcessResults_KeyFoundInResultsWithEntity_AddedEntitySet()
		{
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			Inputs["c"] = "1";
			item.EntityName = "d";
			Entities.Add(new Entity("d"));

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Entities);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.AreSame(Entities[0], resultItems[0].Entity);
		}

		[Test]
		public void ProcessResults_EntityNotFound_EntityIsNull()
		{
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			Inputs["c"] = "1";
			item.EntityName = null;
			Entities.Add(new Entity("d", null, null));

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Entities);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.That(resultItems[0].Entity, Is.Null);
		}

		[Test]
		public void ProcessResults_FirstEntitySelected_FirstEntityChosen()
		{
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			item.FirstEntity = true;
			Inputs["c"] = "1";
			item.EntityName = null;
			Entities.Add(new Entity("d", null, null));

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Entities);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.AreSame(Entities[0], resultItems[0].Entity);
		}

		// Format Messages
		[Test]
		public void ProcessResults_ValidFormatMessage_FormatMessageIsSet()
		{
			var testValue = "something";
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			item.FirstEntity = true;
			item.FormatMessage = "{d}";
			Inputs["c"] = "1";
			Inputs["d"] = testValue;
			item.EntityName = null;
			Entities.Add(new Entity("e", null, null));

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Entities);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.AreEqual(testValue, resultItems[0].FormatMessage);
		}

		[Test]
		public void ProcessResults_UnknownFormatMessageKey_FormatMessageIsSet()
		{
			var testValue = "something";
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			item.FirstEntity = true;
			item.FormatMessage = "{d}";
			item.EntityName = null;
			Entities.Add(new Entity("e", null, null));

			Assert.Throws<KeyNotFoundException>(() => EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Entities));
		}

		[Test]
		public void ProcessResults_PlainFormatMessage_FormatMessageIsSet()
		{
			var testValue = "something";
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			item.FirstEntity = true;
			item.FormatMessage = testValue;
			Inputs["c"] = "1";
			item.EntityName = null;
			Entities.Add(new Entity("e", null, null));

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Entities);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.AreEqual(testValue, resultItems[0].FormatMessage);
		}
	}
}
