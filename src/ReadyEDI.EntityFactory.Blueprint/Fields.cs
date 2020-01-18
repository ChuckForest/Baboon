using ReadyEDI.EntityFactory;
using ReadyEDI.EntityFactory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public class Fields : Collection<Field>
	{
		public Fields()
		{

		}

		public void Load()
		{
			CRUDActions.Retrieve<Field>(this);
		}
	}
}
