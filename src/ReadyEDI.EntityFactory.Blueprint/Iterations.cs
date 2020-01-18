using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class IterationCollection : Collection<Iteration>
	{
		public IterationCollection()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Iteration>(this);
		}
	}
}
