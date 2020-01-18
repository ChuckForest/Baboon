using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Bases : Collection<Base>
	{
		public Bases()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Base>(this);
		}
	}
}
