using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class FieldPresentationCollection : Collection<FieldPresentation>
	{
		public FieldPresentationCollection()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<FieldPresentation>(this);
		}
	}
}
