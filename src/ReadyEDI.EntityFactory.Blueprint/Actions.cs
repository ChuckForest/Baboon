using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Actions : Collection<Action>
	{
		public Actions()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Action>(this);
		}
	}
}
