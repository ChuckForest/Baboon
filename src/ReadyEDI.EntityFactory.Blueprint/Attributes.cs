using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Attributes : Collection<Attribute>
	{
		public Attributes()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Attribute>(this);
		}
	}
}
