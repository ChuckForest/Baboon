using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Extensions : Collection<Extension>
	{
		public Extensions()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Extension>(this);
		}
	}
}
