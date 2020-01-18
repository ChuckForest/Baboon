using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class FieldTypes : Collection<FieldType>
	{
		public FieldTypes()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<FieldType>(this);
		}
	}
}
