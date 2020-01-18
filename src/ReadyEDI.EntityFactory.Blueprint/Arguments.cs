using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Arguments : Collection<Argument>
	{
		public Arguments()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Argument>(this);
		}
	}
}
