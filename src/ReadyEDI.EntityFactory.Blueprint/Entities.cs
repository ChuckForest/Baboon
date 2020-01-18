using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Entities : Collection<Entity>
	{
		public Entities()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Entity>(this);
		}
	}
}
