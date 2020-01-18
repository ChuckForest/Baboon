using ReadyEDI.EntityFactory.Data;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class DataTypes : Collection<DataType>
	{
		public DataTypes()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<DataType>(this);
		}
	}
}
