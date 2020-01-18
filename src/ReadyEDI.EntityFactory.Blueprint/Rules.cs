using ReadyEDI.EntityFactory;
using ReadyEDI.EntityFactory.Blueprint;
using ReadyEDI.EntityFactory.Data;

namespace BluePrintORM.Library.BluePrint.ReadyEDI.BluePrintORM
{
	public class Rules : Collection<Rule>
	{
		public Rules()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Rule>(this);
		}
	}
}
